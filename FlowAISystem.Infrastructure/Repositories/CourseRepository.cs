using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Courses;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;


    public CourseRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<List<CourseListItemDto>> GetAllAsync(
        CourseSearchDto search)
    {
        var query =
            _context.Subjects
                .Include(s => s.Department)

                .Include(s => s.CourseOfferings)
                    .ThenInclude(c => c.Teacher)

                .Include(s => s.CourseOfferings)
                    .ThenInclude(c => c.Semester)

                .AsQueryable();



        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            query = query.Where(s =>
                s.Name.Contains(search.SearchTerm) ||
                s.Code.Contains(search.SearchTerm));
        }



        if (search.DepartmentId.HasValue)
        {
            query = query.Where(s =>
                s.DepartmentId == search.DepartmentId);
        }



        if (search.TeacherId.HasValue)
        {
            query = query.Where(s =>
                s.CourseOfferings
                    .Any(c =>
                        c.TeacherId == search.TeacherId));
        }



        if (search.SemesterId.HasValue)
        {
            query = query.Where(s =>
                s.CourseOfferings
                    .Any(c =>
                        c.SemesterId == search.SemesterId));
        }



        return await query
            .OrderBy(s => s.Code)

            .Select(s => new CourseListItemDto
            {
                Id = s.Id,

                Code = s.Code,

                Name = s.Name,

                Credits = s.Credits,


                DepartmentName =
                    s.Department != null
                        ? s.Department.Name
                        : string.Empty,


                TeacherName =
                    s.CourseOfferings
                        .Select(c => c.Teacher!.TeacherName)
                        .FirstOrDefault()
                    ?? string.Empty,


                SemesterName =
                    s.CourseOfferings
                        .Select(c => c.Semester!.Name)
                        .FirstOrDefault()
                    ?? string.Empty

            })

            .ToListAsync();
    }





    public async Task<Subject?> GetByIdAsync(
        int id)
    {
        return await _context.Subjects

            .Include(s => s.Department)

            .Include(s => s.CourseOfferings)
                .ThenInclude(c => c.Teacher)

            .Include(s => s.CourseOfferings)
                .ThenInclude(c => c.Semester)

            .FirstOrDefaultAsync(
                s => s.Id == id);
    }





    public async Task CreateAsync(
        Subject subject)
    {
        _context.Subjects.Add(subject);

        await _context.SaveChangesAsync();
    }





    public async Task UpdateAsync(
        Subject subject)
    {
        _context.Subjects.Update(subject);

        await _context.SaveChangesAsync();
    }





    public async Task DeleteAsync(
        Subject subject)
    {
        _context.Subjects.Remove(subject);

        await _context.SaveChangesAsync();
    }





    public async Task<bool> ExistsCodeAsync(
        string code,
        int? ignoreId = null)
    {
        return await _context.Subjects
            .AnyAsync(s =>
                s.Code == code &&
                (!ignoreId.HasValue ||
                 s.Id != ignoreId));
    }
}