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
        [Authorize("Student")]
        [HttpPost("register/{subjectId}")]
        public async Task<IActionResult> RegisterSubject(int subjectId)
        {
            // Gọi phương thức RegisterSubjectAsync và truyền HttpContext
            var result = await _studentDao.RegisterSubjectAsync(subjectId, HttpContext);

            // Kiểm tra kết quả và trả về phản hồi phù hợp
            if (result == "Subject registered successfully.")
                return Ok(result);
            else
                return BadRequest(result);
        }


    }
}
