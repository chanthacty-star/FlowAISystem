using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Reports;
using FlowAISystem.Shared.Enums;
using Microsoft.EntityFrameworkCore;


namespace FlowAISystem.Infrastructure.Repositories;


public class ReportRepository : IReportRepository
{

    private readonly AppDbContext _context;


    public ReportRepository(
        AppDbContext context)
    {
        _context = context;
    }


    // =====================================================
    // Student Report
    // =====================================================

    public async Task<StudentReportDto?> GetStudentReportAsync(
        int studentId)
    {

        return await _context.Students

            .Where(s => s.Id == studentId)

            .Select(s => new StudentReportDto
            {

                StudentId = s.Id,


                StudentNumber =
                    s.StudentNumber,


                StudentName =
                    s.Name.FirstName
                    + " "
                    + s.Name.LastName,


                Courses =
                    s.Enrollments
                    .Select(e => new StudentCourseReportDto
                    {

                        CourseName =
                            e.CourseOffering.Subject.Name

                    })
                    .ToList()

            })

            .FirstOrDefaultAsync();

    }


    // =====================================================
    // Attendance Report
    // =====================================================

    public async Task<List<AttendanceReportDto>>
        GetAttendanceReportAsync(
            int? courseOfferingId)
    {

        var query =
            _context.Attendances
            .Include(a => a.Enrollment)
                .ThenInclude(e => e.Student)
            .Include(a => a.Enrollment)
                .ThenInclude(e => e.CourseOffering)
                    .ThenInclude(c => c.Subject)
            .AsQueryable();



        if (courseOfferingId.HasValue)
        {

            query =
                query.Where(a =>
                    a.Enrollment.CourseOfferingId
                    == courseOfferingId.Value);

        }



        return await query

            .GroupBy(a => new
            {

                StudentId =
                    a.Enrollment.StudentId,


                StudentName =
                    a.Enrollment.Student.Name.FirstName
                    + " "
                    + a.Enrollment.Student.Name.LastName,


                CourseName =
                    a.Enrollment.CourseOffering.Subject.Name

            })


            .Select(g => new AttendanceReportDto
            {

                StudentId =
                    g.Key.StudentId,


                StudentName =
                    g.Key.StudentName,


                CourseName =
                    g.Key.CourseName,


                TotalSessions =
                    g.Count(),


                PresentCount =
                    g.Count(x =>
                        x.Status ==
                        AttendanceStatus.Present),


                AbsentCount =
                    g.Count(x =>
                        x.Status ==
                        AttendanceStatus.Absent),



                AttendanceRate =
                    g.Count() == 0
                    ?
                    0
                    :
                    (
                        (decimal)
                        g.Count(x =>
                            x.Status ==
                            AttendanceStatus.Present)
                        /
                        g.Count()
                    )
                    * 100


            })

            .ToListAsync();

    }



    // =====================================================
    // Score Report
    // =====================================================


    public async Task<List<ScoreReportDto>>
        GetScoreReportAsync(
            int? courseOfferingId)
    {

        var query =
            _context.Scores

            .Include(s => s.Enrollment)
                .ThenInclude(e => e.Student)

            .Include(s => s.Enrollment)
                .ThenInclude(e => e.CourseOffering)
                    .ThenInclude(c => c.Subject)

            .AsQueryable();




        if (courseOfferingId.HasValue)
        {

            query =
                query.Where(s =>
                    s.Enrollment.CourseOfferingId
                    ==
                    courseOfferingId.Value);

        }




        return await query

            .GroupBy(s => new
            {

                StudentId =
                    s.Enrollment.StudentId,


                StudentName =
                    s.Enrollment.Student.Name.FirstName
                    + " "
                    + s.Enrollment.Student.Name.LastName,


                CourseName =
                    s.Enrollment.CourseOffering.Subject.Name

            })



            .Select(g => new ScoreReportDto
            {

                StudentId =
                    g.Key.StudentId,


                StudentName =
                    g.Key.StudentName,


                CourseName =
                    g.Key.CourseName,


                TotalMarks =
                    g.Sum(x =>
                        x.Marks),


                MaxMarks =
                    g.Sum(x =>
                        x.MaxMarks),


                Percentage =
                    g.Sum(x => x.MaxMarks) == 0
                    ?
                    0
                    :
                    (
                        g.Sum(x => x.Marks)
                        /
                        g.Sum(x => x.MaxMarks)
                    )
                    * 100,


                Grade =
                    CalculateGrade(
                        g.Sum(x => x.Marks),
                        g.Sum(x => x.MaxMarks))

            })

            .ToListAsync();

    }

    // =====================================================
    // Enrollment Report
    // =====================================================


    public async Task<List<EnrollmentReportDto>>
        GetEnrollmentReportAsync(
            int? semesterId)
    {

        var query =
            _context.Enrollments
            .Include(e => e.CourseOffering)
                .ThenInclude(c => c.Subject)
            .AsQueryable();



        if (semesterId.HasValue)
        {

            query =
                query.Where(e =>
                    e.CourseOffering.SemesterId
                    ==
                    semesterId.Value);

        }



        return await query

            .GroupBy(e =>
                e.CourseOffering.Subject.Name)


            .Select(g => new EnrollmentReportDto
            {

                CourseName =
                    g.Key,


                TotalStudents =
                    g.Count()

            })

            .ToListAsync();

    }


    // =====================================================
    // Dashboard Report
    // =====================================================


    public async Task<DashboardReportDto>
        GetDashboardReportAsync()
    {

        return new DashboardReportDto
        {

            TotalStudents =
                await _context.Students.CountAsync(),


            TotalTeachers =
                await _context.Teachers.CountAsync(),


            TotalCourses =
                await _context.Subjects.CountAsync(),


            TotalEnrollments =
                await _context.Enrollments.CountAsync()

        };

    }


    // =====================================================
    // Grade Calculation
    // =====================================================


    private static string CalculateGrade(
        decimal totalMarks,
        decimal maxMarks)
    {

        if (maxMarks == 0)
            return "N/A";


        var percentage =
            totalMarks / maxMarks * 100;



        return percentage switch
        {

            >= 90 => "A",

            >= 80 => "B",

            >= 70 => "C",

            >= 60 => "D",

            _ => "F"

        };

    }

}