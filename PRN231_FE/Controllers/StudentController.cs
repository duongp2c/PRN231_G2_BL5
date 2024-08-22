using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PRN231_API.DTO; // Ensure the DTO namespace is correct
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PRN231_FE.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly HttpClient _httpClient;

        public StudentController(ILogger<StudentController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: /Student/StudentList/subjectId
        public async Task<IActionResult> StudentList(int subjectId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5231/api/student/subject/{subjectId}/students");
            var json = await response.Content.ReadAsStringAsync();

            // Log the raw JSON response
            _logger.LogInformation("API response: {JsonResponse}", json);

            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("API response is empty or null.");
                return View(new List<StudentDTO>()); // Return an empty list or appropriate response
            }

            try
            {
                var students = System.Text.Json.JsonSerializer.Deserialize<List<StudentDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (students == null)
                {
                    _logger.LogWarning("Deserialization resulted in a null value.");
                }

                return View(students ?? new List<StudentDTO>()); // Handle null students
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize JSON. Raw JSON: {JsonResponse}", json);
                return View(new List<StudentDTO>()); // Handle the error and return an appropriate response
            }
        }


        // GET: /Student/StudentEvaluations/{studentId}
        public async Task<IActionResult> StudentEvaluations(int studentId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5231/api/student/{studentId}/evaluations");
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("API response is empty or null.");
                return View(new List<EvaluationDTO>()); // Return an empty list or appropriate response
            }

            try
            {
                var evaluations = System.Text.Json.JsonSerializer.Deserialize<List<EvaluationDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(evaluations);
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize JSON.");
                return View(new List<EvaluationDTO>()); // Handle the error and return an appropriate response
            }
        }
        // GET: /Student/EditEvaluation/{evaluationId}
        [HttpGet("EditEvaluation/{evaluationId}")]
        public async Task<IActionResult> EditEvaluation(int evaluationId, int studentId, int subjectId, int gradeTypeId)
        {
            // Fetch the evaluation details using evaluationId
            var response = await _httpClient.GetAsync($"http://localhost:5231/api/Student/{studentId}/evaluations/{evaluationId}");
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("API response is empty or null.");
                return View(new EvaluationDTO()); // Return an empty DTO or handle appropriately
            }

            try
            {
                var evaluation = System.Text.Json.JsonSerializer.Deserialize<EvaluationDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (evaluation != null &&
                    evaluation.StudentId == studentId &&
                    evaluation.SubjectId == subjectId &&
                    evaluation.GradeTypeId == gradeTypeId)
                {
                    return View(evaluation);
                }

                // Handle mismatch case or missing data
                _logger.LogWarning("Evaluation data does not match the provided parameters.");
                return View(new EvaluationDTO());
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize JSON.");
                return View(new EvaluationDTO()); // Handle the error and return an appropriate response
            }
        }


        // PUT: /Student/UpdateEvaluation
        // POST: /Student/UpdateEvaluation
        [HttpPost]
        public async Task<IActionResult> UpdateEvaluation(EvaluationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditEvaluation", model); // Return to the edit page with validation errors
            }

            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"http://localhost:5231/api/student/evaluation", content);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to the StudentEvaluations action and refresh the page
                    return RedirectToAction("StudentEvaluations", new { studentId = model.StudentId });
                }
                else
                {
                    _logger.LogWarning($"API request failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Response content: {errorContent}");
                    ViewBag.ErrorMessage = "Failed to update the evaluation. Please try again.";
                    return View("EditEvaluation", model);
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Failed to serialize JSON.");
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View("EditEvaluation", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                ViewBag.ErrorMessage = "An unexpected error occurred. Please try again.";
                return View("EditEvaluation", model);
            }
        }







    }
}
