﻿using Microsoft.AspNetCore.Mvc;
using PRN231_API.DAO;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentDAO _studentDao;

        public StudentController(StudentDAO studentDao)
        {
            _studentDao = studentDao;
        }
        [HttpGet("profile/{id}")]
        public async Task<ActionResult<ProfileDTO>> GetProfile(int id)
        {
            var profile = await _studentDao.GetStudentDetailAsync(id);
            return Ok(profile);
        }
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileDTO profile ,int id)
        {
            var result = await _studentDao.UpdateStudentDetailAsync(profile,id);
            if (result == "Update success")
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}
