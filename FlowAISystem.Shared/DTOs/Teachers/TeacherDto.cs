using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Teachers;

public class TeacherDto
{
    public int Id { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName =>
        $"{FirstName} {LastName}";

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public DateOnly HireDate { get; set; }

    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;
}