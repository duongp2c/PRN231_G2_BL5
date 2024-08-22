using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles ="Student")]
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

        [Authorize(Roles ="Student")]
        [Authorize("Student")]
        [HttpPost("register/{accountId}/{subjectId}")]
        public async Task<IActionResult> RegisterSubject(int subjectId, int accountId)
        {
            // Gọi phương thức RegisterSubjectAsync và truyền HttpContext
            var result = await _studentDao.RegisterSubjectAsync(subjectId, accountId);

            // Kiểm tra kết quả và trả về phản hồi phù hợp
            if (result == "Subject registered successfully.")
                return Ok(result);
            else
                return BadRequest(result);
        }


    }
}
