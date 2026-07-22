using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.Scores;

public class UpdateScoreDto
{

    public int Id { get; set; }



    [Required]
    public int EnrollmentId { get; set; }



    [Required]
    [StringLength(100)]
    public string AssessmentName { get; set; }
        = string.Empty;



    public decimal Marks { get; set; }



    public decimal MaxMarks { get; set; }



    public DateOnly AssessmentDate { get; set; }

}