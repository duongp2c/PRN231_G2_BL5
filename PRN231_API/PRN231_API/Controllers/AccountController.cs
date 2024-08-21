
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using PRN231_API.DAO;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;
using PRN231_API.ViewModel;

namespace PRN231_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private readonly AccountDAO _accountDao;
        private readonly SchoolDBContext _context;
        private readonly IAccountService _accountService;

        public AccountController(AccountDAO accountDao, SchoolDBContext context, IAccountService accountService)
        {
            _accountDao = accountDao;
            _context = context;
            _accountService = accountService;
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Account>> StudentDetails()
        {
            var student = await _context.Accounts.ToListAsync();
            return Ok(student);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountRegisterDto userDto)
        {
            return Ok(_accountService.RegisterAccount(userDto));
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenModel>> Login(AccountLoginDto userDto)
        {
            return await _accountService.LoginAccount(userDto);
        }

        [HttpPost("activate")]
        public IActionResult Activate(ActiveViewModel activeModel)
        {

            return Ok(_accountService.ActiveAccount(activeModel));
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken(TokenModel activeModel)
        {
            try
            {
                var response = await _accountService.Refresh(activeModel);
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
