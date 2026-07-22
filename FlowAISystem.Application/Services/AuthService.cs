using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.DTOs.Auth;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Security;

namespace FlowAISystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;


    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }



    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request)
    {

        // 1. Find user
        var user = await _userRepository
            .GetByUsernameAsync(request.Username);



        if (user == null)
        {
            return null;
        }

        // 2. Check account status
        if (!user.IsActive)
        {
            return null;
        }



        // 2. Verify password
        bool passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);



        if (!passwordValid)
        {
            return null;
        }



        // 3. Get role safely
        var roleName =
            user.Role?.Name ?? "Student";



        // 4. Generate JWT
        var token =
            _jwtService.GenerateToken(
                user.Id,
                user.Username,
                roleName);



        // 5. Return login response
        return new LoginResponseDto
        {
            Username = user.Username,

            Role = roleName,

            Token = token
        };
    }




    public async Task<bool> RegisterAsync(
        RegisterRequestDto request)
    {

        // Check username
        if (await _userRepository
            .ExistsByUsernameAsync(request.Username))
        {
            return false;
        }



        // Check email
        if (await _userRepository
            .ExistsByEmailAsync(request.Email))
        {
            return false;
        }



        var user =
            new Domain.Entities.User
            {
                Username = request.Username,

                Email = request.Email,

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        request.Password),

                RoleId = request.RoleId,

                IsActive = true
            };



        await _userRepository.AddAsync(user);

        await _userRepository.SaveChangesAsync();


        return true;
    }
}

