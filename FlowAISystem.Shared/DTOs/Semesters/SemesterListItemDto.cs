using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Semesters;

public class SemesterListItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public SemesterType SemesterType { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsCurrent { get; set; }

    public string AcademicYearName { get; set; } = string.Empty;

    public int SubjectCount { get; set; }
}