namespace FlowAISystem.Shared.DTOs.AcademicYears;

public class AcademicYearSearchDto
{
    public string SearchTerm { get; set; } = string.Empty;

    public bool? IsCurrent { get; set; }

    public string SortColumn { get; set; } = "Name";

    public bool SortDescending { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}