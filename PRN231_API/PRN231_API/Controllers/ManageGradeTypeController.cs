using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        [Authorize("Teacher, Admin")]
        [HttpGet("{gradeTypeId}")]
        public async Task<IActionResult> GetGradeTypeById(int gradeTypeId)
        {
            var gradeType = await _manageGradeTypeDAO.GetGradeTypeByIdAsync(gradeTypeId);
            if (gradeType == null)
                return NotFound("GradeType not found.");

            return Ok(gradeType);
        }


        [Authorize("Teacher, Admin")]
        [HttpGet("GetAllGradeTypes")]
        public async Task<IActionResult> GetAllGradeTypes()
        {
            try
            {
                var grades = await _manageGradeTypeDAO.GetAllGradeTypesAsync();

                if (grades == null || !grades.Any())
                {
                    return NotFound("No grade type found.");
                }

                return Ok(grades);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [Authorize("Admin")]
        [HttpPost("CreateGradeType")]
        public async Task<IActionResult> CreateGradeType([FromBody] CreateGradeTypeDTO createGradeTypeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var gradetypeDTO = await _manageGradeTypeDAO.CreateGradeTypeAsync(createGradeTypeDTO);
                return Ok(gradetypeDTO);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize("Admin")]
        [HttpDelete("{gradetypeId}")]
        public async Task<IActionResult> DeleteGradeType(int gradetypeId)
        {
            try
            {
                var result = await _manageGradeTypeDAO.DeleteGradeTypeAsync(gradetypeId);
                return Ok("Grade Type deleted successfully.");
            }
            catch
            {
                return BadRequest();
            }
        }
    }


}
