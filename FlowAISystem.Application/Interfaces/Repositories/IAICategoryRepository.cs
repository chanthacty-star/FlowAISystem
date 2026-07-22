using FlowAISystem.Domain.Entities;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IAICategoryRepository
{

    Task<List<AICategory>> GetAllAsync();


    Task<AICategory?> GetByIdAsync(
        int id);


    Task AddAsync(
        AICategory category);


    Task UpdateAsync(
        AICategory category);


    Task DeleteAsync(
        int id);

}