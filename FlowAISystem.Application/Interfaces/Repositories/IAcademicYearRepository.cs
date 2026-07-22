using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.AcademicYears;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IAcademicYearRepository
{
    Task<List<AcademicYearListItemDto>> GetAllAsync(
        AcademicYearSearchDto search);

    Task<AcademicYearDto?> GetByIdAsync(
        int id);

    Task CreateAsync(
        CreateAcademicYearDto dto);

    Task UpdateAsync(
        UpdateAcademicYearDto dto);

    Task DeleteAsync(
        int id);

    Task<bool> ExistsByNameAsync(
        string name);

    Task SetCurrentAsync(
        int id);

    Task<int> GetSemesterCountAsync(
        int academicYearId);

    Task ClearCurrentAcademicYearAsync();

}