using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Application.DTOs.AI;

public class UpdateAIKnowledgeDto
    : AIKnowledgeFormDto
{

    public int Id { get; set; }

}