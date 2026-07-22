using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Departments;

namespace FlowAISystem.Application.Services;

public class DepartmentService
    : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(
        IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DepartmentListItemDto>> GetAllAsync(
        DepartmentSearchDto search)
    {
        var departments =
            await _repository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search.Keyword))
        {
            departments = departments
                .Where(x =>
                    x.Name.Contains(
                        search.Keyword,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    x.Code.Contains(
                        search.Keyword,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return departments
            .Select(x => new DepartmentListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                StudentCount = x.Students.Count,
                TeacherCount = x.Teachers.Count
            })
            .ToList();
    }

    public async Task<DepartmentDto?> GetByIdAsync(
        int id)
    {
        var department =
            await _repository.GetByIdAsync(id);

        if (department == null)
            return null;

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Code = department.Code,
            Description = department.Description,
            StudentCount = department.Students.Count,
            TeacherCount = department.Teachers.Count
        };
    }

    public async Task CreateAsync(
        CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description
        };

        await _repository.AddAsync(department);
    }

    public async Task UpdateAsync(
        UpdateDepartmentDto dto)
    {
        var department =
            await _repository.GetByIdAsync(dto.Id);

        if (department == null)
            return;

        department.Name = dto.Name;
        department.Code = dto.Code;
        department.Description = dto.Description;

        await _repository.UpdateAsync(department);
    }

    public async Task DeleteAsync(
        int id)
    {
        await _repository.DeleteAsync(id);
    }
}

