using FlowAISystem.Application.DTOs.Auth;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request);

    Task<bool> RegisterAsync(
        RegisterRequestDto request);
}
