using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.PublicSharing.DTOs;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;
using PersonalDigitalVault.API.PublicSharing.Validators;

namespace PersonalDigitalVault.API.PublicSharing.Controllers
{
    [ApiController]
    [Route("api/share-links")]
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

            // Temporary user ID for API testing.
            // JWT integration will replace this later.
            int userId = 1;

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
    }
}