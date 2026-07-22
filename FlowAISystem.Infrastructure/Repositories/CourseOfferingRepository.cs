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

        var query =
            _context.CourseOfferings
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Include(c => c.Semester)
                .AsQueryable();



        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            query = query.Where(c =>
                c.ClassName.Contains(search.SearchTerm) ||
                c.Subject!.Name.Contains(search.SearchTerm));
        }



        if (search.SubjectId.HasValue)
        {
            query = query.Where(c =>
                c.SubjectId == search.SubjectId);
        }



        if (search.TeacherId.HasValue)
        {
            query = query.Where(c =>
                c.TeacherId == search.TeacherId);
        }



        if (search.SemesterId.HasValue)
        {
            query = query.Where(c =>
                c.SemesterId == search.SemesterId);
        }




        return await query
            .OrderBy(c => c.ClassName)
            .Select(c => new CourseOfferingListItemDto
            {

                Id = c.Id,

                ClassName = c.ClassName,


                SubjectName =
                    c.Subject!.Name,


                TeacherName =
                    c.Teacher!.TeacherName,


                SemesterName =
                    c.Semester!.Name

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