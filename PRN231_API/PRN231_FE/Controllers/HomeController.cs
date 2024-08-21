using Microsoft.AspNetCore.Mvc;

namespace PRN231_FE.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            Console.WriteLine(accountId);

            return View("Views/Home/Index1.cshtml");
        }
       

    }
}
