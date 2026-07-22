using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class AcademicYear : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsCurrent { get; set; }

    public ICollection<Semester> Semesters { get; set; } = new List<Semester>();
}