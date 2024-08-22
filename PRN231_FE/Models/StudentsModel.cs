//using Microsoft.AspNetCore.Mvc.RazorPages;
//using PRN231_API.DTO;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace PRN231_FE.Models
//{
//    public class StudentsModel : PageModel
//    {
//        private readonly IHttpClientFactory _clientFactory;

//        public StudentsModel(IHttpClientFactory clientFactory)
//        {
//            _clientFactory = clientFactory;
//        }

//        public IList<StudentDTO> Students { get; set; } = new List<StudentDTO>();
//        public int SubjectId { get; set; }

//        public async Task OnGetAsync(int subjectId)
//        {
//            var client = _clientFactory.CreateClient();
//            var response = await client.GetAsync($"http://localhost:5231/api/Student/subject/{subjectId}/students");

//            if (response.IsSuccessStatusCode)
//            {
//                var json = await response.Content.ReadAsStringAsync();
//                // Log or inspect the raw JSON if needed
//                // Console.WriteLine(json);

//                try
//                {
//                    Students = JsonSerializer.Deserialize<List<StudentDTO>>(json, new JsonSerializerOptions
//                    {
//                        PropertyNameCaseInsensitive = true,
//                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
//                    }) ?? new List<StudentDTO>();
//                }
//                catch (JsonException ex)
//                {
//                    // Handle JSON parsing errors
//                    // Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
//                }
//            }
//            else
//            {
//                // Handle unsuccessful status code
//                // Console.WriteLine($"Error: {response.StatusCode}");
//            }
//        }
//    }
//}
