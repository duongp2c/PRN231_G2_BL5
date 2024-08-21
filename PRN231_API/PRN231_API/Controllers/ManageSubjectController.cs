using Microsoft.AspNetCore.Mvc;
using PRN231_API.DAO;
using PRN231_API.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageSubjectController : ControllerBase
    {
        private readonly ManageSubjectDAO _subjectDAO;

        public ManageSubjectController(ManageSubjectDAO subjectDAO)
        {
            _subjectDAO = subjectDAO;
        }

        [HttpGet("GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            try
            {
                var subjects = await _subjectDAO.GetAllSubjectsAsync();

                if (subjects == null || !subjects.Any())
                {
                    return NotFound("No subjects found.");
                }

                return Ok(subjects);
            }
            catch (Exception ex)
            {
                // Log the exception (you might use a logging framework here)
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{subjectId}")]
        public async Task<IActionResult> GetSubjectById(int subjectId)
        {
            var subject = await _subjectDAO.GetSubjectByIdAsync(subjectId);
            if (subject == null)
                return NotFound("Subject not found.");

            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDTO createSubjectDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var subjectDTO = await _subjectDAO.CreateSubjectAsync(createSubjectDTO);
                return CreatedAtAction(nameof(GetSubjectById), new { subjectId = subjectDTO.SubjectId }, subjectDTO);
            }
            catch (Exception ex)
            {
                // Log the exception (you might use a logging framework here)
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPut("{subjectId}")]
        public async Task<IActionResult> UpdateSubject(int subjectId, [FromForm] CreateSubjectDTO updateSubjectDTO)
        {
            var updatedSubject = await _subjectDAO.UpdateSubjectAsync(subjectId, updateSubjectDTO);
            if (updatedSubject == null)
                return NotFound("Subject not found.");

            return Ok(updatedSubject);
        }

        [HttpDelete("{subjectId}")]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            var result = await _subjectDAO.DeleteSubjectAsync(subjectId);
            if (result)
                return Ok("Subject deleted successfully.");
            else
                return NotFound("Subject not found.");
        }
    }
}
