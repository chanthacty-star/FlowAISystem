namespace FlowAISystem.Shared.DTOs.Reports;

public class AIKnowledgeReportDto
{
    public string Question { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}