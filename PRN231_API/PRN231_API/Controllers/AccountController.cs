using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DAO;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace PRN231_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SchoolDBContext _context;
        private readonly IAccountService _accountService;



        public AccountController(SchoolDBContext context,IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
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
