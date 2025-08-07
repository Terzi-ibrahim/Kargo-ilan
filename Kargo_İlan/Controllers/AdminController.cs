using Microsoft.AspNetCore.Mvc;

namespace Kargo_İlan.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
