using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json; // Use System.Text.Json instead of Newtonsoft.Json

namespace PRN231_FE.Controllers
{
    public class ManageStudentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public ManageStudentController(IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {

            return (View("Index"));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudentStatus(int id, bool isActive)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("AuthToken");

            // Set the authorization header for the HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var url = $"http://localhost:5000/api/ManageStudent/{id}/status?isActive={isActive}";

            var response = await _httpClient.PutAsync(url, null); // No content body is needed for this request

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect back to the list of students after a successful update
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the student status.");
                return View("Error"); // Replace with an appropriate error handling view
            }
        }
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("AuthToken");

            // Set the authorization header for the HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var url = $"http://localhost:5000/api/ManageStudent/{id}";

            // Sending the DELETE request
            var response = await _httpClient.DeleteAsync(url);

            // Check if the deletion was successful
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Student successfully deleted.";
                return RedirectToAction("Index"); // Redirect back to the list of students after successful deletion
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the student.";
                return RedirectToAction("Index"); // Redirect back to the list of students with an error message
            }
        }

    }
}
