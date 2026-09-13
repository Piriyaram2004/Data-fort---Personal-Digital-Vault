namespace PersonalDigitalVault.API.Authentication.DTOs
{
    public class ProfileResponseDto
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public string Role { get; set; } = string.Empty;
    }
}