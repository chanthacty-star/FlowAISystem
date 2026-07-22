using FlowAISystem.Shared.Enums;
namespace FlowAISystem.Shared.DTOs.Semesters;

public class SemesterSearchDto
{
    public string? SearchTerm { get; set; }

    public int? AcademicYearId { get; set; }

    public bool? IsCurrent { get; set; }
}