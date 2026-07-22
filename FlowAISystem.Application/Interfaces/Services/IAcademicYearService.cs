using FlowAISystem.Shared.DTOs.AcademicYears;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IAcademicYearService
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

    Task SetCurrentAsync(
        int id);
}