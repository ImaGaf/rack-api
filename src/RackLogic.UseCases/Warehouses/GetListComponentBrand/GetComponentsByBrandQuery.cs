using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetListComponentBrand;
public record GetComponentsByBrandQuery(Guid BrandId, int PageNumber, int PageSize) : IQuery<Result<PagedDataDTO<ListComponentBrandDTO>>>;
