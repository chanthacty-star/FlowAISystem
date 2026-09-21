using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Attendances;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AppDbContext _context;

    public AttendanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AttendanceListItemDto>> GetAllAsync(AttendanceSearchDto search)
    {
        var query = _context.Attendances
            .AsNoTracking()
            .AsQueryable();

        // 1. Text Search Filter (Matches Student Name, Course, OR numeric Student ID inside SearchTerm)
        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            var term = search.SearchTerm.Trim().ToLower();

            query = query.Where(a =>
                (a.Enrollment != null && a.Enrollment.Student != null &&
                    (a.Enrollment.Student.Name.FirstName.ToLower().Contains(term) ||
                     a.Enrollment.Student.Name.LastName.ToLower().Contains(term) ||
                     a.Enrollment.Student.Id.ToString().Contains(term))) ||
                (a.Enrollment != null && a.Enrollment.CourseOffering != null &&
                 a.Enrollment.CourseOffering.Subject != null &&
                 a.Enrollment.CourseOffering.Subject.Name.ToLower().Contains(term)));
        }

        // 2. Specific Student ID Filter
        if (search.StudentId.HasValue && search.StudentId.Value > 0)
        {
            query = query.Where(a =>
                a.Enrollment != null &&
                a.Enrollment.StudentId == search.StudentId.Value);
        }

        // 3. Specific Enrollment ID Filter
        if (search.EnrollmentId.HasValue && search.EnrollmentId.Value > 0)
        {
            query = query.Where(a => a.EnrollmentId == search.EnrollmentId.Value);
        }

        // 4. Course Offering Filter
        if (search.CourseOfferingId.HasValue && search.CourseOfferingId.Value > 0)
        {
            query = query.Where(a =>
                a.Enrollment != null &&
                a.Enrollment.CourseOfferingId == search.CourseOfferingId.Value);
        }

        // 5. Date Filter (Option A)
        if (search.AttendanceDate.HasValue && search.AttendanceDate.Value != default)
        {
            var filterDate = DateOnly.FromDateTime(search.AttendanceDate.Value);
            query = query.Where(a => a.AttendanceDate == filterDate);
        }

        // 6. Projection
        return await query
            .OrderByDescending(a => a.AttendanceDate)
            .Select(a => new AttendanceListItemDto
            {
                Id = a.Id,
                StudentName = a.Enrollment != null && a.Enrollment.Student != null
                    ? (a.Enrollment.Student.Name.FirstName + " " + a.Enrollment.Student.Name.LastName).Trim()
                    : "(Unknown Student)",
                CourseName = a.Enrollment != null
                             && a.Enrollment.CourseOffering != null
                             && a.Enrollment.CourseOffering.Subject != null
                    ? a.Enrollment.CourseOffering.Subject.Name
                    : "(Unknown Course)",
                ClassName = a.Enrollment != null && a.Enrollment.CourseOffering != null
                    ? a.Enrollment.CourseOffering.ClassName
                    : "(Unknown Class)",
                AttendanceDate = a.AttendanceDate,
                Status = a.Status
            })
            .ToListAsync();
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances
            .Include(a => a.Enrollment)
                .ThenInclude(e => e!.Student)
            .Include(a => a.Enrollment)
                .ThenInclude(e => e!.CourseOffering)
                    .ThenInclude(c => c!.Subject)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task CreateAsync(Attendance attendance)
    {
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Attendance attendance)
    {
        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(
        int enrollmentId,
        DateOnly attendanceDate,
        int? ignoreId = null)
    {
        return await _context.Attendances.AnyAsync(a =>
            a.EnrollmentId == enrollmentId &&
            a.AttendanceDate == attendanceDate &&
            (!ignoreId.HasValue || a.Id != ignoreId.Value));
    }
}