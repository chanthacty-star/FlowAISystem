namespace FlowAISystem.Shared.DTOs.Students;

public class StudentListItemDto
{
    public int Id { get; set; }

    public string StudentNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public DateOnly EnrollmentDate { get; set; }
}