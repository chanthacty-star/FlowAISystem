using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Announcements;

namespace FlowAISystem.Application.Interfaces.Repositories;

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



    Task<bool> ExistsAsync(
        string title,
        int? ignoreId = null);

}