using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Announcements;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly AppDbContext _context;

    public AnnouncementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AnnouncementDto>> GetActiveAnnouncementsAsync()
    {
        var now = DateTime.UtcNow;

        return await _context.Announcements
            .AsNoTracking()
            .Where(a =>
                a.PublishDate <= now &&
                (a.ExpireDate == null || a.ExpireDate >= now))
            .OrderByDescending(a => a.PublishDate)
            .Select(a => new AnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                PublishDate = a.PublishDate,
                ExpireDate = a.ExpireDate,
                Audience = a.Audience,
                CreatedByUserId = a.CreatedByUserId,
                CreatedByName = a.CreatedByUser != null
                    ? a.CreatedByUser.Username
                    : string.Empty
            })
            .ToListAsync();
    }

    public async Task<List<AnnouncementListItemDto>> GetAllAsync(
        AnnouncementSearchDto search)
    {
        var query = _context.Announcements
            .AsNoTracking()
            .AsQueryable();

        // 1. Text Search Filter (Case-insensitive)
        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            var term = search.SearchTerm.Trim().ToLower();
            query = query.Where(a =>
                a.Title.ToLower().Contains(term) ||
                a.Content.ToLower().Contains(term));
        }

        // 2. Audience Filter
        if (search.Audience.HasValue)
        {
            query = query.Where(a => a.Audience == search.Audience.Value);
        }

        // 3. Date Filters
        if (search.FromDate.HasValue)
        {
            query = query.Where(a => a.PublishDate >= search.FromDate.Value);
        }

        if (search.ToDate.HasValue)
        {
            query = query.Where(a => a.PublishDate <= search.ToDate.Value);
        }

        // 4. Projection
        return await query
            .OrderByDescending(a => a.PublishDate)
            .Select(a => new AnnouncementListItemDto
            {
                Id = a.Id,
                Title = a.Title,
                Audience = a.Audience,
                CreatedByName = a.CreatedByUser != null
                    ? a.CreatedByUser.Username
                    : "(Unknown)",
                PublishDate = a.PublishDate,
                ExpireDate = a.ExpireDate
            })
            .ToListAsync();
    }

    public async Task<Announcement?> GetByIdAsync(int id)
    {
        return await _context.Announcements
            .Include(a => a.CreatedByUser)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task CreateAsync(Announcement announcement)
    {
        _context.Announcements.Add(announcement);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Announcement announcement)
    {
        _context.Announcements.Update(announcement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Announcement announcement)
    {
        _context.Announcements.Remove(announcement);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByTitleAsync(string title, int? ignoreId = null)
    {
        var normalizedTitle = title.Trim().ToLower();

        return await _context.Announcements
            .AnyAsync(a =>
                a.Title.ToLower() == normalizedTitle &&
                (!ignoreId.HasValue || a.Id != ignoreId.Value));
    }
}