using Microsoft.AspNetCore.Mvc;

namespace PineLinks.Controllers
{
    public class LinkController : Controller
    {
        public IActionResult DeleteLink()
        {
            return View();
        }

        public IActionResult EditLink()
        {
            return View();
        }

        public IActionResult AddLink()
        {
            return View();
        }
    }
}
