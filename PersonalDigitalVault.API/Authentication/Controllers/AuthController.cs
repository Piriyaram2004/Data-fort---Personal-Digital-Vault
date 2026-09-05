using Microsoft.AspNetCore.Mvc;
using PersonalDigitalVault.API.Authentication.DTOs;
using PersonalDigitalVault.API.Authentication.Services;
using PersonalDigitalVault.API.Authentication.Validators;

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

        public AuthController(
            IAuthService authService,
            RegisterRequestValidator registerValidator,
            LoginRequestValidator loginValidator,
            ForgotPasswordRequestValidator forgotPasswordValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _forgotPasswordValidator = forgotPasswordValidator;
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
    }
}