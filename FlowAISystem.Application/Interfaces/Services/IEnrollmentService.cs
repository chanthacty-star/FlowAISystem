using FlowAISystem.Shared.DTOs.Enrollments;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IEnrollmentService
{

    Task<List<EnrollmentListItemDto>> GetAllAsync(
        EnrollmentSearchDto search);



    Task<EnrollmentDto?> GetByIdAsync(
        int id);



    Task CreateAsync(
        CreateEnrollmentDto dto);



    Task UpdateAsync(
        UpdateEnrollmentDto dto);



    Task DeleteAsync(
        int id);

}