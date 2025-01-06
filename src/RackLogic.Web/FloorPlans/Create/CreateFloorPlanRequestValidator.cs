using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.FloorPlans.Create;

public class CreateFloorPlanRequestValidator : Validator<CreateFloorPlanRequest>
{
  public CreateFloorPlanRequestValidator()
  {
    RuleFor(x => x.FloorPlans)
      .NotEmpty()
      .WithMessage("FloorPlans is required.");

    RuleForEach(x => x.FloorPlans).ChildRules(floorPlan =>
    {
      floorPlan.RuleForEach(fp => fp.Files).ChildRules(file =>
      {
        file.RuleFor(f => f.ContentType)
            .Must(IsValidFileType)
            .WithMessage("File must be of type PDF, PNG or JPEG.");

        file.RuleFor(f => f.Length)
            .LessThanOrEqualTo(10 * 1024 * 1024) // 10MB
            .WithMessage("File size must not exceed 10MB.");
      });
    });
  }
  private bool IsValidFileType(string? contentType)
  {
    return contentType == "application/pdf" ||   // PDF
           contentType == "image/png" ||        //  PNG
           contentType == "image/jpeg";        //   JPEG
  }
}
