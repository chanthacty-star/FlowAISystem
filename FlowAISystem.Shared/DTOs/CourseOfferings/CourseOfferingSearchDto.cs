namespace FlowAISystem.Shared.DTOs.CourseOfferings;

public class CourseOfferingSearchDto
{
    public string SearchTerm { get; set; } = string.Empty;


    public int? SubjectId { get; set; }


    public int? TeacherId { get; set; }

    public string? TeacherName { get; set; } // then make sure in Repository exist TeacherName


    public int? SemesterId { get; set; }

    // Text search by Semester Name (string?) e.g. "Spring 2026" or "Fall"
    public string? SemesterName { get; set; }
}