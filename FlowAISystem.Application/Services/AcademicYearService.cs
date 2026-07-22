using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AcademicYears;

namespace FlowAISystem.Application.Services;

public class AcademicYearService : IAcademicYearService
{
    private readonly IAcademicYearRepository _repository;

    public AcademicYearService(
        IAcademicYearRepository repository)
    {
        _repository = repository;
    }

    // ==================================================
    // Academic Year List
    // ==================================================

    public async Task<List<AcademicYearListItemDto>> GetAllAsync(
        AcademicYearSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }

    // ==================================================
    // Academic Year Details
    // ==================================================

    public async Task<AcademicYearDto?> GetByIdAsync(
        int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    // ==================================================
    // Create
    // ==================================================

    public async Task CreateAsync(
        CreateAcademicYearDto dto)
    {
        // Validation

        if (dto.EndDate <= dto.StartDate)
        {
            throw new Exception(
                "End Date must be later than Start Date.");
        }

        // If this Academic Year is current,
        // remove Current from the previous one.

        if (dto.IsCurrent)
        {
            await _repository.ClearCurrentAcademicYearAsync();
        }

        await _repository.CreateAsync(dto);
    }

    // ==================================================
    // Update
    // ==================================================

    public async Task UpdateAsync(
        UpdateAcademicYearDto dto)
    {
        if (dto.EndDate <= dto.StartDate)
        {
            throw new Exception(
                "End Date must be later than Start Date.");
        }

        if (dto.IsCurrent)
        {
            await _repository.ClearCurrentAcademicYearAsync();
        }

        await _repository.UpdateAsync(dto);
    }

    // ==================================================
    // Delete
    // ==================================================

    public async Task DeleteAsync(
        int id)
    {
        await _repository.DeleteAsync(id);
    }

    // ==================================================
    // Set Current Academic Year
    // ==================================================

    public async Task SetCurrentAsync(
        int id)
    {
        await _repository.ClearCurrentAcademicYearAsync();

        await _repository.SetCurrentAsync(id);
    }
}