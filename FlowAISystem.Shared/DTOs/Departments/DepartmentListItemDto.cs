namespace FlowAISystem.Shared.DTOs.Departments;

public class DepartmentListItemDto
{
    public int Id { get; set; }

    public string Name { get; set; }
        = string.Empty;

    public string Code { get; set; }
        = string.Empty;

    public int StudentCount { get; set; }

    public int TeacherCount { get; set; }
}