using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int studentId)
        {
            return Ok( await _studentDao.GetStudentDetailAsync(studentId));
        }
        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileDTO profile)
        {
            var result = await _studentDao.UpdateStudentDetailAsync(profile);
            if (result == "Update success")
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}
