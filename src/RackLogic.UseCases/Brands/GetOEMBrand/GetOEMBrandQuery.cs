using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Brands.GetOEMBrand;
public record GetOEMBrandQuery() : IQuery<Result<List<OEMBrandDTO>>>;
