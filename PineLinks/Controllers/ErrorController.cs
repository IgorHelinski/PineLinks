using Microsoft.AspNetCore.Mvc;

namespace PineLinks.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
