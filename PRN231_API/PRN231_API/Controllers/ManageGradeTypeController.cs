/*using Microsoft.AspNetCore.Mvc;
using PRN231_API.DAO;
using PRN231_API.DTO;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageGradeTypeController : ControllerBase
    {
        private readonly ManageGradeTypeDAO _manageGradeTypeDAO;

        public ManageGradeTypeController(ManageGradeTypeDAO manageGradeTypeDAO)
        {
            _manageGradeTypeDAO = manageGradeTypeDAO;
        }

       

        [HttpGet("{gradeTypeId}")]
        public async Task<IActionResult> GetGradeTypeById(int gradeTypeId)
        {
            var gradeType = await _manageGradeTypeDAO.GetGradeTypeByIdAsync(gradeTypeId);
            if (gradeType == null)
                return NotFound("GradeType not found.");

            return Ok(gradeType);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGradeType([FromBody] CreateGradeTypeDTO createGradeTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var gradeTypeDTO = await _manageGradeTypeDAO.CreateGradeTypeAsync(createGradeTypeDTO);
                return CreatedAtAction(nameof(GetGradeTypeById), new { gradeTypeId = gradeTypeDTO.GradeTypeId }, gradeTypeDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("AssociateGradeTypeWithSubject")]
        public async Task<IActionResult> AssociateGradeTypeWithSubject([FromBody] GradeTypeDTO gradeTypeSubjectDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _manageGradeTypeDAO.AssociateGradeTypeWithSubjectAsync(gradeTypeSubjectDTO);
                if (result == "GradeType successfully associated with Subject.")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }


}
*/