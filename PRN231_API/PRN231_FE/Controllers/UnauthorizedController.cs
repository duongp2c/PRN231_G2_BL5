using Microsoft.AspNetCore.Mvc;

namespace PRN231_FE.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
