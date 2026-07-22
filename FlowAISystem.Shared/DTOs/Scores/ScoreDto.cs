namespace FlowAISystem.Shared.DTOs.Scores;

public class ScoreDto
{
    public int Id { get; set; }


    public string AssessmentName { get; set; }
        = string.Empty;


    public decimal Marks { get; set; }


    public decimal MaxMarks { get; set; }


    public DateOnly AssessmentDate { get; set; }


    public int EnrollmentId { get; set; }


    public string StudentName { get; set; }
        = string.Empty;


    public string CourseName { get; set; }
        = string.Empty;
}