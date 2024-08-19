using Microsoft.AspNetCore.Mvc;

namespace PRN231_FE.Controllers
{
    public class RegisterSubjectController : Controller
    {
        public ActionResult Registesubject()
        {
            return View("Views/Student/RegisterSubject.cshtml");
        }
    }
}
