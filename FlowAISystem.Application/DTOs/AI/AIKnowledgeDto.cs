namespace FlowAISystem.Application.DTOs.AI;

public class AIKnowledgeDto
{

    public int Id { get; set; }


    public string Question { get; set; }
        = string.Empty;


    public string Keywords { get; set; }
        = string.Empty;


    public string Answer { get; set; }
        = string.Empty;


    public int CategoryId { get; set; }


    public string CategoryName { get; set; }
        = string.Empty;


    public DateTime CreatedAt { get; set; }

}