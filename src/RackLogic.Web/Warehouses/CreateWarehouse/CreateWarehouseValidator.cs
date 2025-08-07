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
            .Length(11)
            .Matches(@"^\d{11}$")
            .When(r => !string.IsNullOrEmpty(r.ABN))
            .WithMessage("ABN must be exactly 11 numeric characters.");

    RuleFor(r => r.ClientPhoneNumber)
           .NotEmpty()
           .Matches(@"^\d{10}$")
           .WithMessage("Invalid phone number format.");

    RuleFor(r => r.OwnerPhoneNumber)
           .Matches(@"^\d{10}$")
           .When(r => !string.IsNullOrEmpty(r.OwnerPhoneNumber))
           .WithMessage("Invalid phone number format.");

    RuleFor(r => r.OwnerEmailAddress)
          .EmailAddress()
          .When(r => !string.IsNullOrEmpty(r.OwnerEmailAddress))
          .WithMessage("Email is invalid.");
  }
}
