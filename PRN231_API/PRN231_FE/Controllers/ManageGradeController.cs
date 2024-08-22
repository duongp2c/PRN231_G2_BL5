using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace PRN231_FE.Controllers
{
    public class ManageGradeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public ManageGradeController(IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                // Check if the user has the "Student" role
                if (roleClaim==null)
                {
                    return RedirectToAction("Error", "Unauthorized");
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Unauthorized");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateGrade()
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                // Check if the user has the "Student" role
                if (roleClaim == null)
                {
                    return RedirectToAction("Error", "Unauthorized");
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Unauthorized");
            }
        }

        public async Task<IActionResult> DeleteGrade(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var token = HttpContext.Session.GetString("AuthToken");

                // Set the authorization header for the HttpClient
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var url = $"http://localhost:5000/api/ManageGradeType/{id}";

                var response = await _httpClient.DeleteAsync(url);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Unauthorized");
            }
        }
    }
}
