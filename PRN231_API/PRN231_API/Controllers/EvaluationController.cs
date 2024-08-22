using Microsoft.AspNetCore.Mvc;
using PRN231_API.DAO;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : Controller
    {
        private readonly EvaluationDAO _evaluationDao;
        private readonly StudentDAO _studentDao;

        public EvaluationController(EvaluationDAO evaluationDao , StudentDAO studentDao)
        {
            _evaluationDao = evaluationDao;
            _studentDao = studentDao;
        }
        [HttpGet("subject/{id}")]
        public async Task<ActionResult<List<SubjectDTO>>> GetStudentSubjectById(int id)
        {
            var subjects = await _studentDao.GetStudentSubjectAsync(id);
            return Ok(subjects);
        }
        [HttpGet("subject/{studentId}/{subjectId}")]
        public async Task<ActionResult<List<SubjectDTO>>> GetStudentGrade(int studentId,int subjectId)
        {
            var subjects = await _evaluationDao.GetStudentSubjectGradeAsync(studentId, subjectId);
            return Ok(subjects);
        }

    }
}
