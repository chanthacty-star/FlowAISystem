using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Enrollments;

public class EnrollmentListItemDto
{
    public int Id { get; set; }


    public string StudentName { get; set; }
        = string.Empty;


    public string CourseName { get; set; }
        = string.Empty;


    public string ClassName { get; set; }
        = string.Empty;


    public string TeacherName { get; set; }
        = string.Empty;


    public string SemesterName { get; set; }
        = string.Empty;


    public EnrollmentStatus Status { get; set; }


    public DateOnly EnrollmentDate { get; set; }
}