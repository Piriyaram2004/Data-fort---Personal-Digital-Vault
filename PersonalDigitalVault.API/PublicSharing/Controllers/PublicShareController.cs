using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;

namespace PersonalDigitalVault.API.PublicSharing.Controllers
{
    [ApiController]
    [Route("api/public/share")]
    public class PublicShareController : ControllerBase
    {
        private readonly IShareAccessService _shareAccessService;

        public PublicShareController(
            IShareAccessService shareAccessService)
        {
            _shareAccessService = shareAccessService;
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> GetPublicShare(string token)
        {
            var result = await _shareAccessService
                .GetPublicShareAsync(token);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Share link is invalid, expired, revoked, or unavailable."
                });
            }

            return Ok(result);
        }
    }
}