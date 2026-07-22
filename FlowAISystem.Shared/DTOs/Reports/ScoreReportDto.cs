namespace FlowAISystem.Shared.DTOs.Reports;

public class ScoreReportDto
{

    public int StudentId { get; set; }


    public string StudentName { get; set; }
        = string.Empty;


    public string CourseName { get; set; }
        = string.Empty;



    public decimal TotalMarks { get; set; }


    public decimal MaxMarks { get; set; }


    public decimal Percentage { get; set; }


    public string Grade { get; set; }
        = string.Empty;

}