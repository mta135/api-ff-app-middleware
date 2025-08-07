using FFAppMiddleware.Model.DataScheme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Repositories.Abstract
{
    public interface IProductionRepository
    {
        List<ProductDescription> GetProductDescriptions();
    }
}
