using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.Administration.Services;
using PersonalDigitalVault.API.DTOs.Administration;

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
    }
}