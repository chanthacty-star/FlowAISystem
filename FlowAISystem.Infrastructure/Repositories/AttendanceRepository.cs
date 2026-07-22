using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Attendances;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{

    private readonly AppDbContext _context;


    public AttendanceRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<List<AttendanceListItemDto>> GetAllAsync(
        AttendanceSearchDto search)
    {

        var query =
            _context.Attendances

            .Include(a => a.Enrollment!)
                .ThenInclude(e => e.Student)

            .Include(a => a.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Subject)

            .AsQueryable();



        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {

            query = query.Where(a =>
                a.Enrollment!.Student!.Name.FirstName
                    .Contains(search.SearchTerm)

                ||

                a.Enrollment.Student.Name.LastName
                    .Contains(search.SearchTerm)

                ||

                a.Enrollment.CourseOffering!
                    .Subject!.Name
                    .Contains(search.SearchTerm));

        }



        if (search.EnrollmentId.HasValue)
        {
            query = query.Where(a =>
                a.EnrollmentId == search.EnrollmentId);
        }



        if (search.AttendanceDate.HasValue)
        {
            query = query.Where(a =>
                a.AttendanceDate ==
                search.AttendanceDate);
        }



        if (search.CourseOfferingId.HasValue)
        {
            query = query.Where(a =>
                a.Enrollment!
                .CourseOfferingId ==
                search.CourseOfferingId);
        }



        return await query

            .OrderByDescending(a =>
                a.AttendanceDate)

            .Select(a => new AttendanceListItemDto
            {

                Id = a.Id,


                StudentName =
                    a.Enrollment!
                    .Student!
                    .Name
                    .ToString(),


                CourseName =
                    a.Enrollment!
                    .CourseOffering!
                    .Subject!
                    .Name,


                ClassName =
                    a.Enrollment!
                    .CourseOffering!
                    .ClassName,


                AttendanceDate =
                    a.AttendanceDate,


                Status =
                    a.Status

            })

            .ToListAsync();

    }



    public async Task<Attendance?> GetByIdAsync(
        int id)
    {

        return await _context.Attendances

            .Include(a => a.Enrollment!)
                .ThenInclude(e => e.Student)

            .Include(a => a.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Subject)

            .FirstOrDefaultAsync(a =>
                a.Id == id);

    }




    public async Task CreateAsync(
        Attendance attendance)
    {

        _context.Attendances.Add(attendance);

        await _context.SaveChangesAsync();

    }




    public async Task UpdateAsync(
        Attendance attendance)
    {

        _context.Attendances.Update(attendance);

        await _context.SaveChangesAsync();

    }




    public async Task DeleteAsync(
        Attendance attendance)
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

            a.EnrollmentId == enrollmentId

            &&

            a.AttendanceDate == attendanceDate

            &&

            (!ignoreId.HasValue ||
             a.Id != ignoreId)

        );

    }

}