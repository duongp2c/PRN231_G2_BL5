using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repositories;

namespace PRN231_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly IEvaluationRepository evaluationRepository;

        public StudentController(IStudentRepository studentRepository, IEvaluationRepository evaluationRepository)
        {

            this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            this.evaluationRepository = evaluationRepository ?? throw new ArgumentNullException(nameof(evaluationRepository));
        }


        //// GET: api/student/{studentId}
        //[HttpGet("{studentId}")]
        //public async Task<ActionResult<Student>> GetStudent(int studentId)
        //{
        //    var student = await studentRepository.GetStudentByIdAsync(studentId);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(student);
        //}

        // GET: api/evaluation/{evaluationId}
        [HttpGet("{studentId}/evaluations/{evaluationId}")]
        public async Task<ActionResult<EvaluationDTO>> GetEvaluationByEIdAsync(int evaluationId)
        {
            var evaluation = await evaluationRepository.GetEvaluationByIdAsync(evaluationId);

            if (evaluation == null)
            {
                return NotFound();
            }

            var evaluationDTO = new EvaluationDTO
            {
                EvaluationId = evaluation.EvaluationId,
                StudentId = (int)evaluation.StudentId,
                StudentName = evaluation.Student?.Name ?? "Unknown",
                SubjectId = (int)evaluation.SubjectId,
                SubjectName = evaluation.Subject?.SubjectName ?? "Unknown",
                GradeTypeId = (int)evaluation.GradeTypeId,
                GradeTypeName = evaluation.GradeType?.GradeTypeName ?? "Unknown",
                Grade = evaluation.Grade,
                AdditionExplanation = evaluation.AdditionExplanation
            };

            return Ok(evaluationDTO);
        }

        // GET: api/student/{studentId}/evaluations
        [HttpGet("{studentId}/evaluations")]
        public async Task<ActionResult<IEnumerable<EvaluationDTO>>> GetEvaluationByIdAsync(int studentId)
        {
            var evaluations = await studentRepository.GetEvaluationsByStudentIdAsync(studentId);
            if (evaluations == null || !evaluations.Any())
            {
                return NotFound();
            }

            var evaluationDTOs = evaluations.Select(e => new EvaluationDTO
            {
                EvaluationId = e.EvaluationId,
                StudentId = (int)e.StudentId,
                StudentName = e.Student?.Name ?? "Unknown", // Handle null Student
                SubjectId = (int)e.SubjectId,
                SubjectName = e.Subject?.SubjectName ?? "Unknown", // Handle null Subject
                GradeTypeId = (int)e.GradeTypeId,
                GradeTypeName = e.GradeType?.GradeTypeName ?? "Unknown", // Handle null GradeType
                Grade = e.Grade,
                AdditionExplanation = e.AdditionExplanation
            }).ToList();

            return Ok(evaluationDTOs);
        }

        // PUT: api/student/evaluation
        [HttpPut("evaluation")]
        public async Task<IActionResult> UpdateEvaluation([FromBody] EvaluationDTO evaluationDTO)
        {
            if (evaluationDTO == null)
            {
                return BadRequest("Evaluation data is required.");
            }

            try
            {
                await evaluationRepository.UpdateEvaluationAsync(evaluationDTO);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Evaluation not found.");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }



        //[HttpPost("evaluation")]
        //public async Task<ActionResult<Evaluation>> CreateEvaluation([FromForm] Evaluation evaluation)
        //{
        //    if (evaluation == null)
        //    {
        //        return BadRequest("Evaluation data is required.");
        //    }

        //    var createdEvaluation = await evaluationRepository.AddEvaluationAsync(evaluation);
        //    return CreatedAtAction(nameof(GetEvaluationByIdAsync), new { studentId = createdEvaluation.StudentId }, createdEvaluation);
        //}

        // GET: api/student/subject/{subjectId}/students
        [HttpGet("subject/{subjectId}/students")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsBySubjectAsync(int subjectId)
        {
            // Fetch students for the given subjectId
            var students = await studentRepository.GetStudentsBySubjectAsync(subjectId);

            // Check if students list is empty or null
            if (students == null || !students.Any())
            {
                return NotFound("No students found for this subject.");
            }

            // Map to StudentDTO
            var studentDTOs = students.Select(s => new StudentDTO
            {
                StudentId = s.StudentId,
                AccountId = s.AccountId,  // Add these fields if needed
                Name = s.Name,
                Age = s.Age,
                IsRegularStudent = s.IsRegularStudent,
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                TeacherName = s.TeacherName
            }).ToList();

            return Ok(studentDTOs);
        }



    }
}
