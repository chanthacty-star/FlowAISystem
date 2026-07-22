namespace FlowAISystem.Shared.DTOs.CourseOfferings;

public class CourseOfferingDto
{
    public int Id { get; set; }


    public string ClassName { get; set; } = string.Empty;


    public int Capacity { get; set; }


    public string? Room { get; set; }



    // Subject

    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = string.Empty;



    // Teacher

    public int TeacherId { get; set; }

    public string TeacherName { get; set; } = string.Empty;



    // Semester

    public int SemesterId { get; set; }

    public string SemesterName { get; set; } = string.Empty;
}