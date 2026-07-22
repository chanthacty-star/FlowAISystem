using FlowAISystem.Shared.DTOs.Announcements;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IAnnouncementService
{

    Task<List<AnnouncementListItemDto>> GetAllAsync(
        AnnouncementSearchDto search);



    Task<AnnouncementDto?> GetByIdAsync(
        int id);



    Task CreateAsync(
        CreateAnnouncementDto dto);



    Task UpdateAsync(
        UpdateAnnouncementDto dto);



    Task DeleteAsync(
        int id);

}