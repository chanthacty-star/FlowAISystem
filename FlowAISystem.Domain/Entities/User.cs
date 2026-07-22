using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;


    // Foreign Key
    public int RoleId { get; set; }


    // Navigation Property
    public Role? Role { get; set; }

    public ICollection<Announcement> Announcements { get; set; }
         = new List<Announcement>();
}