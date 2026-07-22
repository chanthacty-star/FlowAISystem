namespace FlowAISystem.Shared.DTOs.Reports;

public class StudentReportDto
{

    public int StudentId { get; set; }


    public string StudentNumber { get; set; }
        = string.Empty;


    public string StudentName { get; set; }
        = string.Empty;


    public string DepartmentName { get; set; }
        = string.Empty;



    public List<StudentCourseReportDto> Courses { get; set; }
        = new();

}