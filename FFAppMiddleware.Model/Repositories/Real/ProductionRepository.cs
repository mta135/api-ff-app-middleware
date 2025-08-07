using FFAppMiddleware.Model.DataScheme;
using FFAppMiddleware.Model.Models.Products;
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

        public List<ProductDescriptionApiModel> GetProductDescriptions()
        {
            List<ProductDescriptionApiModel> productDescriptions = _db.ProductDescriptions.OrderBy(pd => pd.Description)
                .Select(x => new ProductDescriptionApiModel
                {
                    ProductDescriptionId = x.ProductDescriptionId,
                    Description = x.Description,

                    Rowguid = x.Rowguid,
                    ModifiedDate = x.ModifiedDate

                }).ToList();

            return productDescriptions;
        }
    }
}
