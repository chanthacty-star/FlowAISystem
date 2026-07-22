using FlowAISystem.Shared.DTOs.Departments;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IDepartmentService
{
    Task<List<DepartmentListItemDto>> GetAllAsync(
        DepartmentSearchDto search);

    Task<DepartmentDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateDepartmentDto dto);

    Task UpdateAsync(UpdateDepartmentDto dto);

    Task DeleteAsync(int id);
}
