using FFappMiddleware.Application.Services.Abstract;
using FFAppMiddleware.Model.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Authorize]
   // /[Route("api/[controller]/[action]")]
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("Products")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllProducts(ProductRequest filter)
        {
            try
            {
                ProductResponse products = await _productService.GetAllProducts(filter);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("ProductCategories")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductCategoriesModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ProductCategories(string lang)
        {
            try
            {
                List<ProductCategoriesModel> products = await _productService.ProductsCategories(lang);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpPost("ProductPromotions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProductPromotionsByProductId([FromBody]List<long> productids)
        {
            try
            {
                List<ProductModel> products = await _productService.GetPromotionsForProductId(productids);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
        [HttpGet("GetBestSellingProducts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBestSellingProducts()
        {
            try
            {
                List<long> products = await _productService.GetBestSellingProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
        




        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> RetrieveProducts()
        //{
        //    try
        //    {
        //        List<ProductModel> products = await _productService.RetrieveProducts();
        //        return Ok(products);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
        //    }
        //}

    }
}
