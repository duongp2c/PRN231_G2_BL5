using Microsoft.AspNetCore.Mvc;
using PRN231_API.DAO;
using PRN231_API.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageTeacherController : ControllerBase
    {
        private readonly ManageTeacherDAO _teacherDAO;

        public ManageTeacherController(ManageTeacherDAO teacherDAO)
        {
            _teacherDAO = teacherDAO;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var teacher = await _teacherDAO.GetTeacherByIdAsync(id);
            if (teacher == null)
                return NotFound(); // Trả về 404 nếu không tìm thấy

            return Ok(teacher);
        }


        // Get all teachers
        [HttpGet("GetAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _teacherDAO.GetAllTeachersWithSubjectAsync();

            if (teachers == null || !teachers.Any())
            {
                return NotFound("No teachers found.");
            }

            return Ok(teachers);
        }

        [HttpPost("CreateTeacher")]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDTO createTeacherDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teacher = await _teacherDAO.CreateTeacherAsync(createTeacherDTO);
            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
        }
        [HttpPut("{teacherId}/status")]
        public async Task<IActionResult> EditIsActiveTeacher(int teacherId, [FromQuery] bool isActive)
        {
            var result = await _teacherDAO.EditActiveTeacherAsync(teacherId, isActive);
            if (result)
                return Ok("Teacher status updated successfully.");
            else
                return NotFound("Teacher not found.");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var result = await _teacherDAO.DeleteTeacherAsync(id);
            if (result)
            {
                return Ok("Teacher deleted successfully.");
            }
            else
            {
                return NotFound( "Teacher Not Found" );
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchTeachersByNameAsync([FromQuery] string name)
        {
            // If name is null or empty, return all teachers
            if (string.IsNullOrWhiteSpace(name))
            {
                var teachers = await _teacherDAO.GetAllTeachersWithSubjectAsync();
                return Ok(teachers);
            }

            // Perform search if name is provided
            var searchedTeachers = await _teacherDAO.SearchTeachersByNameAsync(name);
            return Ok(searchedTeachers);
        }


    }
}
