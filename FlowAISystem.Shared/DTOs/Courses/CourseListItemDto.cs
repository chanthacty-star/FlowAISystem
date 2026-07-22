namespace FlowAISystem.Shared.DTOs.Courses;

public class CourseListItemDto
{

    public int Id { get; set; }


    public string Code { get; set; }
        = string.Empty;


    public string Name { get; set; }
        = string.Empty;


    public int Credits { get; set; }



    public string DepartmentName { get; set; }
        = string.Empty;



    // Current Offering Information

    public string TeacherName { get; set; }
        = string.Empty;


    public string SemesterName { get; set; }
        = string.Empty;

}