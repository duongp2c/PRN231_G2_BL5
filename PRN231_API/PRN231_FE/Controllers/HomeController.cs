using Microsoft.AspNetCore.Mvc;

namespace PRN231_FE.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            ViewBag.AccountId = accountId;

            return View("Views/Home/Index1.cshtml");
        }
       

    }
}
