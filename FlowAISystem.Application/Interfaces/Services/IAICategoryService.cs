using FlowAISystem.Application.DTOs.AI.Category;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IAICategoryService
{

    Task<List<AICategoryDto>> GetAllAsync();


    Task<AICategoryDto?> GetByIdAsync(
        int id);


    Task CreateAsync(
        CreateAICategoryDto dto);


    Task UpdateAsync(
        UpdateAICategoryDto dto);


    Task DeleteAsync(
        int id);

}
