namespace FlowAISystem.Shared.DTOs.Users;

public class ResetPasswordDto
{
    public int UserId { get; set; }

    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}