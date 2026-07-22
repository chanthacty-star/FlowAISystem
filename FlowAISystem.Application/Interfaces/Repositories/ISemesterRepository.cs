using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Semesters;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface ISemesterRepository
{
    Task<List<Semester>> GetAllAsync(
        SemesterSearchDto search);

    Task<Semester?> GetByIdAsync(
        int id);

    Task AddAsync(
        Semester semester);

    Task UpdateAsync(
        Semester semester);

    Task DeleteAsync(
        int id);

    Task SetCurrentAsync(
        int id);
}