using FFAppMiddleware.Model.Models.Products;

namespace FFappMiddleware.Application.Services.Abstract
{
    public interface IProductService
    {
        Task<List<ProductApiModel>> RetrieveProducts();

        Task<List<ProductcCategoriesApiModel>> GetProductsCategories();
    }
}
