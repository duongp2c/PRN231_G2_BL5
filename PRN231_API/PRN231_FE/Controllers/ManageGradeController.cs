using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace PRN231_FE.Controllers
{
    public class ManageGradeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ManageGradeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateGrade()
        {
            return View();
        }

        public async Task<IActionResult> DeleteGrade(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"http://localhost:5000/api/ManageGradeType/{id}";

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
