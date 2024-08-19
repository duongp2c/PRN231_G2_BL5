﻿using Microsoft.AspNetCore.Mvc;
using PRN231_API.DAO;

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
        [HttpPost("{studentId}/register/{subjectId}")]
        public async Task<IActionResult> RegisterSubject(int studentId, int subjectId)
        {
            var result = await _studentDao.RegisterSubjectAsync(studentId, subjectId);
            if (result == "Subject registered successfully.")
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}
