using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Warehouses.CreateWarehouse;

public class CreateWarehouseValidator : Validator<CreateWarehouseRequest>
{
  public CreateWarehouseValidator()
  {
    RuleFor(r => r.CompanyName)
           .NotEmpty()
           .Matches(@"^[a-zA-Z0-9 ]+$")
           .WithMessage("Company Name can only contain alphanumeric characters.");
    RuleFor(r => r.ABN)
            .NotEmpty()
            .Length(11)
            .Matches(@"^\d{11}$")
            .WithMessage("ABN must be exactly 11 numeric characters.");
    RuleFor(r => r.ClientPhoneNumber)
           .NotEmpty()
           .Matches(@"^0[45]\d{8}$")
           .WithMessage("Invalid phone number format.");
    RuleFor(r => r.OwnerPhoneNumber)
           .NotEmpty()
           .Matches(@"^0[45]\d{8}$")
           .WithMessage("Invalid phone number format.");
    RuleFor(r => r.FileImageLogo)
           .NotNull()
           .WithMessage("ImageLogo is required.")
           .Must(file => file!.Length <= 2 * 1024 * 1024)
           .WithMessage("The file size must be less than 2MB.");

    RuleFor(r => r.OwnerEmailAddress)
          .EmailAddress()
          .WithMessage("Email is invalid.");
  }
}
