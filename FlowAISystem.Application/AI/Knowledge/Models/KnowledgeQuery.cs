namespace FlowAISystem.Application.AI.Knowledge.Models;

public class KnowledgeQuery
{
    public string Topic { get; set; } = string.Empty;
    public string? Level { get; set; }
    public int? UserId { get; set; }
}