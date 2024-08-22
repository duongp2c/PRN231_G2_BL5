namespace PRN231_API.DTO
{
    // DTO for Reset Password Request
    public class ResetPasswordRequestDTO
    {
        public string Email { get; set; }
    }

    // DTO for Password Reset Input
    public class ResetPasswordDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
