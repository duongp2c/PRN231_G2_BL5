﻿using Microsoft.AspNetCore.Mvc;

namespace PRN231_FE.Controllers
{
    public class ManageTeacherController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ManageTeacherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            return (View("Index"));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTeacherStatus(int id, bool isActive)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"http://localhost:5231/api/ManageTeacher/{id}/status?isActive={isActive}";

            var response = await httpClient.PutAsync(url, null); // No content body is needed for this request

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect back to the list of students after a successful update
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the student status.");
                return View("Error"); // Replace with an appropriate error handling view
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateTeacher()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateTeacher(CreateTeacherDTO createTeacher)
        //{

        //    return View();
        //}
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"http://localhost:5231/api/ManageTeacher/{id}";

            // Sending the DELETE request
            var response = await httpClient.DeleteAsync(url);

            // Check if the deletion was successful
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Student successfully deleted.";
                return RedirectToAction("Index"); // Redirect back to the list of students after successful deletion
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the student.";
                return RedirectToAction("Index"); // Redirect back to the list of students with an error message
            }
        }
    }
}
