using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Enrollments;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IEnrollmentRepository
{

    Task<List<EnrollmentListItemDto>> GetAllAsync(
        EnrollmentSearchDto search);



    Task<Enrollment?> GetByIdAsync(
        int id);



    Task CreateAsync(
        Enrollment enrollment);



    Task UpdateAsync(
        Enrollment enrollment);



    Task DeleteAsync(
        Enrollment enrollment);



    Task<bool> ExistsAsync(
        int studentId,
        int courseOfferingId,
        int? ignoreId = null);

}