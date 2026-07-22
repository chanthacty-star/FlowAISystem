namespace FlowAISystem.Shared.DTOs.Feedbacks;

public class FeedbackListItemDto
{

    public int Id { get; set; }



    public string StudentName { get; set; }
        = string.Empty;



    public string CourseName { get; set; }
        = string.Empty;



    public string TeacherName { get; set; }
        = string.Empty;



    public int Rating { get; set; }



    public string Comment { get; set; }
        = string.Empty;



    public DateOnly FeedbackDate { get; set; }

}