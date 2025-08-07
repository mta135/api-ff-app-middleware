using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    public class ProductionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
