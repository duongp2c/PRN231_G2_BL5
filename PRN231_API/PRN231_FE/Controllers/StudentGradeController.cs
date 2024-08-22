using Microsoft.AspNetCore.Mvc;
using PRN231_FE.Models;
using System.Text.Json;
using Newtonsoft.Json;

namespace PRN231_FE.Controllers
{
    public class StudentGradeController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentGradeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            var url = "http://localhost:5000/api/Evaluation/subject/"+accountId;
            List<SubjectDTO> sList = new List<SubjectDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var PResponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<List<SubjectDTO>>(PResponse);
                    //sList = result.subjects;
                    ViewData["sList"] = result;
                }
                return View();

            }
        }
        //http://localhost:5139/StudentGrade/CheckGrade/1
        [HttpGet]
        [ActionName("CheckGrade")]
        public async Task<IActionResult> CheckGrade(string id)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            var url = "http://localhost:5000/api/Evaluation/subject/"+accountId+"/"+id;//studentId(session) then subjectId(url)
            List<GradeDTO> sList = new List<GradeDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var PResponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<List<GradeDTO>>(PResponse);
                    //sList = result.subjects;
                    ViewData["sList"] = result;
                }
                return View("CheckGrade");

            }
        }
    }
}
