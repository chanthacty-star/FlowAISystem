namespace FlowAISystem.Shared.DTOs.Students;

public class StudentFeedbackDto
{
    public int Id { get; set; }


    public string SubjectCode { get; set; }
        = string.Empty;


    public string SubjectName { get; set; }
        = string.Empty;



    public string Comment { get; set; }
        = string.Empty;



    public DateOnly CreatedDate { get; set; }


    public string TeacherName { get; set; }
        = string.Empty;
}