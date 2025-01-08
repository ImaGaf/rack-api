using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Brands.GetAll;
public record GetAllBrandQuery() : IQuery<Result<List<GetAllBrandDTO>>>;
