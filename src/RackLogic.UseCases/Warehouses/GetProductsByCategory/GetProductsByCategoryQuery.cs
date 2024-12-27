using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetProductsByCategory;
public record GetProductsByCategoryQuery(Guid ManufacturerId, string Category) : IQuery<Result<List<ProductDTO>>>;
