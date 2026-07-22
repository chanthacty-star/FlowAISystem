using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class FeedbackRepository : IFeedbackRepository
{

    private readonly AppDbContext _context;



    public FeedbackRepository(
        AppDbContext context)
    {
        _context = context;
    }






    public async Task<List<FeedbackListItemDto>> GetAllAsync(
        FeedbackSearchDto search)
    {

        var query =
            _context.Feedbacks

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.Student)

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Subject)

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Teacher)

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Semester)

            .AsQueryable();






        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {

            query = query.Where(f =>

                f.Enrollment!.Student!
                    .Name.FirstName
                    .Contains(search.SearchTerm)

                ||

                f.Enrollment.Student!
                    .Name.LastName
                    .Contains(search.SearchTerm)

                ||

                f.Enrollment.CourseOffering!
                    .Subject!
                    .Name
                    .Contains(search.SearchTerm)

            );

        }






        if (search.Rating.HasValue)
        {

            query = query.Where(f =>
                f.Rating == search.Rating);

        }






        if (search.TeacherId.HasValue)
        {

            query = query.Where(f =>
                f.Enrollment!
                .CourseOffering!
                .TeacherId
                ==
                search.TeacherId);

        }






        if (search.FromDate.HasValue)
        {

            query = query.Where(f =>
                f.FeedbackDate >= search.FromDate);

        }






        if (search.ToDate.HasValue)
        {

            query = query.Where(f =>
                f.FeedbackDate <= search.ToDate);

        }






        return await query

            .OrderByDescending(f =>
                f.FeedbackDate)


            .Select(f => new FeedbackListItemDto
            {

                Id = f.Id,


                StudentName =
                    f.Enrollment!
                    .Student!
                    .Name
                    .ToString(),


                CourseName =
                    f.Enrollment!
                    .CourseOffering!
                    .Subject!
                    .Name,


                TeacherName =
                    f.Enrollment!
                    .CourseOffering!
                    .Teacher!
                    .TeacherName,


                Rating =
                    f.Rating,


                Comment =
                    f.Comment,


                FeedbackDate =
                    f.FeedbackDate


            })

            .ToListAsync();

    }








    public async Task<Feedback?> GetByIdAsync(
        int id)
    {

        return await _context.Feedbacks

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.Student)

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Subject)

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Teacher)

            .Include(f => f.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Semester)

            .FirstOrDefaultAsync(
                f => f.Id == id);

    }








    public async Task CreateAsync(
        Feedback feedback)
    {

        _context.Feedbacks.Add(feedback);

        await _context.SaveChangesAsync();

    }








    public async Task UpdateAsync(
        Feedback feedback)
    {

        _context.Feedbacks.Update(feedback);

        await _context.SaveChangesAsync();

    }








    public async Task DeleteAsync(
        Feedback feedback)
    {

        _context.Feedbacks.Remove(feedback);

        await _context.SaveChangesAsync();

    }








    public async Task<bool> ExistsAsync(
        int enrollmentId,
        int? ignoreId = null)
    {

        return await _context.Feedbacks.AnyAsync(f =>

            f.EnrollmentId == enrollmentId

            &&

            (!ignoreId.HasValue
            ||
            f.Id != ignoreId)

        );

    }


}