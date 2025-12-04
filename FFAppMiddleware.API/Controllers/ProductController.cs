using FFappMiddleware.Application.Services.Abstract;
using FFAppMiddleware.API.Authorization;
using FFAppMiddleware.API.Security;
using FFAppMiddleware.Model.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [AuthorizeRoles(CustomUserRoleEnum.RoleAleator)]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductCategoriesModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ProductCategories([FromQuery] string lang)
        {
            try
            {
                List<ProductCategoriesModel> products = await _productService.ProductsCategories(lang);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }



        [HttpPost("ProductPromotions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProductPromotionsByProductId([FromBody] List<long> productIds)
        {
            if (productIds == null || !productIds.Any())
                return BadRequest("Product IDs are required.");

            try
            {
                List<ProductModel> products = await _productService.GetPromotionsForProductId(productIds);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("GetBestSellingProducts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<long>))]
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("GetBestBrands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Brands>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBestBrands()
        {
            try
            {
                List<Brands> brands = await _productService.GetBestBrands();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpGet("GetAllPromotions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PromotionResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllPromotions()
        {
            try
            {
                List<PromotionResponse> products = await _productService.GetAllPromotions();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
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
