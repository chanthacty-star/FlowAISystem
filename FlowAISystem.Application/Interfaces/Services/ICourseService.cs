using FlowAISystem.Shared.DTOs.Courses;

namespace FlowAISystem.Application.Interfaces.Services;

public interface ICourseService
{
    Task<List<CourseListItemDto>> GetAllAsync(
        CourseSearchDto search);

    Task<CourseDto?> GetByIdAsync(
        int id);

    Task CreateAsync(
        CreateCourseDto dto);

    Task UpdateAsync(
        UpdateCourseDto dto);

    Task DeleteAsync(
        int id);
}