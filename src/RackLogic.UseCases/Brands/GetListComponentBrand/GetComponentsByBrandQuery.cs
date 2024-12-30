using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Brands.GetListComponentBrand;
public record GetComponentsByBrandQuery(Guid BrandId, int PageNumber, int PageSize) : IQuery<Result<PagedDataDTO<ListComponentBrandDTO>>>;
