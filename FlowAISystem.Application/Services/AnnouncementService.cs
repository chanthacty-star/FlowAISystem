using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Announcements;

namespace FlowAISystem.Application.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _repository;

    public AnnouncementService(IAnnouncementRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AnnouncementDto>> GetActiveAnnouncementsAsync()
    {
        return await _repository.GetActiveAnnouncementsAsync();
    }

    public async Task<List<AnnouncementListItemDto>> GetAllAsync(AnnouncementSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }

    public async Task<AnnouncementDto?> GetByIdAsync(int id)
    {
        var announcement = await _repository.GetByIdAsync(id);

        if (announcement == null)
            return null;

        return new AnnouncementDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            PublishDate = announcement.PublishDate,
            ExpireDate = announcement.ExpireDate,
            Audience = announcement.Audience,
            CreatedByUserId = announcement.CreatedByUserId,
            CreatedByName = announcement.CreatedByUser?.Username ?? string.Empty
        };
    }

    public async Task CreateAsync(CreateAnnouncementDto dto)
    {
        var exists = await _repository.ExistsByTitleAsync(dto.Title);

        if (exists)
        {
            throw new InvalidOperationException($"An announcement with the title '{dto.Title.Trim()}' already exists.");
        }

        var announcement = new Announcement
        {
            Title = dto.Title.Trim(),
            Content = dto.Content,
            Audience = dto.Audience,
            PublishDate = EnsureUtc(dto.PublishDate),
            ExpireDate = dto.ExpireDate.HasValue ? EnsureUtc(dto.ExpireDate.Value) : null,
            CreatedByUserId = dto.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(announcement);
    }

    public async Task UpdateAsync(UpdateAnnouncementDto dto)
    {
        var announcement = await _repository.GetByIdAsync(dto.Id);

        if (announcement == null)
        {
            throw new KeyNotFoundException("Announcement not found.");
        }

        var exists = await _repository.ExistsByTitleAsync(dto.Title, dto.Id);
        if (exists)
        {
            throw new InvalidOperationException($"An announcement with the title '{dto.Title.Trim()}' already exists.");
        }

        announcement.Title = dto.Title.Trim();
        announcement.Content = dto.Content;
        announcement.Audience = dto.Audience;
        announcement.PublishDate = EnsureUtc(dto.PublishDate);
        announcement.ExpireDate = dto.ExpireDate.HasValue ? EnsureUtc(dto.ExpireDate.Value) : null;
        announcement.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(announcement);
    }

    public async Task DeleteAsync(int id)
    {
        var announcement = await _repository.GetByIdAsync(id);

        if (announcement == null)
        {
            throw new KeyNotFoundException("Announcement not found.");
        }

        await _repository.DeleteAsync(announcement);
    }

    private static DateTime EnsureUtc(DateTime date)
    {
        return date.Kind switch
        {
            DateTimeKind.Unspecified => DateTime.SpecifyKind(date, DateTimeKind.Utc),
            DateTimeKind.Local => date.ToUniversalTime(),
            _ => date
        };
    }
}