using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.CourseOfferings;

namespace FlowAISystem.Application.Services;

public class CourseOfferingService : ICourseOfferingService
{

    private readonly ICourseOfferingRepository _repository;


    public CourseOfferingService(
        ICourseOfferingRepository repository)
    {
        _repository = repository;
    }



    public async Task<List<CourseOfferingListItemDto>> GetAllAsync(
        CourseOfferingSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }





    public async Task<CourseOfferingDto?> GetByIdAsync(
        int id)
    {

        var offering =
            await _repository.GetByIdAsync(id);


        if (offering == null)
            return null;



        return new CourseOfferingDto
        {

            Id = offering.Id,


            ClassName = offering.ClassName,


            Capacity = offering.Capacity,


            Room = offering.Room,



            SubjectId = offering.SubjectId,

            SubjectName =
                offering.Subject?.Name
                ?? string.Empty,



            TeacherId = offering.TeacherId,

            TeacherName =
                offering.Teacher?.TeacherName
                ?? string.Empty,



            SemesterId = offering.SemesterId,

            SemesterName =
                offering.Semester?.Name
                ?? string.Empty

        };
    }






    public async Task CreateAsync(
        CreateCourseOfferingDto dto)
    {

        if (await _repository.ExistsAsync(
            dto.ClassName,
            dto.SubjectId,
            dto.SemesterId))
        {
            throw new Exception(
                "Course offering already exists.");
        }



        var offering = new CourseOffering
        {

            ClassName = dto.ClassName,


            Capacity = dto.Capacity,


            Room = dto.Room,


            SubjectId = dto.SubjectId,


            TeacherId = dto.TeacherId,


            SemesterId = dto.SemesterId

        };


        await _repository.CreateAsync(offering);

    }






    public async Task UpdateAsync(
        UpdateCourseOfferingDto dto)
    {

        var offering =
            await _repository.GetByIdAsync(dto.Id);



        if (offering == null)
            throw new Exception(
                "Course offering not found.");




        if (await _repository.ExistsAsync(
            dto.ClassName,
            dto.SubjectId,
            dto.SemesterId,
            dto.Id))
        {
            throw new Exception(
                "Course offering already exists.");
        }





        offering.ClassName =
            dto.ClassName;


        offering.Capacity =
            dto.Capacity;


        offering.Room =
            dto.Room;


        offering.SubjectId =
            dto.SubjectId;


        offering.TeacherId =
            dto.TeacherId;


        offering.SemesterId =
            dto.SemesterId;




        await _repository.UpdateAsync(offering);

    }






    public async Task DeleteAsync(
        int id)
    {

        var offering =
            await _repository.GetByIdAsync(id);



        if (offering == null)
            throw new Exception(
                "Course offering not found.");



        await _repository.DeleteAsync(offering);

    }

}