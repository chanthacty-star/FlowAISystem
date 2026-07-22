using FlowAISystem.Shared.DTOs.Semesters;

namespace FlowAISystem.Application.Interfaces.Services;

public interface ISemesterService
{
    Task<List<SemesterListItemDto>> GetAllAsync(
        SemesterSearchDto search);

    Task DeleteAsync(int id);

    Task SetCurrentAsync(int id);

    Task<SemesterDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateSemesterDto dto);

    Task UpdateAsync(UpdateSemesterDto dto);


}