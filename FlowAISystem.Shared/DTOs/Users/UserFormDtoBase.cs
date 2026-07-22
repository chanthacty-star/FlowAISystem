using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.Users;

public abstract class UserFormDtoBase
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}