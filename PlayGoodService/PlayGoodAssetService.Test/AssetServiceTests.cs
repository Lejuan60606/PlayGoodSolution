using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PlayGoodAssetService.Models;
using PlayGoodAssetService.Repositories;
using PlayGoodAssetService.Services;

namespace PlayGoodService.Test
{
    [TestFixture]
    internal class AssetServiceTests
    {
        private AssetService _assetService;
        private Mock<IAssetRepository> _mockRepository;
        private ILogger<AssetService> _logger;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IAssetRepository>();
            _logger = NullLogger<AssetService>.Instance;
            _assetService = new AssetService(_mockRepository.Object, _logger);
        }

        [Test]
        public async Task GetAllAssetsAsync_ShouldReturnAllAssets()
        {
            var mockAssets = new List<AssetMetadata>
            {
                new AssetMetadata { AssetId = "ASSET001" },
                new AssetMetadata { AssetId = "ASSET002" }
            };

            _mockRepository.Setup(repo => repo.GetAllAssetsAsync())
                .ReturnsAsync(mockAssets);

            var result = await _assetService.GetAllAssetsAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            _mockRepository.Verify(repo => repo.GetAllAssetsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAssetByIdAsync_ValidId_ShouldReturnAsset()
        {
            var assetId = "ASSET002";
            var mockAsset = new AssetMetadata { AssetId = assetId };

            _mockRepository.Setup(repo => repo.GetAssetByIdAsync(assetId))
                .ReturnsAsync(mockAsset);

            var result = await _assetService.GetAssetByIdAsync(assetId);

            Assert.IsNotNull(result);
            Assert.That(result.AssetId, Is.EqualTo(assetId));
            _mockRepository.Verify(repo => repo.GetAssetByIdAsync(assetId), Times.Once);
        }

        [Test]
        public async Task GetAssetByIdAsync_InvalidId_ShouldReturnNull()
        {
            var assetId = "ASSET999";

            _mockRepository.Setup(repo => repo.GetAssetByIdAsync(assetId))
                .ReturnsAsync((AssetMetadata)null);

            var result = await _assetService.GetAssetByIdAsync(assetId);

            Assert.IsNull(result);
            _mockRepository.Verify(repo => repo.GetAssetByIdAsync(assetId), Times.Once);
        }

        [Test]
        public async Task AddAssetAsync_ShouldAddAsset()
        {
            var newAsset = new AssetMetadata { AssetId = "ASSET003" };

            _mockRepository.Setup(repo => repo.AddAssetAsync(newAsset))
                .Returns(Task.CompletedTask);

            await _assetService.AddAssetAsync(newAsset);

            _mockRepository.Verify(repo => repo.AddAssetAsync(newAsset), Times.Once);
        }


        [Test]
        public async Task UpdateAssetAsync_ValidAsset_ShouldUpdateAsset()
        {
            var assetId = "ASSET001";
            var updatedAsset = new AssetMetadata { AssetId = assetId, Name = "new name" };

            _mockRepository.Setup(repo => repo.GetAssetByIdAsync(assetId))
                .ReturnsAsync(updatedAsset);
            _mockRepository.Setup(repo => repo.UpdateAssetAsync(updatedAsset))
                .Returns(Task.CompletedTask);

            await _assetService.UpdateAssetAsync(assetId, updatedAsset);

            _mockRepository.Verify(repo => repo.GetAssetByIdAsync(assetId), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateAssetAsync(updatedAsset), Times.Once);
        }

        [Test]
        public void UpdateAssetAsync_MismatchedAssetId_ShouldThrowArgumentException()
        {
            var assetId = "ASSET001";
            var updatedAsset = new AssetMetadata { AssetId = "ASSET002"};

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _assetService.UpdateAssetAsync(assetId, updatedAsset));
            Assert.That(ex.Message, Is.EqualTo("Asset ID mismatch."));
            _mockRepository.Verify(repo => repo.GetAssetByIdAsync(It.IsAny<string>()), Times.Never);
            _mockRepository.Verify(repo => repo.UpdateAssetAsync(It.IsAny<AssetMetadata>()), Times.Never);
        }

        [Test]
        public void UpdateAssetAsync_NonExistingAsset_ShouldThrowKeyNotFoundException()
        {
            var assetId = "ASSET999";
            var updatedAsset = new AssetMetadata { AssetId = assetId};

            _mockRepository.Setup(repo => repo.GetAssetByIdAsync(assetId))
                .ReturnsAsync((AssetMetadata)null);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _assetService.UpdateAssetAsync(assetId, updatedAsset));
            Assert.That(ex.Message, Is.EqualTo("Asset not found."));
            _mockRepository.Verify(repo => repo.GetAssetByIdAsync(assetId), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateAssetAsync(It.IsAny<AssetMetadata>()), Times.Never);
        }

        [Test]
        public async Task DeleteAssetAsync_ShouldDeleteAsset()
        {
            var assetId = "ASSET001";
            var existingAsset = new AssetMetadata { AssetId = assetId };

            _mockRepository.Setup(repo => repo.GetAssetByIdAsync(assetId))
                .ReturnsAsync(existingAsset);
            _mockRepository.Setup(repo => repo.DeleteAssetAsync(assetId))
                .Returns(Task.CompletedTask);

            await _assetService.DeleteAssetAsync(assetId);

            _mockRepository.Verify(repo => repo.DeleteAssetAsync(assetId), Times.Once);
        }

        [Test]
        public async Task DeleteAssetAsync_NonExisting_ShouldReturn()
        {
            var assetId = "ASSET999";
            var updatedAsset = new AssetMetadata { AssetId = assetId };

            _mockRepository.Setup(repo => repo.GetAssetByIdAsync(assetId))
                .ReturnsAsync((AssetMetadata)null);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _assetService.DeleteAssetAsync(assetId));
            Assert.That(ex.Message, Is.EqualTo("Asset not found."));
            _mockRepository.Verify(repo => repo.GetAssetByIdAsync(assetId), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAssetAsync(It.IsAny<string>()), Times.Never);
        }
    }

}
