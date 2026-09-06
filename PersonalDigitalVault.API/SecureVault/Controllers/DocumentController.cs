using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.SecureVault.DTOs;
using PersonalDigitalVault.API.SecureVault.Services;

namespace PersonalDigitalVault.API.SecureVault.Controllers
{
    [ApiController]
    [Route("api/documents")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user identity.");
            }

            var documents =
                await _documentService.GetDocumentsAsync(userId);

            return Ok(documents);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument(
            CreateDocumentRequest request)
        {
            try
            {
                var userIdClaim =
                    User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Invalid user identity.");
                }

                var document =
                    await _documentService.CreateDocumentAsync(
                        request,
                        userId);

                return StatusCode(
                    StatusCodes.Status201Created,
                    document);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(
    int id,
    UpdateDocumentRequest request)
        {
            try
            {
                var userIdClaim =
                    User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Invalid user identity.");
                }

                var document =
                    await _documentService.UpdateDocumentAsync(
                        id,
                        request,
                        userId);

                return Ok(document);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                var userIdClaim =
                    User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Invalid user identity.");
                }

                await _documentService.DeleteDocumentAsync(
                    id,
                    userId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument(
    [FromForm] DocumentUploadRequest request)
        {
            try
            {
                var userIdClaim =
                    User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Invalid user identity.");
                }

                var document =
                    await _documentService.UploadDocumentAsync(
                        request,
                        userId);

                return StatusCode(
                    StatusCodes.Status201Created,
                    document);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}