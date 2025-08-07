using FFAppMiddleware.Model.DataScheme;
using FFAppMiddleware.Model.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Repositories.Abstract
{
    public interface IProductionRepository
    {
        List<ProductDescriptionApiModel> GetProductDescriptions();
    }
}
