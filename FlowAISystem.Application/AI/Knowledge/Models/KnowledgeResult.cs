namespace FlowAISystem.Application.AI.Knowledge.Models;

public class KnowledgeResult
{
    public string Content { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string? Title { get; set; }
    public double RelevanceScore { get; set; }
}