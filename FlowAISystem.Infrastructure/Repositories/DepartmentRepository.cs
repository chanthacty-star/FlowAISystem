using Microsoft.EntityFrameworkCore;

using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;

namespace FlowAISystem.Infrastructure.Repositories;

public class DepartmentRepository
    : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments

            .Include(x => x.Students)

            .Include(x => x.Teachers)

            .OrderBy(x => x.Name)

            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(
        int id)
    {
        return await _context.Departments

            .Include(x => x.Students)

            .Include(x => x.Teachers)

            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(
        Department department)
    {
        _context.Departments.Add(department);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        Department department)
    {
        _context.Departments.Update(department);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(
        int id)
    {
        var department =
            await _context.Departments.FindAsync(id);

        if (department == null)
            return;

        _context.Departments.Remove(department);

        await _context.SaveChangesAsync();
    }
}

