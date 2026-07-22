namespace FlowAISystem.Shared.DTOs.Students;

public class StudentSearchDto
{
    public string? SearchTerm { get; set; }

    public int? DepartmentId { get; set; }

    public string SortColumn { get; set; } = "StudentNumber";

    public bool SortDescending { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}