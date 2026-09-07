using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.SecureVault.Services;

namespace PersonalDigitalVault.API.SecureVault.Controllers
{
    [ApiController]
    [Route("api/credentials")]
    [Authorize]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialService _credentialService;

        public CredentialController(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCredentials()
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user identity.");
            }

            var credentials =
                await _credentialService.GetCredentialsAsync(userId);

            return Ok(credentials);
        }
    }
}