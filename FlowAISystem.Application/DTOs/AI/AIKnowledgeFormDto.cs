using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Application.DTOs.AI;

public class AIKnowledgeFormDto
{

    [Required]
    [MaxLength(200)]
    public string Question { get; set; } = string.Empty;


    [Required]
    [MaxLength(500)]
    public string Keywords { get; set; } = string.Empty;


    [Required]
    public string Answer { get; set; } = string.Empty;


    [Required]
    public int CategoryId { get; set; }

}