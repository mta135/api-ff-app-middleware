using FFappMiddleware.ApplicationServices.Services.Abstract;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.Products;

namespace FFappMiddleware.ApplicationServices.Services.Real
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductApiModel>> RetrieveProducts()
        {
            List<ProductApiModel> products = await _productRepository.RetrieveProducts();
            return products;
        }
    }
   
}
