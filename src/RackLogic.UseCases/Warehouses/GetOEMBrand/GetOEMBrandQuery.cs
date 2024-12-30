using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Warehouses.GetOEMBrand;
public record GetOEMBrandQuery() : IQuery<Result<List<OEMBrandDTO>>>;
