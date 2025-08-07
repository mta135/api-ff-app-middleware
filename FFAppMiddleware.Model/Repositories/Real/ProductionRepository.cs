using FFAppMiddleware.Model.DataScheme;
using FFAppMiddleware.Model.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Repositories.Real
{
    public class ProductionRepository : IProductionRepository
    {
        private AdventureWorksDbContext _db;

        public ProductionRepository()
        {
            _db = new AdventureWorksDbContext();
        }

        public List<ProductDescription> GetProductDescriptions()
        {
            List<ProductDescription> productDescriptions = _db.ProductDescriptions.OrderBy(pd => pd.Description).ToList();
            return productDescriptions;
        }
    }
}
