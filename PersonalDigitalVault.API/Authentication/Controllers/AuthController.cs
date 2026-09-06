using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.Authentication.DTOs;
using PersonalDigitalVault.API.Authentication.Services;
using PersonalDigitalVault.API.Authentication.Validators;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PersonalDigitalVault.API.Authentication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly RegisterRequestValidator _registerValidator;
        private readonly LoginRequestValidator _loginValidator;
        private readonly ForgotPasswordRequestValidator _forgotPasswordValidator;
        private readonly ResetPasswordRequestValidator _resetPasswordValidator;
        private readonly ChangePasswordRequestValidator _changePasswordValidator;

        public AuthController(
            IAuthService authService,
            RegisterRequestValidator registerValidator,
            LoginRequestValidator loginValidator,
            ForgotPasswordRequestValidator forgotPasswordValidator,
            ResetPasswordRequestValidator resetPasswordValidator,
            ChangePasswordRequestValidator changePasswordValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _forgotPasswordValidator = forgotPasswordValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _changePasswordValidator = changePasswordValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var errors = _registerValidator.Validate(request);

            if (errors.Count > 0)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }

            try
            {
                var result = await _authService.RegisterAsync(request);

                return StatusCode(201, result);
            }
            catch (InvalidOperationException ex)
                when (ex.Message == "Email is already registered." ||
                      ex.Message == "User name is already taken.")
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException)
            {
                return StatusCode(500, new
                {
                    message = "Registration could not be completed."
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var errors = _loginValidator.Validate(request);

            if (errors.Count > 0)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }

            try
            {
                var result = await _authService.LoginAsync(request);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }
            catch (InvalidOperationException)
            {
                return StatusCode(500, new
                {
                    message = "Login could not be completed."
                });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordRequestDto request)
        {
            var errors = _forgotPasswordValidator.Validate(request);

            if (errors.Count > 0)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }

            try
            {
                await _authService.ForgotPasswordAsync(request);

                return Ok(new
                {
                    message = "If the email is registered, a password reset link will be sent."
                });
            }
            catch (InvalidOperationException)
            {
                return StatusCode(500, new
                {
                    message = "Password reset request could not be completed."
                });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordRequestDto request)
        {
            var errors = _resetPasswordValidator.Validate(request);

            if (errors.Count > 0)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }

            try
            {
                await _authService.ResetPasswordAsync(request);

                return Ok(new
                {
                    message = "Password has been reset successfully."
                });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new
                {
                    message = "Invalid or expired password reset request."
                });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordRequestDto request)
        {
            var errors = _changePasswordValidator.Validate(request);

            if (errors.Count > 0)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid authentication token."
                });
            }

            try
            {
                await _authService.ChangePasswordAsync(
                    userId,
                    request);

                return Ok(new
                {
                    message = "Password changed successfully."
                });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new
                {
                    message = "Current password is incorrect."
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    message = "User account is not available."
                });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid authentication token."
                });
            }

            try
            {
                var profile = await _authService.GetProfileAsync(userId);

                return Ok(profile);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    message = "User account is not available."
                });
            }
        }
    }
}