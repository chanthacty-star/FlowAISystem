using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.AcademicYears;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class AcademicYearRepository : IAcademicYearRepository
{
    private readonly AppDbContext _context;

    public AcademicYearRepository(AppDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // Academic Year List
    // ==========================================

    public async Task<List<AcademicYearListItemDto>> GetAllAsync(
        AcademicYearSearchDto search)
    {
        IQueryable<AcademicYear> query =
            _context.AcademicYears
                .AsNoTracking()
                .Include(a => a.Semesters);

        // Search
        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            string term = search.SearchTerm
                .Trim()
                .ToLower();

            query = query.Where(a =>
                a.Name.ToLower().Contains(term));
        }

        // Current Filter
        if (search.IsCurrent.HasValue)
        {
            query = query.Where(a =>
                a.IsCurrent == search.IsCurrent.Value);
        }

        query = query.OrderByDescending(a => a.StartDate);

        return await query
            .Select(a => new AcademicYearListItemDto
            {
                Id = a.Id,

                Name = a.Name,

                StartDate = a.StartDate,

                EndDate = a.EndDate,

                IsCurrent = a.IsCurrent,

                SemesterCount = a.Semesters.Count
            })
            .ToListAsync();
    }

    // ==========================================
    // Academic Year Details
    // ==========================================

    public async Task<AcademicYearDto?> GetByIdAsync(
        int id)
    {
        return await _context.AcademicYears
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AcademicYearDto
            {
                Id = a.Id,

                Name = a.Name,

                StartDate = a.StartDate,

                EndDate = a.EndDate,

                IsCurrent = a.IsCurrent
            })
            .FirstOrDefaultAsync();
    }

    // ==========================================
    // Create
    // ==========================================

    public async Task CreateAsync(
        CreateAcademicYearDto dto)
    {
        var academicYear =
            new AcademicYear
            {
                Name = dto.Name,

                StartDate = dto.StartDate,

                EndDate = dto.EndDate,

                IsCurrent = dto.IsCurrent
            };

        _context.AcademicYears.Add(academicYear);

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Update
    // ==========================================

    public async Task UpdateAsync(
        UpdateAcademicYearDto dto)
    {
        var academicYear =
            await _context.AcademicYears
                .FirstOrDefaultAsync(a => a.Id == dto.Id);

        if (academicYear == null)
            return;

        academicYear.Name = dto.Name;

        academicYear.StartDate = dto.StartDate;

        academicYear.EndDate = dto.EndDate;

        academicYear.IsCurrent = dto.IsCurrent;

        academicYear.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Delete
    // ==========================================

    public async Task DeleteAsync(
        int id)
    {
        var academicYear =
            await _context.AcademicYears
                .FirstOrDefaultAsync(a => a.Id == id);

        if (academicYear == null)
            return;

        _context.AcademicYears.Remove(academicYear);

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Exists By Name
    // ==========================================

    public async Task<bool> ExistsByNameAsync(
        string name)
    {
        name = name.Trim().ToLower();

        return await _context.AcademicYears
            .AnyAsync(a =>
                a.Name.ToLower() == name);
    }

    // ==========================================
    // Set Current Academic Year
    // ==========================================

    public async Task SetCurrentAsync(
        int id)
    {
        var academicYear =
            await _context.AcademicYears
                .FirstOrDefaultAsync(a => a.Id == id);

        if (academicYear == null)
            return;

        academicYear.IsCurrent = true;

        academicYear.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Clear Current Academic Year
    // ==========================================

    public async Task ClearCurrentAcademicYearAsync()
    {
        var currentYears =
            await _context.AcademicYears
                .Where(a => a.IsCurrent)
                .ToListAsync();

        foreach (var year in currentYears)
        {
            year.IsCurrent = false;
            year.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Semester Count
    // ==========================================

    public async Task<int> GetSemesterCountAsync(
        int academicYearId)
    {
        return await _context.Semesters
            .CountAsync(s =>
                s.AcademicYearId == academicYearId);
    }
}