using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Domain.Entities;

public class AIKnowledge
{
    public int Id { get; set; }


    [Required]
    [MaxLength(200)]
    public string Question { get; set; }
        = string.Empty;


    [Required]
    [MaxLength(500)]
    public string Keywords { get; set; }
        = string.Empty;


    [Required]
    public string Answer { get; set; }
        = string.Empty;


    // Foreign Key
    public int CategoryId { get; set; }


    // Navigation Property
    public AICategory Category { get; set; }
        = null!;


    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}