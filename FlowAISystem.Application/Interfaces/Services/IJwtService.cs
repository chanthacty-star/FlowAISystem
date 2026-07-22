namespace FlowAISystem.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(
        int userId,
        string username,
        string role);
}
