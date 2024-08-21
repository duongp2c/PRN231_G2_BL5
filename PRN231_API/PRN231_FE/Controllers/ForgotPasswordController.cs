using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_FE.Controllers
{
    public class ForgotPasswordController : Controller
    {
        // GET: ForgotPasswordController
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ResetPass(string token)
        {
            // You might want to pass the token to the view
            ViewBag.Token = token;

            // Ensure your view is correctly set up to handle this
            return View("Views/ForgotPassword/ResetPass.cshtml");
        }

        // GET: ForgotPasswordController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForgotPasswordController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ForgotPasswordController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ForgotPasswordController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ForgotPasswordController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ForgotPasswordController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
