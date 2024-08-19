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
        [HttpGet("student/{studentId}")]
        public IActionResult GetSubjectsByStudentID(int studentId)
        {
            var subjects = _subjectDAO.GetSubjectsByStudentID(studentId);
            if (subjects == null || !subjects.Any())
            {
                return NotFound(new { message = "No subjects found for this student." });
            }
            return Ok(subjects);
        }
        [EnableQuery]
        [HttpGet("subjecttwofields")]
        public IActionResult GetSubject2Fields()
        {
            var subjects = _subjectDAO.GetAllSubjects2Field().AsQueryable();
            return Ok(subjects);
        }
    }
}
