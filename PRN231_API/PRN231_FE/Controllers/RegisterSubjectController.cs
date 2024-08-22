using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN231_FE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_FE.Controllers
{
    public class RegisterSubjectController : Controller
    {
        private readonly HttpClient _httpClient;

        public RegisterSubjectController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: RegisterSubjectController
        // GET: RegisterSubjectController
        public async Task<ActionResult> RegisterSubject()
        {
            try
            {
                // Retrieve AccountId from the session
                var accountId = HttpContext.Session.GetString("AccountId");
                // Check if AccountId is null, if so, return a login alert
                if (string.IsNullOrEmpty(accountId))
                {
                    TempData["AlertMessage"] = "Bạn cần đăng nhập để tiếp tục.";
                    return RedirectToAction("Login", "Account");
                }

                // Retrieve the token from the session
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["AlertMessage"] = "Token không hợp lệ.";
                    return RedirectToAction("Login", "Account");
                }

                // Validate role from the token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                // Check if the user has the "Student" role
                if (roleClaim != "Student")
                {
                    return RedirectToAction("Error", "Unauthorized");
                }

                // Set the authorization header for the HttpClient
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Call the API to get the subject data
                var subjects = await _httpClient.GetFromJsonAsync<List<SubjectViewModel>>("http://localhost:5000/api/Subject/GetSubjectIdAndName");

                // Call the API to get the subjects the student is currently enrolled in
                var subjectOfAccount = await _httpClient.GetFromJsonAsync<List<SubjectOfAccount>>($"http://localhost:5000/api/Subject/student/{accountId}");

                // Check if there are no subjects
                if (subjectOfAccount == null)
                {
                    ViewBag.SubjectOfAccount = null;
                }

                // Pass the data and AccountId to the view
                ViewBag.AccountId = accountId;
                ViewBag.SubjectOfAccount = subjectOfAccount;

                return View(subjects);
            }
            catch (Exception ex)
            {
                // Handle any errors by logging and returning an empty list or an error message
                Console.WriteLine(ex.Message);
                return View(new List<SubjectViewModel>());
            }
        }

        // GET: RegisterSubjectController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                // Retrieve AccountId and SubjectId from the form data
                var accountId = HttpContext.Session.GetString("AccountId");
                var subjectId = collection["subjectId"];

                // Retrieve the token from the session or other secure storage
                var token = HttpContext.Session.GetString("AuthToken"); // Adjust based on how you're storing the token

                // Create request message with authorization header
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:5000/api/Student/register/{accountId}/{subjectId}");
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Call the API to register the subject
                var response = await _httpClient.SendAsync(requestMessage);

                // Extract response content as string
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to RegisterSubject view with the API's success message
                    TempData["SuccessMessage"] = responseContent; // Pass the success message from the API
                    return RedirectToAction("RegisterSubject");
                }
                else
                {
                    // Set error message with the API's error message
                    TempData["ErrorMessage"] = responseContent; // Pass the error message from the API
                    return RedirectToAction("RegisterSubject");
                }
            }
            catch(Exception ex)
            {// Handle any errors and set a generic error message
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("RegisterSubject");
            }
        }

    }
}

