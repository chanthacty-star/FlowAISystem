using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Semesters;

using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class SemesterRepository : ISemesterRepository
{
    private readonly AppDbContext _context;

    public SemesterRepository(AppDbContext context)
    {
        _context = context;
    }
    // Getallsemester by 
    public async Task<List<Semester>> GetAllAsync(
        SemesterSearchDto search)
    {
        var query =
            _context.Semesters
                    .Include(x => x.AcademicYear)
                    .Include(x => x.Subjects)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            query = query.Where(x =>
                x.Name.Contains(search.SearchTerm));
        }

        if (search.AcademicYearId.HasValue)
        {
            query = query.Where(x =>
                x.AcademicYearId ==
                search.AcademicYearId.Value);
        }

        if (search.IsCurrent.HasValue)
        {
            query = query.Where(x =>
                x.IsCurrent ==
                search.IsCurrent.Value);
        }

        return await query
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();
    }

    // get by ID
    public async Task<Semester?> GetByIdAsync(
    int id)
    {
        return await _context.Semesters
            .Include(x => x.AcademicYear)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    // add 
    public async Task AddAsync(
    Semester semester)
    {
        await _context.Semesters.AddAsync(semester);

        await _context.SaveChangesAsync();
    }
    // update 
    public async Task UpdateAsync(
    Semester semester)
    {
        _context.Semesters.Update(semester);

        await _context.SaveChangesAsync();
    }

    // delete
    public async Task DeleteAsync(
    int id)
    {
        var semester =
            await _context.Semesters.FindAsync(id);

        if (semester == null)
            return;

        _context.Semesters.Remove(semester);

        await _context.SaveChangesAsync();
    }
    // setcurrent 
    public async Task SetCurrentAsync(
    int id)
    {
        var semesters =
            await _context.Semesters.ToListAsync();

        foreach (var semester in semesters)
        {
            semester.IsCurrent = semester.Id == id;
        }

        await _context.SaveChangesAsync();
    }
    // 
}