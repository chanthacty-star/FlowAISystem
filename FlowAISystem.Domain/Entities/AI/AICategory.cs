using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Domain.Entities;

public class AICategory
{
    public int Id { get; set; }



    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
        = string.Empty;



    [MaxLength(500)]
    public string Description { get; set; }
        = string.Empty;



    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;



    // Navigation Property
    // One Category has many AIKnowledge records

    public ICollection<AIKnowledge> KnowledgeItems { get; set; }
        = new List<AIKnowledge>();
}