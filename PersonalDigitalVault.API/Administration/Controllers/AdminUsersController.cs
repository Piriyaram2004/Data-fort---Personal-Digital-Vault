using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.Administration.DTOs;
using PersonalDigitalVault.API.Administration.Services;
using PersonalDigitalVault.API.DTOs.Administration;
using System.Security.Claims;

namespace PersonalDigitalVault.API.Administration.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Administrator")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUsersController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdminUserDto>>> GetAllUsers()
        {
            var users = await _adminUserService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(
            int id,
            [FromBody] UpdateAdminUserStatusRequest request)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid user id."
                });
            }

            var adminUserIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(adminUserIdValue, out var adminUserId))
            {
                return Unauthorized();
            }

            var ipAddress =
                HttpContext.Connection.RemoteIpAddress?.ToString();

            var updated = await _adminUserService.UpdateUserStatusAsync(
                id,
                request.IsActive,
                adminUserId,
                ipAddress);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = request.IsActive
                    ? "User account activated successfully."
                    : "User account deactivated successfully."
            });
        }
    }
}