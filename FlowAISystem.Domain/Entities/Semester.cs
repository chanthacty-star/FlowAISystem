using FlowAISystem.Domain.Common;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Domain.Entities;

public class Semester : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public SemesterType SemesterType { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsCurrent { get; set; }

    public int AcademicYearId { get; set; }

    // Navigation Property
    public AcademicYear? AcademicYear { get; set; }

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public ICollection<CourseOffering> CourseOfferings { get; set; }
    = new List<CourseOffering>();
}