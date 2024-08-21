using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_API.DAO;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : Controller
    {
        private readonly SubjectDAO _subjectDAO;

        public SubjectController(SubjectDAO subjectDAO)
        {
            _subjectDAO = subjectDAO;
        }
        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            var subjects = _subjectDAO.GetAllSubjects().AsQueryable();
            return Ok(subjects);
        }
        [Authorize("Student")]
        [HttpGet("student/{accountId}")]
        public async Task<IActionResult> GetSubjectsByStudentID(int accountId)
        {
            // Gọi phương thức bất đồng bộ để lấy danh sách môn học
            var subjects = await _subjectDAO.GetSubjectsByStudentIDAsync(accountId);

            // Kiểm tra xem danh sách môn học có rỗng hoặc không
            if (subjects == null || !subjects.Any())
            {
                return NotFound(new { message = "No subjects found for this student." });
            }

            // Trả về kết quả với mã trạng thái 200 OK
            return Ok(subjects);
        }
        [EnableQuery]
        [HttpGet("subjecttwofields")]
        public IActionResult GetSubject2Fields()
        {
            var subjects = _subjectDAO.GetAllSubjects2Field().AsQueryable();
            return Ok(subjects);
        }
        [HttpGet("GetSubjectIdAndName")]
        public IActionResult GetSubjectIdAndName()
        {
            var subject = _subjectDAO.GetAllSubjectsIdAndName();
            return Ok(subject);
        }
    }
}
