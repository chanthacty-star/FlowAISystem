using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Announcements;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{

    private readonly AppDbContext _context;



    public AnnouncementRepository(
        AppDbContext context)
    {
        _context = context;
    }







    public async Task<List<AnnouncementListItemDto>> GetAllAsync(
        AnnouncementSearchDto search)
    {

        var query =
            _context.Announcements
            .Include(a => a.CreatedByUser)
            .AsQueryable();





        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            query = query.Where(a =>
                a.Title.Contains(search.SearchTerm) ||
                a.Content.Contains(search.SearchTerm));
        }






        if (search.Audience.HasValue)
        {
            query = query.Where(a =>
                a.Audience == search.Audience);
        }






        if (search.FromDate.HasValue)
        {
            query = query.Where(a =>
                a.PublishDate >= search.FromDate);
        }






        if (search.ToDate.HasValue)
        {
            query = query.Where(a =>
                a.PublishDate <= search.ToDate);
        }






        return await query
            .OrderByDescending(a => a.PublishDate)
            .Select(a => new AnnouncementListItemDto
            {

                Id = a.Id,


                Title = a.Title,


                Audience = a.Audience,


                CreatedByName =
                    a.CreatedByUser!.Username,



                PublishDate =
                    a.PublishDate,



                ExpireDate =
                    a.ExpireDate

            })
            .ToListAsync();

    }








    public async Task<Announcement?> GetByIdAsync(
        int id)
    {

        return await _context.Announcements
            .Include(a => a.CreatedByUser)
            .FirstOrDefaultAsync(a =>
                a.Id == id);

    }








    public async Task CreateAsync(
        Announcement announcement)
    {

        _context.Announcements.Add(
            announcement);


        await _context.SaveChangesAsync();

    }








    public async Task UpdateAsync(
        Announcement announcement)
    {

        _context.Announcements.Update(
            announcement);


        await _context.SaveChangesAsync();

    }








    public async Task DeleteAsync(
        Announcement announcement)
    {

        _context.Announcements.Remove(
            announcement);


        await _context.SaveChangesAsync();

    }








    public async Task<bool> ExistsAsync(
        string title,
        int? ignoreId = null)
    {

        return await _context.Announcements
            .AnyAsync(a =>
                a.Title == title &&
                (!ignoreId.HasValue ||
                 a.Id != ignoreId));

    }

}