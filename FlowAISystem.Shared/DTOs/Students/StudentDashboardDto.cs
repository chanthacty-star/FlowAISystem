namespace FlowAISystem.Shared.DTOs.Students;

public class StudentDashboardDto
{
    public string StudentNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public DateOnly EnrollmentDate { get; set; }

    public string? ProfileImage { get; set; }

    public string Username { get; set; } = string.Empty;

    public bool AccountActive { get; set; }
}