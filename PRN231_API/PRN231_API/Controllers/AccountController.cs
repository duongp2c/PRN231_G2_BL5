using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DAO;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private readonly AccountDAO _accountDao;

        public AccountController(AccountDAO accountDao)
        {
            _accountDao = accountDao;
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ResetPasswordRequestDTO request)
        {
            var result = await _accountDao.RequestPasswordResetAsync(request.Email);
            if (!result) return NotFound("Email not found.");

            return Ok("Password reset link sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            var result = await _accountDao.ResetPasswordAsync(request.Token, request.NewPassword, request.ConfirmPassword);
            if (!result) return BadRequest("Invalid token or passwords do not match.");

            return Ok("Password reset successful.");
        }


    }
}
