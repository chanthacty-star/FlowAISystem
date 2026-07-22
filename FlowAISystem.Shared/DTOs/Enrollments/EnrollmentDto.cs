using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Enrollments;

public class EnrollmentDto
{
    public int Id { get; set; }


    public DateOnly EnrollmentDate { get; set; }


    public EnrollmentStatus Status { get; set; }



    // Student

    public int StudentId { get; set; }

    public string StudentName { get; set; }
        = string.Empty;



    // Course Offering

    public int CourseOfferingId { get; set; }

    public string ClassName { get; set; }
        = string.Empty;


    public string CourseName { get; set; }
        = string.Empty;


    public string TeacherName { get; set; }
        = string.Empty;


    public string SemesterName { get; set; }
        = string.Empty;
}