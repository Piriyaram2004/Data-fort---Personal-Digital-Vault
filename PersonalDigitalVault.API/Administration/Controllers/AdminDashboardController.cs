using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.Administration.Services;

namespace PersonalDigitalVault.API.Administration.Controllers
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Administrator")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _adminDashboardService.GetDashboardAsync();

            return Ok(dashboard);
        }
    }

