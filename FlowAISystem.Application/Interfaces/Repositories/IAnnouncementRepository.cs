namespace FlowAISystem.Application.Interfaces.Repositories;

using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Announcements;

public interface IAnnouncementRepository
{
    Task<List<AnnouncementListItemDto>> GetAllAsync(
        AnnouncementSearchDto search);

    Task<Announcement?> GetByIdAsync(
        int id);

    Task CreateAsync(
        Announcement announcement);

    Task UpdateAsync(
        Announcement announcement);

    Task DeleteAsync(
        Announcement announcement);

    // Renamed for explicit clarity
    Task<bool> ExistsByTitleAsync(
        string title,
        int? ignoreId = null);

    Task<List<AnnouncementDto>> GetActiveAnnouncementsAsync();
}