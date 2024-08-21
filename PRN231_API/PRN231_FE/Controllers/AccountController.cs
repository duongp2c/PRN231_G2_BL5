using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PRN231_FE.Models;
using System.IdentityModel.Tokens.Jwt;

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
                var response = await _httpClient.PostAsync("http://localhost:5231/api/Account/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Activate", "Account");
                }

                ModelState.AddModelError(string.Empty, "Registration failed.");
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
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5231/api/Account/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<TokenModel>(jsonResponse);
                    string token = loginResponse.AccessToken;

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                    var accountId = jsonToken.Claims.First(claim => claim.Type == "UserID").Value;

                    // Save the AccountId to the session
                    HttpContext.Session.SetString("AccountId", accountId);
                    // Store the token in a cookie or session
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Login failed.");
            }

            return View(model);
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
                var response = await _httpClient.PostAsync("http://localhost:5231/api/Account/activate", content);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally, redirect to a confirmation page or login
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, "Activation failed.");
            }

            return View(model);
        }
    }

}
