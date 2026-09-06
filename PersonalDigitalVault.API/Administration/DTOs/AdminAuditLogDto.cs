namespace PersonalDigitalVault.API.Administration.DTOs
{
    public class AdminAuditLogDto
    {
        public int AuditLogId { get; set; }

        public int? UserId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string EntityType { get; set; } = string.Empty;

        public int? EntityId { get; set; }

        public string? Details { get; set; }

        public string? IPAddress { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}