using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Semesters;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Application.Services;

public class SemesterService : ISemesterService
{
    private readonly ISemesterRepository _repository;

    public SemesterService(
        ISemesterRepository repository)
    {
        _repository = repository;
    }
    // get all
    public async Task<List<SemesterListItemDto>> GetAllAsync(
    SemesterSearchDto search)
    {
        var semesters =
            await _repository.GetAllAsync(search);

        return semesters.Select(x => new SemesterListItemDto
        {
            Id = x.Id,
            Name = x.Name,
            SemesterType = x.SemesterType,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            IsCurrent = x.IsCurrent,
            AcademicYearName = x.AcademicYear?.Name ?? "",
            SubjectCount = x.Subjects.Count
        }).ToList();
    }
    // get by ID
    public async Task<SemesterDto?> GetByIdAsync(
    int id)
    {
        var semester =
            await _repository.GetByIdAsync(id);

        if (semester == null)
            return null;

        return new SemesterDto
        {
            Id = semester.Id,
            Name = semester.Name,
            SemesterType = semester.SemesterType,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate,
            IsCurrent = semester.IsCurrent,
            AcademicYearId = semester.AcademicYearId,
            AcademicYearName =
                semester.AcademicYear?.Name ?? ""
        };
    }
    // create
    public async Task CreateAsync(
    CreateSemesterDto dto)
    {
        if (dto.EndDate < dto.StartDate)
            throw new Exception(
                "End Date must be after Start Date.");

        var semester = new Semester
        {
            Name = dto.Name,
            SemesterType = dto.SemesterType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            AcademicYearId = dto.AcademicYearId,
            IsCurrent = dto.IsCurrent
        };

        await _repository.AddAsync(semester);

        if (dto.IsCurrent)
            await _repository.SetCurrentAsync(semester.Id);
    }
    // upadet 
    public async Task UpdateAsync(
    UpdateSemesterDto dto)
    {
        if (dto.EndDate < dto.StartDate)
            throw new Exception(
                "End Date must be after Start Date.");

        var semester =
            await _repository.GetByIdAsync(dto.Id);

        if (semester == null)
            throw new Exception("Semester not found.");

        semester.Name = dto.Name;

        semester.SemesterType = dto.SemesterType;

        semester.StartDate = dto.StartDate;

        semester.EndDate = dto.EndDate;

        semester.AcademicYearId = dto.AcademicYearId;

        semester.IsCurrent = dto.IsCurrent;

        await _repository.UpdateAsync(semester);

        if (dto.IsCurrent)
            await _repository.SetCurrentAsync(dto.Id);
    }
    // delete 
    public async Task DeleteAsync(
    int id)
    {
        await _repository.DeleteAsync(id);
    }
    // setcurrent
    public async Task SetCurrentAsync(
    int id)
    {
        await _repository.SetCurrentAsync(id);
    }
}