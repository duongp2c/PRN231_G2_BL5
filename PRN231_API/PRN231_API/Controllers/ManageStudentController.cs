using Microsoft.AspNetCore.Mvc;
using PRN231_API.DAO;
using PRN231_API.DTO;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageStudentController : Controller
    {
        private readonly StudentDAO _studentDao;

        public ManageStudentController(StudentDAO studentDao)
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
        [HttpGet("GetAllStudent")]

        public async Task<IActionResult> GetAllStudent()
        {
            var students = await _studentDao.GetAllStudentsWithDetailsAndSubjectsAsync();

            if (students == null || !students.Any())
            {
                return NotFound("No students found.");
            }

            return Ok(students);
        }
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var result = await _studentDao.DeleteStudentAsync(studentId);
            if (result)
                return Ok("Student deleted successfully.");
            else
                return NotFound("Student not found.");
        }
        [HttpPut("{studentId}/status")]
        public async Task<IActionResult> EditIsActiveStudent(int studentId, [FromQuery] bool isActive)
        {
            var result = await _studentDao.EditActiveStudentAsync(studentId, isActive);
            if (result)
                return Ok("Student status updated successfully.");
            else
                return NotFound("Student not found.");
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchStudentsByNameAsync([FromQuery] string name)
        {
            // If name is null or empty, return all students
            if (string.IsNullOrWhiteSpace(name))
            {
                var students = await _studentDao.GetAllStudentsWithDetailsAndSubjectsAsync();
                return Ok(students);
            }

            // Perform search if name is provided
            var searchedStudents = await _studentDao.SearchStudentsByNameAsync(name);
            return Ok(searchedStudents);
        }


    }
}
