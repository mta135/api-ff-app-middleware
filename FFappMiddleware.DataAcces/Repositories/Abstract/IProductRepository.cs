using FFAppMiddleware.Model.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFappMiddleware.DataBase.Repositories.Abstract
{
    public interface IProductRepository
    {
        Task<List<ProductApiModel>> RetrieveProducts();

        Task<List<ProductcCategoriesApiModel>> GetProductsCategories();
    }
}
