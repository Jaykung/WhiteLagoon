using Microsoft.AspNetCore.Mvc;

namespace WhiteLagoon.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult E_NotFound()
        {
            return View();
        }
    }
}
