using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.Feedbacks;

public class CreateFeedbackDto
{

    [Required]
    public int EnrollmentId { get; set; }



    [Range(1, 5)]
    public int Rating { get; set; }



    [Required]
    public string Comment { get; set; }
        = string.Empty;



    [Required]
    public DateOnly FeedbackDate { get; set; }

}