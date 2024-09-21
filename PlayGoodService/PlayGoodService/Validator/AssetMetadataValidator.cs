using FluentValidation;
using PlayGoodAssetService.Models;

namespace PlayGoodAssetService.Validator
{
    public class AssetMetadataValidator : AbstractValidator<AssetMetadata>
    {

        public AssetMetadataValidator()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId is required.")
                .Matches("ASSET\\d{3}").WithMessage("AssetId does not follow the format: Asset followed by 3 digits.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "Approved" || status == "Pending" || status == "Rejected")
                .WithMessage("Status must be 'Approved', 'Pending', or 'Rejected'.");
        }
    }
}
