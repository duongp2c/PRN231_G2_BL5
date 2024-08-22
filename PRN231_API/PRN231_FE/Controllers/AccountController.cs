using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PRN231_FE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace PRN231_FE.Controllers
{


    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5000/api/Account/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Activate", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email already registered.");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jsonContent = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("http://localhost:5000/api/Account/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var loginResponse = JsonConvert.DeserializeObject<TokenModel>(jsonResponse);
                        string token = loginResponse.AccessToken;

                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                        var accountId = jsonToken.Claims.First(claim => claim.Type == "UserID").Value;
                        var role = jsonToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;


                        HttpContext.Session.SetString("AuthToken", token);
                        HttpContext.Session.SetString("Role", role);
                        HttpContext.Session.SetString("AccountId", accountId);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                        ModelState.AddModelError(string.Empty, (string)errorResponse.message);
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "Login failed.");
                }

                return View(model);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public IActionResult Activate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Activate(ActivateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5000/api/Account/activate", content);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally, redirect to a confirmation page or login
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, "Activation failed.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Clear cache
            Response.Headers["Cache-Control"] = "no-cache, no-store";
            Response.Headers["Expires"] = DateTime.UtcNow.ToString("R");

            // Clear cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Login", "Account");
        }
    }

}
