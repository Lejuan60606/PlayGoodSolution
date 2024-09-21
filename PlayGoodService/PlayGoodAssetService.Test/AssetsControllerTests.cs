

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PlayGoodService.Controllers;
using PlayGoodService.Models;
using PlayGoodService.Services;

namespace PlayGoodService.Test
{
    [TestFixture]
    internal class AssetsControllerTests
    {
        private AssetsController _controller;
        private Mock<IAssetService> _mockService;
        private ILogger<AssetsController> _logger;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IAssetService>();
            _logger = NullLogger<AssetsController>.Instance;
            _controller = new AssetsController(_mockService.Object, _logger);
        }

        [Test]
        public async Task GetAllAssetMetadata_AssetsExist_ReturnsOkResultWithAssets()
        {
            var mockAssets = new List<AssetMetadata>
            {
                new AssetMetadata { AssetId = "ASSET001" },
                new AssetMetadata { AssetId = "ASSET002" }
            };

            _mockService.Setup(service => service.GetAllAssetsAsync())
                .ReturnsAsync(mockAssets);
           
            var result = await _controller.GetAllAssetMetadata();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var assets = okResult.Value as IEnumerable<AssetMetadata>;
            Assert.IsNotNull(assets);
            Assert.That(assets.Count(), Is.EqualTo(2));
            _mockService.Verify(service => service.GetAllAssetsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllAssetMetadata_NoAssets_ReturnsNoContentResult()
        {
            var emptyAssets = new List<AssetMetadata>();

            _mockService.Setup(service => service.GetAllAssetsAsync())
                .ReturnsAsync(emptyAssets);

            var result = await _controller.GetAllAssetMetadata();

            Assert.IsInstanceOf<NoContentResult>(result);
            _mockService.Verify(service => service.GetAllAssetsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllAssetMetadata_ServiceThrowsException_ReturnsInternalServerError()
        {
            _mockService.Setup(service => service.GetAllAssetsAsync())
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.GetAllAssetMetadata();

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult.Value, Is.EqualTo("Internal server error."));
            _mockService.Verify(service => service.GetAllAssetsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAssetById_ExistingId_ReturnsOkResultWithAsset()
        {
            var assetId = "ASSET001";
            var mockAsset = new AssetMetadata { AssetId = assetId };

            _mockService.Setup(service => service.GetAssetByIdAsync(assetId))
                .ReturnsAsync(mockAsset);

            var result = await _controller.GetAssetById(assetId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var asset = okResult.Value as AssetMetadata;
            Assert.IsNotNull(asset);
            Assert.That(asset.AssetId, Is.EqualTo(assetId));
            _mockService.Verify(service => service.GetAssetByIdAsync(assetId), Times.Once);
        }

        [Test]
        public async Task GetAssetById_NonExistingId_ReturnsNotFoundResult()
        {
            var assetId = "ASSET123";

            _mockService.Setup(service => service.GetAssetByIdAsync(assetId))
                .ReturnsAsync((AssetMetadata)null);

            var result = await _controller.GetAssetById(assetId);

            Assert.IsInstanceOf<NotFoundResult>(result);
            _mockService.Verify(service => service.GetAssetByIdAsync(assetId), Times.Once);
        }

        [Test]
        public async Task AddAsset_ValidAsset_ReturnsCreatedAtActionResult()
        {
            var newAsset = new AssetMetadata { AssetId = "ASSET003", Name = "New Asset" };

            _controller.ModelState.Clear();

            _mockService.Setup(service => service.AddAssetAsync(newAsset))
                .Returns(Task.CompletedTask);

            var result = await _controller.AddAsset(newAsset);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            Assert.That(createdAtResult.ActionName, Is.EqualTo(nameof(_controller.GetAssetById)));
            Assert.That(createdAtResult.RouteValues["id"], Is.EqualTo(newAsset.AssetId));
            _mockService.Verify(service => service.AddAssetAsync(newAsset), Times.Once);
        }

        [Test]
        public async Task AddAsset_NullAsset_ReturnsBadRequest()
        {
            AssetMetadata nullAsset = null;

            var result = await _controller.AddAsset(nullAsset);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("assetMetadata object is null."));
            _mockService.Verify(service => service.AddAssetAsync(It.IsAny<AssetMetadata>()), Times.Never);
        }

        [Test]
        public async Task AddAsset_InvalidModelState_ReturnsBadRequest()
        {
            var newAsset = new AssetMetadata { AssetId = "" }; 
            _controller.ModelState.AddModelError("AssetId", "AssetId is required.");

            var result = await _controller.AddAsset(newAsset);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid model object."));
            _mockService.Verify(service => service.AddAssetAsync(It.IsAny<AssetMetadata>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsset_ValidAsset_ReturnsNoContent()
        {
            var assetId = "ASSET001";
            var updatedAsset = new AssetMetadata { AssetId = assetId, Name = "new name" };

            _controller.ModelState.Clear();

            _mockService.Setup(service => service.UpdateAssetAsync(assetId, updatedAsset))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateAsset(assetId, updatedAsset);

            Assert.IsInstanceOf<NoContentResult>(result);
            _mockService.Verify(service => service.UpdateAssetAsync(assetId, updatedAsset), Times.Once);
        }

        [Test]
        public async Task UpdateAsset_ServiceThrowsArgumentException_ReturnsBadRequest()
        {
            var assetId = "ASSET001";
            var updatedAsset = new AssetMetadata { AssetId = "ASSET002" };

            _controller.ModelState.Clear();

            _mockService.Setup(service => service.UpdateAssetAsync(assetId, updatedAsset)).ThrowsAsync(new ArgumentException("Asset ID mismatch."));


            var result = await _controller.UpdateAsset(assetId, updatedAsset);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Asset ID mismatch."));
            _mockService.Verify(service => service.UpdateAssetAsync(assetId, updatedAsset), Times.Once);
        }

        [Test]
        public async Task UpdateAsset_ServiceThrowsKeyNotFoundException_ReturnsNotFound()
        {
            var assetId = "ASSET123";
            var updatedAsset = new AssetMetadata { AssetId = assetId, Name = "Non-Existing Asset" };

            _controller.ModelState.Clear();

            _mockService.Setup(service => service.UpdateAssetAsync(assetId, updatedAsset))
                .ThrowsAsync(new KeyNotFoundException("Asset not found."));

            var result = await _controller.UpdateAsset(assetId, updatedAsset);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.That(notFoundResult.Value, Is.EqualTo("Asset not found."));
            _mockService.Verify(service => service.UpdateAssetAsync(assetId, updatedAsset), Times.Once);
        }


        [Test]
        public async Task DeleteAsset_ExistingId_ReturnsNoContent()
        {
            var assetId = "ASSET001";

            _mockService.Setup(service => service.DeleteAssetAsync(assetId))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteAsset(assetId);

            Assert.IsInstanceOf<NoContentResult>(result);
            _mockService.Verify(service => service.DeleteAssetAsync(assetId), Times.Once);
        }

        [Test]
        public async Task DeleteAsset_NonExistingId_ReturnsNotFound()
        {
            var assetId = "ASSET123";

            _mockService.Setup(service => service.DeleteAssetAsync(assetId))
                .ThrowsAsync(new KeyNotFoundException("Asset not found."));

            var result = await _controller.DeleteAsset(assetId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("Asset not found."));
            _mockService.Verify(service => service.DeleteAssetAsync(assetId), Times.Once);
        }


    }



}
