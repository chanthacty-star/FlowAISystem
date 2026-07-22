using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Enrollments;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EnrollmentListItemDto>> GetAllAsync(
        EnrollmentSearchDto search)
    {
        var query = _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.CourseOffering!)
                .ThenInclude(c => c.Subject)
            .Include(e => e.CourseOffering!)
                .ThenInclude(c => c.Teacher)
            .Include(e => e.CourseOffering!)
                .ThenInclude(c => c.Semester)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            query = query.Where(e =>
                e.Student!.Name.FirstName.Contains(search.SearchTerm) ||
                e.Student.Name.LastName.Contains(search.SearchTerm) ||
                e.CourseOffering!.Subject!.Name.Contains(search.SearchTerm));
        }

        if (search.StudentId.HasValue)
        {
            query = query.Where(e =>
                e.StudentId == search.StudentId);
        }

        if (search.CourseOfferingId.HasValue)
        {
            query = query.Where(e =>
                e.CourseOfferingId == search.CourseOfferingId);
        }

        if (search.SemesterId.HasValue)
        {
            query = query.Where(e =>
                e.CourseOffering!.SemesterId == search.SemesterId);
        }

        return await query
            .OrderBy(e => e.Student!.Name.FirstName)
            .Select(e => new EnrollmentListItemDto
            {
                Id = e.Id,

                StudentName =
                     $"{e.Student!.Name.FirstName} {e.Student.Name.LastName}",

                CourseName =
                    e.CourseOffering!.Subject!.Name,

                ClassName =
                    e.CourseOffering.ClassName,

                TeacherName =
                    e.CourseOffering.Teacher!.TeacherName,

                SemesterName =
                    e.CourseOffering.Semester!.Name,

                Status =
                    e.Status,

                EnrollmentDate =
                    e.EnrollmentDate
            })
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.CourseOffering!)
                .ThenInclude(c => c.Subject)
            .Include(e => e.CourseOffering!)
                .ThenInclude(c => c.Teacher)
            .Include(e => e.CourseOffering!)
                .ThenInclude(c => c.Semester)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task CreateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Enrollment enrollment)
    {
        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(
        int studentId,
        int courseOfferingId,
        int? ignoreId = null)
    {
        return await _context.Enrollments.AnyAsync(e =>
            e.StudentId == studentId &&
            e.CourseOfferingId == courseOfferingId &&
            (!ignoreId.HasValue || e.Id != ignoreId));
    }
}