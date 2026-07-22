using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Enrollments;

namespace FlowAISystem.Application.Services;

public class EnrollmentService : IEnrollmentService
{

    private readonly IEnrollmentRepository _repository;


    public EnrollmentService(
        IEnrollmentRepository repository)
    {
        _repository = repository;
    }



    public async Task<List<EnrollmentListItemDto>> GetAllAsync(
        EnrollmentSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }



    public async Task<EnrollmentDto?> GetByIdAsync(
        int id)
    {

        var enrollment =
            await _repository.GetByIdAsync(id);


        if (enrollment == null)
            return null;


        return new EnrollmentDto
        {
            Id = enrollment.Id,

            EnrollmentDate =
                enrollment.EnrollmentDate,

            Status =
                enrollment.Status,


            StudentId =
                enrollment.StudentId,

            StudentName =
                enrollment.Student?.Name.ToString()
                ?? string.Empty,


            CourseOfferingId =
                enrollment.CourseOfferingId,

            ClassName =
                enrollment.CourseOffering?.ClassName
                ?? string.Empty,


            CourseName =
                enrollment.CourseOffering?
                    .Subject?.Name
                ?? string.Empty,


            TeacherName =
                enrollment.CourseOffering?
                    .Teacher?.TeacherName
                ?? string.Empty,


            SemesterName =
                enrollment.CourseOffering?
                    .Semester?.Name
                ?? string.Empty
        };
    }



    public async Task CreateAsync(
        CreateEnrollmentDto dto)
    {

        if (await _repository.ExistsAsync(
            dto.StudentId,
            dto.CourseOfferingId))
        {
            throw new Exception(
                "Student already enrolled in this class.");
        }


        var enrollment = new Enrollment
        {
            StudentId =
                dto.StudentId,


            CourseOfferingId =
                dto.CourseOfferingId,


            EnrollmentDate =
                dto.EnrollmentDate,


            Status =
                dto.Status
        };


        await _repository.CreateAsync(
            enrollment);
    }



    public async Task UpdateAsync(
        UpdateEnrollmentDto dto)
    {

        var enrollment =
            await _repository.GetByIdAsync(dto.Id);


        if (enrollment == null)
            throw new Exception(
                "Enrollment not found.");



        enrollment.StudentId =
            dto.StudentId;


        enrollment.CourseOfferingId =
            dto.CourseOfferingId;


        enrollment.EnrollmentDate =
            dto.EnrollmentDate;


        enrollment.Status =
            dto.Status;



        await _repository.UpdateAsync(
            enrollment);
    }



    public async Task DeleteAsync(
        int id)
    {

        var enrollment =
            await _repository.GetByIdAsync(id);


        if (enrollment == null)
            throw new Exception(
                "Enrollment not found.");



        await _repository.DeleteAsync(
            enrollment);
    }
}