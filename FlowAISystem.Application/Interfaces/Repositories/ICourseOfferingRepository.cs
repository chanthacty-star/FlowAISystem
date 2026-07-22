using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.CourseOfferings;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface ICourseOfferingRepository
{
    Task<List<CourseOfferingListItemDto>> GetAllAsync(
        CourseOfferingSearchDto search);


    Task<CourseOffering?> GetByIdAsync(
        int id);


    Task CreateAsync(
        CourseOffering offering);


    Task UpdateAsync(
        CourseOffering offering);


    Task DeleteAsync(
        CourseOffering offering);


    Task<bool> ExistsAsync(
        string className,
        int subjectId,
        int semesterId,
        int? ignoreId = null);
}