using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Courses;

namespace FlowAISystem.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;


    public CourseService(
        ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }



    public async Task<List<CourseListItemDto>> GetAllAsync(
        CourseSearchDto search)
    {
        return await _courseRepository.GetAllAsync(search);
    }





    public async Task<CourseDto?> GetByIdAsync(
    int id)
    {
        var subject =
            await _courseRepository.GetByIdAsync(id);

        if (subject == null)
            return null;


        return new CourseDto
        {
            Id = subject.Id,

            Code = subject.Code,

            Name = subject.Name,

            Credits = subject.Credits,


            DepartmentId = subject.DepartmentId,

            DepartmentName =
                subject.Department?.Name
                ?? string.Empty
        };
    }





    public async Task CreateAsync(
        CreateCourseDto dto)
    {

        if (await _courseRepository
            .ExistsCodeAsync(dto.Code))
        {
            throw new Exception(
                "Course code already exists.");
        }



        var subject = new Subject
        {
            Code = dto.Code,

            Name = dto.Name,

            Credits = dto.Credits,


            DepartmentId = dto.DepartmentId
        };



        await _courseRepository
            .CreateAsync(subject);
    }







    public async Task UpdateAsync(
        UpdateCourseDto dto)
    {

        var subject =
            await _courseRepository
                .GetByIdAsync(dto.Id);



        if (subject == null)
        {
            throw new Exception(
                "Course not found.");
        }




        if (await _courseRepository
            .ExistsCodeAsync(dto.Code, dto.Id))
        {
            throw new Exception(
                "Course code already exists.");
        }



        subject.Code = dto.Code;

        subject.Name = dto.Name;

        subject.Credits = dto.Credits;


        subject.DepartmentId =
            dto.DepartmentId;



        await _courseRepository
            .UpdateAsync(subject);
    }







    public async Task DeleteAsync(
        int id)
    {

        var subject =
            await _courseRepository
                .GetByIdAsync(id);



        if (subject == null)
        {
            throw new Exception(
                "Course not found.");
        }



        await _courseRepository
            .DeleteAsync(subject);
    }
}