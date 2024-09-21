using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PlayGoodAssetService.Data;
using PlayGoodAssetService.Models;
using PlayGoodAssetService.Repositories;

namespace PlayGoodService.Test
{
    [TestFixture]
    public class AssetRepositoryTests
    {
        private AssetRepository _repository;
        private AssetAppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AssetAppDbContext>()
                .UseInMemoryDatabase(databaseName: "AssetDatabase")
                .Options;

            _context = new AssetAppDbContext(options);

            SeedDatabase();

            _repository = new AssetRepository(_context, NullLogger<AssetRepository>.Instance);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            var assets = new List<AssetMetadata>
            {
                new AssetMetadata
                {
                    AssetId = "ASSET001",
                    Name = "Millennium Falcon",
                    Description = "High-resolution concept art of the Millennium Falcon spaceship.",
                    FileFormat = "jpg",
                    FileSize = "1024",
                    Path = "/path/to/asset001.jpg",
                    CreatedBy = "John Doe",
                    VersionNumber = "1.0",
                    Timestamp = DateTime.UtcNow,
                    UserName = "Editor01",
                    Comments = "Approved final version of concept art.",
                    Preview = "/path/to/preview/asset001_preview.jpg",
                    Status = "Approved"
                },
                new AssetMetadata
                {
                    AssetId = "ASSET002",
                    Name = "Lightsaber Photograph",
                    Description = "Photograph of a replica lightsaber.",
                    FileFormat = "png",
                    FileSize = "2048",
                    Path = "/path/to/asset002.png",
                    CreatedBy = "Jane Smith",
                    VersionNumber = "2.0",
                    Timestamp = DateTime.UtcNow,
                    UserName = "Editor06",
                    Comments = "Approved final version of photographs.",
                    Preview = "/path/to/preview/asset002_preview.png",
                    Status = "Approved"
                }
            };

            _context.AssetMetadata.AddRange(assets);
            _context.SaveChanges();
        }


        [Test]
        public async Task GetAllAssetsAsync_ShouldReturnAllAssets()
        {
            var assets = await _repository.GetAllAssetsAsync();

            Assert.IsNotNull(assets);
            Assert.That(assets.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAssetByIdAsync_ExistingId_ShouldReturnAsset()
        {
            var assetId = "ASSET001";

            var asset = await _repository.GetAssetByIdAsync(assetId);

            Assert.IsNotNull(asset);
            Assert.That(asset.AssetId, Is.EqualTo(assetId));
        }

        [Test]
        public async Task GetAssetByIdAsync_InvalidId_ShouldReturnNull()
        {
            var assetId = "ASSET345";

            var asset = await _repository.GetAssetByIdAsync(assetId);

            Assert.IsNull(asset);
        }

        [Test]
        public async Task AddAssetAsync_ShouldAddAsset()
        {
            var newAsset = new AssetMetadata
            {
                AssetId = "ASSET003",
                Name = "Darth Vader Costume",
                Description = "Realistic Darth Vader costume with helmet, cape, and armor.",
                FileFormat = "ToBeDefined",
                FileSize = "ToBeDefined",
                Path = "/path/to/asset003.jpg",
                CreatedBy = "Mark Johnson",
                VersionNumber = "3.0",
                Timestamp = DateTime.UtcNow,
                UserName = "Editor03",
                Comments = "Initial version.",
                Preview = "/path/to/preview/asset003_preview.jpg",
                Status = "Pending"
            };

            await _repository.AddAssetAsync(newAsset);
            var asset = await _repository.GetAssetByIdAsync(newAsset.AssetId);

            Assert.IsNotNull(asset);
            Assert.That(asset.AssetId, Is.EqualTo(newAsset.AssetId));
        }

        [Test]
        public async Task UpdateAssetAsync_ShouldUpdateAsset()
        {
            var assetId = "ASSET001";
            var assetToUpdate = await _repository.GetAssetByIdAsync(assetId);
            assetToUpdate.Name = "New Name";

            await _repository.UpdateAssetAsync(assetToUpdate);
            var updatedAsset = await _repository.GetAssetByIdAsync(assetId);

            Assert.IsNotNull(updatedAsset);
            Assert.That(updatedAsset.Name, Is.EqualTo("New Name"));
        }  


        [Test]
        public async Task DeleteAssetAsync_ShouldDeleteAsset()
        {
            var assetId = "ASSET001";

            await _repository.DeleteAssetAsync(assetId);
            var asset = await _repository.GetAssetByIdAsync(assetId);

            Assert.IsNull(asset);
        }

        [Test]
        public void DeleteAssetAsync_InvalidAssetId_ShouldThrowKeyNotFoundException()
        {
            var assetId = "ASSET999";

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _repository.DeleteAssetAsync(assetId));
            Assert.That(ex.Message, Is.EqualTo("Asset not found."));
        }

    }
}