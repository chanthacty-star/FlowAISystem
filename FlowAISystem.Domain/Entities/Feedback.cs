using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class Feedback : BaseEntity
{

    // Rating 1 - 5

    public int Rating { get; set; }



    // Student comment

    public string Comment { get; set; }
        = string.Empty;



    public DateOnly FeedbackDate { get; set; }



    // Enrollment Relationship

    public int EnrollmentId { get; set; }


    public Enrollment? Enrollment { get; set; }

}