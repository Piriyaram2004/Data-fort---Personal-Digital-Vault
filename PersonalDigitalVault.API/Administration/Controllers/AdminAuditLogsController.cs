using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.Administration.Services;

namespace PersonalDigitalVault.API.Administration.Controllers
{
    [ApiController]
    [Route("api/admin/audit-logs")]
    [Authorize(Roles = "Administrator")]
    public class AdminAuditLogsController : ControllerBase
    {
        private readonly IAdminAuditLogService _adminAuditLogService;

        public AdminAuditLogsController(
            IAdminAuditLogService adminAuditLogService)
        {
            _adminAuditLogService = adminAuditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var auditLogs = await _adminAuditLogService.GetAllAsync();

            return Ok(auditLogs);
        }
    }
}