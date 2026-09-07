using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PersonalDigitalVault.API.PublicSharing.DTOs;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;
using PersonalDigitalVault.API.PublicSharing.Validators;

namespace PersonalDigitalVault.API.PublicSharing.Controllers
{
    [ApiController]
    [Route("api/share-links")]
    [Authorize]
    public class ShareController : ControllerBase
    {
        private readonly IShareService _shareService;
        private readonly ShareLinkValidator _validator;

        public ShareController(
            IShareService shareService,
            ShareLinkValidator validator)
        {
            _shareService = shareService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShareLink(
            [FromBody] CreateShareLinkDto request)
        {
            var validationError = _validator.ValidateCreate(request);

            if (validationError != null)
            {
                return BadRequest(new
                {
                    message = validationError
                });
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user authentication."
                });
            }

            var result = await _shareService
                .CreateShareLinkAsync(userId, request);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Document not found or access denied."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                result);
        }
        [HttpGet]
        public async Task<IActionResult> GetShareLinks()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user authentication."
                });
            }

            var result = await _shareService
                .GetShareLinksAsync(userId);

            return Ok(result);
        }



        [HttpPost("{id}/revoke")]
        public async Task<IActionResult> RevokeShareLink(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user authentication."
                });
            }

            var result = await _shareService
                .RevokeShareLinkAsync(userId, id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Share link not found or access denied."
                });
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShareLink(
         int id,
         [FromBody] UpdateShareLinkDto request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user authentication."
                });
            }

            if (request.ExpiresAt.HasValue &&
                request.ExpiresAt.Value <= DateTime.UtcNow)
            {
                return BadRequest(new
                {
                    message = "Expiry date and time must be in the future."
                });
            }

            var result = await _shareService
                .UpdateShareLinkAsync(
                    userId,
                    id,
                    request.ExpiresAt);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Share link not found or access denied."
                });
            }

            return Ok(result);
        }

    }
}