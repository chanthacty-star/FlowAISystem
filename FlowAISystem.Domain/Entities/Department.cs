using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Navigation Properties
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public ICollection<Student> Students { get; set; } = new List<Student>();

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}