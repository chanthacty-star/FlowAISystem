using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.Feedbacks;

public class UpdateFeedbackDto
{

    public int Id { get; set; }



    [Range(1, 5)]
    public int Rating { get; set; }



    [Required]
    public string Comment { get; set; }
        = string.Empty;



    [Required]
    public DateOnly FeedbackDate { get; set; }



    public int EnrollmentId { get; set; }

}