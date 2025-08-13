using FFAppMiddleware.Model.Models.Products;

namespace FFappMiddleware.ApplicationServices.Services.Abstract
{
    public interface IProductService
    {
        Task<List<ProductApiModel>> RetrieveProducts();
    }
}
