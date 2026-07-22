using FlowAISystem.Domain.Entities;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync();

    Task<Department?> GetByIdAsync(int id);

    Task AddAsync(Department department);

    Task UpdateAsync(Department department);

    Task DeleteAsync(int id);
}