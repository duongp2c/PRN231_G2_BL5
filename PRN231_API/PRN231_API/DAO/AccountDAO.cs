using MailKit.Net.Smtp;
using MimeKit;
using PRN231_API.Models;
using PRN231_API.Repository;


namespace PRN231_API.DAO
{
    public class AccountDAO
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AccountDAO(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
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
    }
}

