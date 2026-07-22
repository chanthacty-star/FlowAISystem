using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class Score : BaseEntity
{
    public string AssessmentName { get; set; } = string.Empty;

    public decimal Marks { get; set; }

    public decimal MaxMarks { get; set; }

    public DateOnly AssessmentDate { get; set; }


    public int EnrollmentId { get; set; }

    public Enrollment? Enrollment { get; set; }
}