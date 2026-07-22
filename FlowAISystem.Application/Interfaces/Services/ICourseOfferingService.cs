using FlowAISystem.Shared.DTOs.CourseOfferings;

namespace FlowAISystem.Application.Interfaces.Services;

public interface ICourseOfferingService
{

    Task<List<CourseOfferingListItemDto>> GetAllAsync(
        CourseOfferingSearchDto search);



    Task<CourseOfferingDto?> GetByIdAsync(
        int id);



    Task CreateAsync(
        CreateCourseOfferingDto dto);



    Task UpdateAsync(
        UpdateCourseOfferingDto dto);



    Task DeleteAsync(
        int id);

}