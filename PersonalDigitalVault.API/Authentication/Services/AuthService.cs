using Microsoft.AspNetCore.Identity;
using PersonalDigitalVault.API.Authentication.DTOs;
using PersonalDigitalVault.API.Authentication.Helpers;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly PasswordResetTokenHelper _passwordResetTokenHelper;
        private readonly IEmailService _emailService;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher<User> passwordHasher,
            JwtTokenHelper jwtTokenHelper,
            IPasswordResetTokenRepository passwordResetTokenRepository,
            PasswordResetTokenHelper passwordResetTokenHelper,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenHelper = jwtTokenHelper;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _passwordResetTokenHelper = passwordResetTokenHelper;
            _emailService = emailService;
        }

        public async Task<RegisterResponseDto> RegisterAsync(
            RegisterRequestDto request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new InvalidOperationException(
                    "Email is already registered.");
            }

            if (await _userRepository.UserNameExistsAsync(request.UserName))
            {
                throw new InvalidOperationException(
                    "User name is already taken.");
            }

            var userRole = await _roleRepository.GetByNameAsync("User");

            if (userRole == null)
            {
                throw new InvalidOperationException(
                    "User role is not configured.");
            }

            var user = new User
            {
                Email = request.Email.Trim(),
                UserName = request.UserName.Trim(),
                FullName = request.FullName.Trim(),
                RoleId = userRole.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    request.Password);

            await _userRepository.AddAsync(user);

            return new RegisterResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                Role = userRole.RoleName
            };
        }

        public async Task<LoginResponseDto> LoginAsync(
            LoginRequestDto request)
        {
            var user = await _userRepository
                .GetByEmailAsync(request.Email.Trim());

            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var token = _jwtTokenHelper.GenerateToken(user);

            return new LoginResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                Role = user.Role.RoleName,
                Token = token
            };
        }

        public async Task ForgotPasswordAsync(
        ForgotPasswordRequestDto request)
        {
            var user = await _userRepository
                .GetByEmailAsync(request.Email.Trim());

            if (user == null || !user.IsActive)
            {
                return;
            }

            var rawToken =
                _passwordResetTokenHelper.GenerateToken();

            var tokenHash =
                _passwordResetTokenHelper.HashToken(rawToken);

            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _passwordResetTokenRepository
                .AddAsync(resetToken);

            var resetLink =
                $"http://localhost:4200/reset-password?token={Uri.EscapeDataString(rawToken)}&email={Uri.EscapeDataString(user.Email)}";

            await _emailService.SendPasswordResetEmailAsync(
                user.Email,
                resetLink);

            // Raw token is used only in the reset link.
            // Do not store, return, or log the raw token.
        }
    }
}