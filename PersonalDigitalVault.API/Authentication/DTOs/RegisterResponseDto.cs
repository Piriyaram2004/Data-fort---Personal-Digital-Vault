namespace PersonalDigitalVault.API.Authentication.DTOs
{
    public class RegisterResponseDto
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}