using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Announcements;

namespace FlowAISystem.Application.Services;

public class AnnouncementService : IAnnouncementService
{

    private readonly IAnnouncementRepository _repository;


    public AnnouncementService(
        IAnnouncementRepository repository)
    {
        _repository = repository;
    }





    public async Task<List<AnnouncementListItemDto>> GetAllAsync(
        AnnouncementSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }






    public async Task<AnnouncementDto?> GetByIdAsync(
        int id)
    {

        var announcement =
            await _repository.GetByIdAsync(id);



        if (announcement == null)
            return null;



        return new AnnouncementDto
        {
            Id = announcement.Id,

            Title = announcement.Title,

            Content = announcement.Content,

            PublishDate =
                announcement.PublishDate,

            ExpireDate =
                announcement.ExpireDate,

            Audience =
                announcement.Audience,


            CreatedByUserId =
                announcement.CreatedByUserId,


            CreatedByName =
                announcement.CreatedByUser?.Username
                ?? string.Empty
        };
    }








    public async Task CreateAsync(
        CreateAnnouncementDto dto)
    {

        var exists =
            await _repository.ExistsAsync(
                dto.Title);



        if (exists)
        {
            throw new Exception(
                "Announcement title already exists.");
        }





        var announcement = new Announcement
        {

            Title =
                dto.Title,


            Content =
                dto.Content,


            PublishDate =
                dto.PublishDate,


            ExpireDate =
                dto.ExpireDate,


            Audience =
                dto.Audience,


            CreatedByUserId =
                dto.CreatedByUserId

        };



        await _repository.CreateAsync(
            announcement);

    }








    public async Task UpdateAsync(
        UpdateAnnouncementDto dto)
    {

        var announcement =
            await _repository.GetByIdAsync(dto.Id);



        if (announcement == null)
        {
            throw new Exception(
                "Announcement not found.");
        }




        announcement.Title =
            dto.Title;


        announcement.Content =
            dto.Content;


        announcement.PublishDate =
            dto.PublishDate;


        announcement.ExpireDate =
            dto.ExpireDate;


        announcement.Audience =
            dto.Audience;



        await _repository.UpdateAsync(
            announcement);

    }








    public async Task DeleteAsync(
        int id)
    {

        var announcement =
            await _repository.GetByIdAsync(id);



        if (announcement == null)
        {
            throw new Exception(
                "Announcement not found.");
        }



        await _repository.DeleteAsync(
            announcement);

    }

}