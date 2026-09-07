using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.SecureVault.DTOs;
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
                return Unauthorized("Invalid user identity.");

            var credentials =
                await _credentialService.GetCredentialsAsync(userId);

            return Ok(credentials);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCredential(
            [FromBody] CreateCredentialRequest request)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user identity.");

            try
            {
                var credential =
                    await _credentialService.CreateCredentialAsync(
                        userId,
                        request);

                return CreatedAtAction(
                    nameof(GetCredentials),
                    credential);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCredential(
    int id,
    [FromBody] UpdateCredentialRequest request)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user identity.");

            try
            {
                var credential =
                    await _credentialService.UpdateCredentialAsync(
                        id,
                        userId,
                        request);

                if (credential == null)
                    return NotFound("Credential not found.");

                return Ok(credential);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredential(int id)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user identity.");

            var deleted = await _credentialService.DeleteCredentialAsync(
                id,
                userId);

            if (!deleted)
                return NotFound("Credential not found.");

            return NoContent();
        }
    }
}