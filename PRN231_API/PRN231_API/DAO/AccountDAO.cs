
using MailKit.Net.Smtp;
using MimeKit;
using PRN231_API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;
using PRN231_API.ViewModel;
using static PRN231_API.DAO.AccountDAO;
using Microsoft.AspNetCore.Http;


namespace PRN231_API.DAO
{
        public interface IAccountService 
    {
        Task<CustomResponse> RegisterAccount(AccountRegisterDto userDto);
        Task<TokenModel> LoginAccount(AccountLoginDto userDto, HttpContext httpContext);
        Task<TokenModel> Refresh(TokenModel tokenModel);
        Task<CustomResponse> ActiveAccount(ActiveViewModel activeModel);
    }
    public class AccountDAO : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly EmailDAO _emailDAO;

        public AccountDAO(IAccountRepository accountRepository, IConfiguration configuration, IMapper mapper, IJwtTokenService tokenService, EmailDAO emailDAO)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
            _mapper = mapper;
            _jwtTokenService = tokenService;
            _emailDAO = emailDAO;
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(email);
            if (account == null) return false;

            var token = Guid.NewGuid().ToString();  // Generate a unique token
            await _accountRepository.SaveResetTokenAsync(account.AccountId, token);

            var resetLink = $"http://localhost:5139/ForgotPassword/ResetPass?token={token}";
            await SendEmailAsync(email, "Reset Your Password", $"Click here to reset your password: {resetLink}");

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword) return false;

            var account = await _accountRepository.GetAccountByResetTokenAsync(token);
            if (account == null) return false;

            return await _accountRepository.UpdatePasswordAsync(account.AccountId, newPassword);
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("School", "quanthai111202@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    // Cài đặt SMTP từ file cấu hình
                    var smtpHost = _configuration["Smtp:Host"];
                    var smtpPort = int.Parse(_configuration["Smtp:Port"]);
                    var smtpUser = _configuration["Smtp:User"];
                    var smtpPass = _configuration["Smtp:Pass"];

                    await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);// Sử dụng true cho SSL
                    await client.AuthenticateAsync(smtpUser, smtpPass);
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ (ghi log)
                    Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        public class ActivateViewModel
        {
            public string Email { get; set; }
            public string ActivationCode { get; set; }
        }

        public async Task<TokenModel> LoginAccount(AccountLoginDto userDto, HttpContext httpContext)
        {
            try
            {
                var user = await _accountRepository.Login(userDto);
                if (user == null)
                    return null;

                // Tạo token và refresh token
                var token = _jwtTokenService.GenerateToken(user);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                await _jwtTokenService.SaveRefreshTokenToRedisAsync(refreshToken, user.Email);

                // Lưu AccountId vào session sau khi đăng nhập thành công
                httpContext.Session.SetInt32("AccountId", user.AccountId);

                return new TokenModel { AccessToken = token, RefreshToken = refreshToken };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<TokenModel> Refresh([FromBody] TokenModel model)
        {
            var refreshtoken = await _jwtTokenService.GetRefreshTokenFromRedisAsync(model.Email);
            if (refreshtoken == null)
            {
                return null;
            }

            var user = await _accountRepository.GetUserByUsernameAsync(model.Email);
            if (user == null)
            {
                return null;
            }

            var newAccessToken = _jwtTokenService.GenerateToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _jwtTokenService.SaveRefreshTokenToRedisAsync(newRefreshToken, user.Email);

            return new TokenModel { AccessToken = newAccessToken, RefreshToken = newRefreshToken };

        }

        public async Task<CustomResponse> RegisterAccount(AccountRegisterDto userDto)
        {
            try
            {
                var user = _mapper.Map<Account>(userDto);
                var response = await _accountRepository.Register(user);
                var subject = "Your Activation Code";
                var body = $"Your activation code is {user.ActiveCode}. Please use this code to activate your account.";
                _emailDAO.SendEmail(user.Email, subject, body);

                return new CustomResponse { Message = "Success", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CustomResponse> ActiveAccount(ActiveViewModel activeModel)
        {
            try
            {
                _accountRepository.ActiveAccount(activeModel);
                return new CustomResponse { Message = "Success", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}


