using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Scores;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class ScoreRepository : IScoreRepository
{

    private readonly AppDbContext _context;


    public ScoreRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<List<ScoreListItemDto>> GetAllAsync(
        ScoreSearchDto search)
    {

        var query =
            _context.Scores

            .Include(s => s.Enrollment!)
                .ThenInclude(e => e.Student)

            .Include(s => s.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Subject)

            .AsQueryable();



        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {

            query =
                query.Where(s =>

                    s.AssessmentName
                        .Contains(search.SearchTerm)

                    ||

                    s.Enrollment!
                    .Student!
                    .Name
                    .FirstName
                    .Contains(search.SearchTerm)

                    ||

                    s.Enrollment!
                    .CourseOffering!
                    .Subject!
                    .Name
                    .Contains(search.SearchTerm)

                );

        }



        if (search.EnrollmentId.HasValue)
        {

            query =
                query.Where(s =>
                    s.EnrollmentId ==
                    search.EnrollmentId);

        }



        return await query

            .OrderByDescending(s =>
                s.AssessmentDate)


            .Select(s =>
                new ScoreListItemDto
                {

                    Id =
                        s.Id,


                    StudentName =
                        s.Enrollment!
                        .Student!
                        .Name
                        .ToString(),


                    CourseName =
                        s.Enrollment!
                        .CourseOffering!
                        .Subject!
                        .Name,


                    AssessmentName =
                        s.AssessmentName,


                    Marks =
                        s.Marks,


                    MaxMarks =
                        s.MaxMarks,


                    AssessmentDate =
                        s.AssessmentDate

                })

            .ToListAsync();

    }





    public async Task<Score?> GetByIdAsync(
        int id)
    {

        return await _context.Scores

            .Include(s => s.Enrollment!)
                .ThenInclude(e => e.Student)

            .Include(s => s.Enrollment!)
                .ThenInclude(e => e.CourseOffering!)
                    .ThenInclude(c => c.Subject)

            .FirstOrDefaultAsync(
                s => s.Id == id);

    }





    public async Task CreateAsync(
        Score score)
    {

        _context.Scores.Add(score);


        await _context.SaveChangesAsync();

    }





    public async Task UpdateAsync(
        Score score)
    {

        _context.Scores.Update(score);


        await _context.SaveChangesAsync();

    }





    public async Task DeleteAsync(
        Score score)
    {

        _context.Scores.Remove(score);


        await _context.SaveChangesAsync();

    }





    public async Task<bool> ExistsAsync(
        int enrollmentId,
        string assessmentName,
        int? ignoreId = null)
    {

        return await _context.Scores.AnyAsync(s =>

            s.EnrollmentId == enrollmentId

            &&

            s.AssessmentName == assessmentName

            &&

            (!ignoreId.HasValue
                ||
             s.Id != ignoreId)

        );

    }

}