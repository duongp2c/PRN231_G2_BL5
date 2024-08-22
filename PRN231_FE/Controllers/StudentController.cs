using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PRN231_FE.Models; // Ensure the DTO namespace is correct
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
        public async Task<IActionResult> EditEvaluation(int evaluationId, int studentId, string studentName, int subjectId, string subjectName, int gradeTypeId, string gradeTypeName)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5231/api/Student/{studentId}/evaluations/{evaluationId}");
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("API response is empty or null.");
                return View(new EvaluationDTO());
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
                    // Setting ViewBag properties manually
                    ViewBag.StudentName = studentName;
                    ViewBag.SubjectName = subjectName;
                    ViewBag.GradeTypeName = gradeTypeName;

                    // Manually set the values in the model if needed
                    //evaluation.StudentName = studentName;
                    //evaluation.SubjectName = subjectName;
                    //evaluation.GradeTypeName = gradeTypeName;

                    return View(evaluation);
                }

                _logger.LogWarning("Evaluation data does not match the provided parameters.");
                return View(new EvaluationDTO());
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize JSON.");
                return View(new EvaluationDTO());
            }
        }





        // PUT: /Student/UpdateEvaluation
        // POST: /Student/UpdateEvaluation
        [HttpPost]
        public async Task<IActionResult> UpdateEvaluation(EvaluationDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "There are some errors in the form.";
                return View("EditEvaluation", model);
            }

            // Assuming _httpClient is injected in the constructor
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("http://localhost:5231/api/Student/evaluation", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Evaluation updated successfully.";
                return RedirectToAction("StudentEvaluations", new { studentId = model.StudentId });
            }
            else
            {
                ViewBag.ErrorMessage = $"API request failed with status code: {response.StatusCode}";
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Response content: {ResponseContent}", responseContent);
                return View("EditEvaluation", model);
            }
        }








    }
}
