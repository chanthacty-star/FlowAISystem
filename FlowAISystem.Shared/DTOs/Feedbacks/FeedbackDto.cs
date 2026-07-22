namespace FlowAISystem.Shared.DTOs.Feedbacks;

public class FeedbackDto
{

    public int Id { get; set; }



    public int Rating { get; set; }



    public string Comment { get; set; }
        = string.Empty;



    public DateOnly FeedbackDate { get; set; }



    // Enrollment

    public int EnrollmentId { get; set; }



    // Display Information

    public string StudentName { get; set; }
        = string.Empty;



    public string CourseName { get; set; }
        = string.Empty;



    public string TeacherName { get; set; }
        = string.Empty;



    public string SemesterName { get; set; }
        = string.Empty;

}