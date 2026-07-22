using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Courses;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<List<CourseListItemDto>> GetAllAsync(
        CourseSearchDto search);

    Task<Subject?> GetByIdAsync(int id);

    Task CreateAsync(Subject subject);

    Task UpdateAsync(Subject subject);

    Task DeleteAsync(Subject subject);

    Task<bool> ExistsCodeAsync(
        string code,
        int? ignoreId = null);
}