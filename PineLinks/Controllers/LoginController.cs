using Microsoft.AspNetCore.Mvc;

namespace PineLinks.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
