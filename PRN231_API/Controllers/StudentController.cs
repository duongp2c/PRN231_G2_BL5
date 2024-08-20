using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_API.Models;
using PRN231_API.Services;

namespace PRN231_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/student/{studentId}
        [HttpGet("{studentId}")]
        public async Task<ActionResult<Student>> GetStudent(int studentId)
        {
            var student = await _studentService.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // GET: api/student/{studentId}/evaluations
        [HttpGet("{studentId}/evaluations")]
        public async Task<ActionResult<IEnumerable<Evaluation>>> GetStudentEvaluations(int studentId)
        {
            var evaluations = await _studentService.GetEvaluationsByStudentIdAsync(studentId);
            return Ok(evaluations);
        }

        // PUT: api/student/evaluation
        [HttpPut("evaluation")]
        public async Task<IActionResult> UpdateEvaluation(Evaluation evaluation)
        {
            // Additional validation could be done here if needed
            await _studentService.UpdateEvaluationAsync(evaluation);
            return NoContent();
        }
    }
}
