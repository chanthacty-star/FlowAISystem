using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.CourseOfferings;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class CourseOfferingRepository
    : ICourseOfferingRepository
{

    private readonly AppDbContext _context;


    public CourseOfferingRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<List<CourseOfferingListItemDto>> GetAllAsync(
        CourseOfferingSearchDto search)
    {
        var query = _context.CourseOfferings
            .AsNoTracking()
            .AsQueryable();

        // 1. General Search Term Filter (ClassName, Subject Name, OR Teacher Name)
        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            var term = search.SearchTerm.Trim().ToLower();

            query = query.Where(c =>
                c.ClassName.ToLower().Contains(term) ||
                (c.Subject != null && c.Subject.Name.ToLower().Contains(term)) ||
                (c.Teacher != null &&
                    (c.Teacher.TeacherName.ToLower().Contains(term) ||
                     c.Teacher.Name.FirstName.ToLower().Contains(term) ||
                     c.Teacher.Name.LastName.ToLower().Contains(term))) ||
                    (c.Semester != null && c.Semester.Name.ToLower().Contains(term))); // Match Semester Name in SearchTerm
        }

        // 2. Subject ID Filter
        if (search.SubjectId.HasValue && search.SubjectId.Value > 0)
        {
            query = query.Where(c => c.SubjectId == search.SubjectId.Value);
        }

        // 3. Teacher ID Filter
        if (search.TeacherId.HasValue && search.TeacherId.Value > 0)
        {
            query = query.Where(c => c.TeacherId == search.TeacherId.Value);
        }
        // 3.1. Specific Semester Name Filter (string?) -> Use !string.IsNullOrWhiteSpace
        if (!string.IsNullOrWhiteSpace(search.SemesterName))
        {
            var semesterTerm = search.SemesterName.Trim().ToLower();

            query = query.Where(c =>
                c.Semester != null &&
                c.Semester.Name.ToLower().Contains(semesterTerm));
        }

        // 4. Semester ID Filter
        if (search.SemesterId.HasValue && search.SemesterId.Value > 0)
        {
            query = query.Where(c => c.SemesterId == search.SemesterId.Value);
        }

        // 5. Specific Teacher Name Filter
        if (!string.IsNullOrWhiteSpace(search.TeacherName))
        {
            var nameTerm = search.TeacherName.Trim().ToLower();

            query = query.Where(c =>
                c.Teacher != null &&
                (c.Teacher.TeacherName.ToLower().Contains(nameTerm) ||
                 c.Teacher.Name.FirstName.ToLower().Contains(nameTerm) ||
                 c.Teacher.Name.LastName.ToLower().Contains(nameTerm) ||
                 (c.Teacher.Name.FirstName + " " + c.Teacher.Name.LastName).ToLower().Contains(nameTerm)));
        }

        // 6. Direct Projection to DTO
        return await query
            .OrderBy(c => c.ClassName)
            .Select(c => new CourseOfferingListItemDto
            {
                Id = c.Id,
                ClassName = c.ClassName,
                SubjectName = c.Subject != null ? c.Subject.Name : "(No Subject)",

                // Constructs full name if TeacherName property is empty
                TeacherName = c.Teacher != null
                    ? (!string.IsNullOrWhiteSpace(c.Teacher.TeacherName)
                        ? c.Teacher.TeacherName
                        : (c.Teacher.Name.FirstName + " " + c.Teacher.Name.LastName).Trim())
                    : "(No Teacher)",

                SemesterName = c.Semester != null ? c.Semester.Name : "(No Semester)",
                Room = c.Room,
                Capacity = c.Capacity, // when the item inside UI list not appear look for Repository
            })
            .ToListAsync();
    }




    public async Task<CourseOffering?> GetByIdAsync(
        int id)
    {

        return await _context.CourseOfferings
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Include(c => c.Semester)
            .FirstOrDefaultAsync(c =>
                c.Id == id);

    }


    public async Task CreateAsync(
        CourseOffering offering)
    {

        _context.CourseOfferings.Add(offering);

        await _context.SaveChangesAsync();

    }


    public async Task UpdateAsync(
        CourseOffering offering)
    {

        _context.CourseOfferings.Update(offering);

        await _context.SaveChangesAsync();

    }

    public async Task DeleteAsync(
        CourseOffering offering)
    {

        _context.CourseOfferings.Remove(offering);

        await _context.SaveChangesAsync();

    }


    public async Task<bool> ExistsAsync(
        string className,
        int subjectId,
        int semesterId,
        int? ignoreId = null)
    {

        return await _context.CourseOfferings.AnyAsync(c =>
            c.ClassName == className &&
            c.SubjectId == subjectId &&
            c.SemesterId == semesterId &&
            (!ignoreId.HasValue ||
             c.Id != ignoreId));

    }

}