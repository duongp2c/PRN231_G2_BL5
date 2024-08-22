using Microsoft.AspNetCore.Authorization;
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
                throw new Exception(ex.Message);
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

        [Authorize("Admin")]
        [HttpPost("CreateSubject")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDTO createSubjectDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var subjectDTO = await _subjectDAO.CreateSubjectAsync(createSubjectDTO);
                return Ok(subjectDTO);
            }
            catch
            {
                return BadRequest();
            }
        }


        [Authorize("Admin")]
        [HttpPut("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject(int subjectId, [FromForm] CreateSubjectDTO updateSubjectDTO)
        {

            try
            {
                var updatedSubject = await _subjectDAO.UpdateSubjectAsync(subjectId, updateSubjectDTO);
                return Ok(updatedSubject);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize("Admin")]
        [HttpDelete("{subjectId}")]
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            try
            {
                var result = await _subjectDAO.DeleteSubjectAsync(subjectId);
                return Ok("Subject deleted successfully.");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
