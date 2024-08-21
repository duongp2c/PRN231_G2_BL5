using Microsoft.AspNetCore.Mvc;
using PRN231_FE.Models;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace PRN231_FE.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProfileController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var url = "http://localhost:5231/api/Student/profile/1";
            ProfileDTO profile = new ProfileDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var PResponse = response.Content.ReadAsStringAsync().Result;
                    profile = JsonSerializer.Deserialize<ProfileDTO>(PResponse);
                    Console.WriteLine(profile);
                    ViewData["name"] = profile.name;
                    ViewData["age"] = profile.age;
                    ViewData["address"] = profile.address;
                    ViewData["additionalInfo"] = profile.additionalInfo;
                    ViewData["phone"] = profile.phone;
                }
                
                
            }
            return View();
        }
        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> Update()
        {
            var name = Request.Form["name"].ToString().Trim();
            //var age = int.Parse(Request.Form["age"]);
            var age = Request.Form["age"].ToString().Trim();
            var address = Request.Form["address"].ToString().Trim();
            var additionalInfo = Request.Form["additionalInfo"].ToString().Trim();
            var phone = Request.Form["phone"].ToString().Trim();
            if (ModelState.IsValid)
            {
                Dictionary<string,string> formData = new Dictionary<string,string>();
                formData["name"] = name;
                formData["address"] = address;
                formData["age"] = age;
                formData["additionalInfo"] = additionalInfo;
                formData["phone"] = phone;
                formData["image"] = "image";
                HttpContent formContent = new FormUrlEncodedContent(formData);
                var response = await _httpClient.PostAsync("http://localhost:5231/api/Student/update/1", formContent);
            }
            return RedirectToAction("Index", "Profile");
        }
    }
}
