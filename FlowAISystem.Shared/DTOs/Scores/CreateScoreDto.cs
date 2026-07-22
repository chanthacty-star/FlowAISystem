using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.Scores;

public class CreateScoreDto
{

    [Required]
    public int EnrollmentId { get; set; }



    [Required]
    [StringLength(100)]
    public string AssessmentName { get; set; }
        = string.Empty;



    [Range(0, 100)]
    public decimal Marks { get; set; }



    [Range(1, 100)]
    public decimal MaxMarks { get; set; }



    [Required]
    public DateOnly AssessmentDate { get; set; }

}