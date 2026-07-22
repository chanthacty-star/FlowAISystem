using FlowAISystem.Application.Interfaces;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class AICategoryRepository
    : IAICategoryRepository
{

    private readonly AppDbContext _context;


    public AICategoryRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<List<AICategory>> GetAllAsync()
    {
        return await _context.AICategories
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }




    public async Task<AICategory?> GetByIdAsync(
        int id)
    {
        return await _context.AICategories
            .FirstOrDefaultAsync(x => x.Id == id);
    }




    public async Task AddAsync(
        AICategory category)
    {
        await _context.AICategories
            .AddAsync(category);

        await _context.SaveChangesAsync();
    }




    public async Task UpdateAsync(
        AICategory category)
    {
        _context.AICategories
            .Update(category);

        await _context.SaveChangesAsync();
    }




    public async Task DeleteAsync(
        int id)
    {

        var category =
            await GetByIdAsync(id);



        if (category != null)
        {
            _context.AICategories
                .Remove(category);


            await _context.SaveChangesAsync();
        }

    }

}
