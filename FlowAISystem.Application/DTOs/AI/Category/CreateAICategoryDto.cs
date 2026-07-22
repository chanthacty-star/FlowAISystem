using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Application.DTOs.AI.Category;

public class CreateAICategoryDto
{

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
        = string.Empty;


    [MaxLength(500)]
    public string Description { get; set; }
        = string.Empty;

}