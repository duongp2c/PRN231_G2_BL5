using Microsoft.AspNetCore.Mvc;

namespace PRN231_FE.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Views/Home/Index1.cshtml");
        }
    }
}
