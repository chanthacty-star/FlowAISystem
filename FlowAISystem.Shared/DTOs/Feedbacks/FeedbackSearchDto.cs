namespace FlowAISystem.Shared.DTOs.Feedbacks;

public class FeedbackSearchDto
{

    public string SearchTerm { get; set; }
        = string.Empty;



    public int? Rating { get; set; }



    public int? TeacherId { get; set; }



    public int? CourseId { get; set; }



    public DateOnly? FromDate { get; set; }



    public DateOnly? ToDate { get; set; }

}