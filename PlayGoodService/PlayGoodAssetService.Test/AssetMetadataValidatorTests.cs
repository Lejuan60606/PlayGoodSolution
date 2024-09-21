
using FluentValidation.TestHelper;
using PlayGoodService.Models;
using PlayGoodService.Validator;

namespace PlayGoodService.Test
{
    [TestFixture]
    internal class AssetMetadataValidatorTests
    {
        private AssetMetadataValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new AssetMetadataValidator();
        }

        [Test]
        public void Validate_ValidAssetMetadata_ShouldPassValidation()
        {
            var model = new AssetMetadata
            {
                AssetId = "ASSET123",
                Name = "Name",
                Status = "Approved"
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validate_EmptyAssetId_ShouldHaveValidationError()
        {
            var model = new AssetMetadata
            {
                AssetId = "",
                Name = "Name",
                Status = "Approved"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.AssetId)
                .WithErrorMessage("AssetId is required.");
        }

        [Test]
        public void Validate_InvalidAssetIdFormat_ShouldHaveValidationError()
        {
            var model = new AssetMetadata
            {
                AssetId = "qwer123",
                Name = "Name",
                Status = "Approved"
            };
            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.AssetId)
                .WithErrorMessage("AssetId does not follow the format: Asset followed by 3 digits.");
        }

        [Test]
        public void Validate_EmptyName_ShouldHaveValidationError()
        {
            var model = new AssetMetadata
            {
                AssetId = "ASSET123",
                Name = "",
                Status = "Approved"
            };

            var result = _validator.TestValidate(model);


            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required.");
        }

        [Test]
        public void Validate_NameExceedsMaxLength_ShouldHaveValidationError()
        {
            var model = new AssetMetadata
            {
                AssetId = "ASSET123",
                Name = new string('A', 101), 
                Status = "Approved"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name cannot exceed 100 characters.");
        }

        [Test]
        public void Validate_EmptyStatus_ShouldHaveValidationError()
        {
            var model = new AssetMetadata
            {
                AssetId = "ASSET123",
                Name = "Name",
                Status = ""
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("Status is required.");
        }

        [Test]
        public void Validate_InvalidStatus_ShouldHaveValidationError()
        {
            var model = new AssetMetadata
            {
                AssetId = "ASSET123",
                Name = "Name",
                Status = "InvalidStatus"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("Status must be 'Approved', 'Pending', or 'Rejected'.");
        }


    }
}
