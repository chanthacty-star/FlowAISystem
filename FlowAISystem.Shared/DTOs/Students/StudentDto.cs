using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Students;

public class StudentDto
{
    public int Id { get; set; }

    public string StudentNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    public int? UserId { get; set; }

    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;
}