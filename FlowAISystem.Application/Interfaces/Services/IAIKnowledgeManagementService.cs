using FlowAISystem.Application.DTOs.AI;
using FlowAISystem.Application.DTOs.AI.Category;

namespace FlowAISystem.Application.Interfaces.Services;


public interface IAIKnowledgeManagementService
{
    Task<List<AIKnowledgeDto>> GetAllAsync();

    Task<AIKnowledgeDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateAIKnowledgeDto dto);

    Task UpdateAsync(UpdateAIKnowledgeDto dto);

    Task DeleteAsync(int id);

    Task<List<AICategoryDto>> GetCategoriesAsync();
}
