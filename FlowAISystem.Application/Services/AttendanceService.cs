using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Attendances;

namespace FlowAISystem.Application.Services;

public class AttendanceService : IAttendanceService
{

    private readonly IAttendanceRepository _repository;


    public AttendanceService(
        IAttendanceRepository repository)
    {
        _repository = repository;
    }



    public async Task<List<AttendanceListItemDto>> GetAllAsync(
        AttendanceSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }



    public async Task<AttendanceDto?> GetByIdAsync(
        int id)
    {

        var attendance =
            await _repository.GetByIdAsync(id);


        if (attendance == null)
            return null;



        return new AttendanceDto
        {
            Id = attendance.Id,

            AttendanceDate =
                attendance.AttendanceDate,

            Status =
                attendance.Status,

            Remark =
                attendance.Remark,

            EnrollmentId =
                attendance.EnrollmentId,


            StudentName =
                attendance.Enrollment!
                .Student!
                .Name
                .ToString(),


            CourseName =
                attendance.Enrollment!
                .CourseOffering!
                .Subject!
                .Name,


            ClassName =
                attendance.Enrollment!
                .CourseOffering!
                .ClassName
        };

    }



    public async Task CreateAsync(
        CreateAttendanceDto dto)
    {

        var exists =
            await _repository.ExistsAsync(
                dto.EnrollmentId,
                dto.AttendanceDate);


        if (exists)
            throw new Exception(
                "Attendance already exists.");



        var attendance = new Attendance
        {
            EnrollmentId =
                dto.EnrollmentId,

            AttendanceDate =
                dto.AttendanceDate,

            Status =
                dto.Status,

            Remark =
                dto.Remark
        };


        await _repository.CreateAsync(attendance);

    }



    public async Task UpdateAsync(
        UpdateAttendanceDto dto)
    {

        var attendance =
            await _repository.GetByIdAsync(dto.Id);



        if (attendance == null)
            throw new Exception(
                "Attendance not found.");



        attendance.EnrollmentId =
            dto.EnrollmentId;


        attendance.AttendanceDate =
            dto.AttendanceDate;


        attendance.Status =
            dto.Status;


        attendance.Remark =
            dto.Remark;



        await _repository.UpdateAsync(attendance);

    }



    public async Task DeleteAsync(
        int id)
    {

        var attendance =
            await _repository.GetByIdAsync(id);



        if (attendance == null)
            throw new Exception(
                "Attendance not found.");



        await _repository.DeleteAsync(attendance);

    }

}