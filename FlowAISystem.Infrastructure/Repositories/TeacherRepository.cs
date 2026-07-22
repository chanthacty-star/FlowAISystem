using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;


    public TeacherRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<List<Teacher>> GetAllAsync()
    {
        return await _context.Teachers

            .Include(t => t.Department)

            .Include(t => t.User)

            .Include(t => t.CourseOfferings)

            .ToListAsync();
    }



    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await _context.Teachers

            .Include(t => t.Department)

            .Include(t => t.User)

            .Include(t => t.CourseOfferings)

            .FirstOrDefaultAsync(
                t => t.Id == id);
    }




    public async Task AddAsync(
        Teacher teacher)
    {
        await _context.Teachers.AddAsync(
            teacher);

        await _context.SaveChangesAsync();
    }





    public async Task UpdateAsync(
        Teacher teacher)
    {
        _context.Teachers.Update(
            teacher);

        await _context.SaveChangesAsync();
    }





    public async Task DeleteAsync(
        int id)
    {
        var teacher =
            await _context.Teachers
                .FirstOrDefaultAsync(
                    t => t.Id == id);


        if (teacher == null)
            return;


        _context.Teachers.Remove(
            teacher);


        await _context.SaveChangesAsync();
    }





    public async Task<Teacher?> GetByUserIdAsync(
        int userId)
    {
        return await _context.Teachers

            .Include(t => t.Department)

            .Include(t => t.CourseOfferings)

            .FirstOrDefaultAsync(
                t => t.UserId == userId);
    }





    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _context.Departments

            .OrderBy(d => d.Name)

            .ToListAsync();
    }
}