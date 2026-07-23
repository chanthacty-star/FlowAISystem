namespace FlowAISystem.Shared.DTOs.Students;

public class StudentSubjectSummaryDto
{
    public int SubjectId { get; set; }

    public string SubjectCode { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public int Credits { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public string SemesterName { get; set; } = string.Empty;
}