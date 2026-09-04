namespace PersonalDigitalVault.API.DTOs.Administration
{
    public class AdminUserDto
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string RoleName { get; set; } = string.Empty;
    }
}