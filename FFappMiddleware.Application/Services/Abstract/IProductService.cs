using FFAppMiddleware.Model.Models.Products;

namespace FFappMiddleware.Application.Services.Abstract
{
    public interface IProductService
    {
        //Task<List<ProductModel>> RetrieveProducts();

        Task<List<ProductCategoriesModel>> ProductsCategories(string lang);

        Task<ProductResponse> GetAllProducts(ProductRequest filter);

        Task<List<ProductModel>> GetPromotionsForProductId(List<long> ids);
        Task<List<long>> GetBestSellingProducts();
        
    }
   
}
