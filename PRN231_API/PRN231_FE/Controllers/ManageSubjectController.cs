using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace PRN231_FE.Controllers
{
    public class ManageSubjectController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ManageSubjectController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateSubject()
        {
            return View();
        }

        public async Task<IActionResult> DeleteSubject(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"http://localhost:5000/api/ManageSubject/{id}";

            var response = await httpClient.DeleteAsync(url);

            // Check if the deletion was successful
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Subject successfully deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the student.";
                return RedirectToAction("Index");
            }
        }
    }
}
