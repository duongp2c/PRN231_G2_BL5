using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN231_FE.Models;
using System.Text;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;

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
            if (ModelState.IsValid)
            {
                var accountId = HttpContext.Session.GetString("AccountId");
                ViewBag.AccountId = accountId;
                var token = HttpContext.Session.GetString("AuthToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account");
                }
                var handle = new JwtSecurityTokenHandler();
                var JWTToken = handle.ReadJwtToken(token);
                var roleClaim = JWTToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                if(roleClaim != "Student")
                {
                    return RedirectToAction("Error","Unauthorized");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = "http://localhost:5000/api/Student/profile/" + accountId;
                ProfileDTO profile = new ProfileDTO();
                using (var client = new HttpClient())
                {
                   client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    client.BaseAddress = new Uri(url);
                    
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var PResponse = response.Content.ReadAsStringAsync().Result;
                        profile = JsonConvert.DeserializeObject<ProfileDTO>(PResponse);
                        Console.WriteLine(profile);
                        //ViewData["name"] = profile.name;
                        //ViewData["age"] = profile.age;
                        //ViewData["address"] = profile.address;
                        //ViewData["additionalInfo"] = profile.additionalInfo;
                        //ViewData["phone"] = profile.phone;
                        //ViewData["image"] = profile.image;
                    }
                    return View(profile);

                }
            }

            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> Index(ProfileDTO p)
        {
            var name = Request.Form["name"].ToString().Trim();
            //var age = int.Parse(Request.Form["age"]);
            var age = Request.Form["age"].ToString().Trim();
            var address = Request.Form["address"].ToString().Trim();
            var additionalInfo = Request.Form["additionalInfo"].ToString().Trim();
            var phone = Request.Form["phone"].ToString().Trim();
            var image = Request.Form["image"].ToString().Trim();
            if (ModelState.IsValid)
            {
                
                Dictionary<string,string> formData = new Dictionary<string,string>();
                formData["name"] = p.name;
                formData["address"] = p.address;
                formData["age"] = p.age.ToString();
                formData["additionalInfo"] = p.additionalInfo;
                formData["phone"] = p.phone;
                formData["image"] = p.image;
                HttpContent formContent = new FormUrlEncodedContent(formData);
                var accountId = HttpContext.Session.GetString("AccountId");
                var response = await _httpClient.PostAsync("http://localhost:5000/api/Student/update/"+accountId, formContent);
            }
            else
            {
                TempData["ErrorMessage"] = "Update Failed";
                return View(p);
            }
            TempData["SuccessMessage"] = "Update Success";
            return View(p);
        }
    }
}
