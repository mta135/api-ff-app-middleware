using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.Products;

namespace FFappMiddleware.Application.Services.Real
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

 
        //public async Task<List<ProductModel>> RetrieveProducts()
        //{
        //    List<ProductModel> products = await _productRepository.RetrieveProducts();
        //    return products;
        //}

        public async Task<List<ProductCategoriesModel>> ProductsCategories(string lang)
        {
            List<ProductCategoriesModel> products = await _productRepository.ProductsCategories(lang);
            return products;
        }


        public async Task<ProductResponse> GetAllProducts(ProductRequest filter)
        {
            ProductResponse products = await _productRepository.GetAllProducts(filter);
            return products;
        }

        public async Task<List<ProductModel>> GetPromotionsForProductId(List<long> ids)
        {
            List<ProductModel> products = await _productRepository.GetPromotionsForProductId(ids);
            return products;
        }

        public async Task<List<long>> GetBestSellingProducts()
        {
            List<long> products = await _productRepository.GetBestSellingProducts();
            return products;
        }
    }
   
}
