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
            .WithMessage("ABN must be exactly 11 numeric characters.");

    RuleFor(r => r.ClientPhoneNumber)
           .NotEmpty()
           .Matches(@"^0[45]\d{8}$")
           .WithMessage("Invalid phone number format.");

    RuleFor(r => r.OwnerPhoneNumber)
           .Matches(@"^0[45]\d{8}$")
           .WithMessage("Invalid phone number format.");

    RuleFor(r => r.OwnerEmailAddress)
          .EmailAddress()
          .WithMessage("Email is invalid.");
  }
}
