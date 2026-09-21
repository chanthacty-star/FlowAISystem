using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Attendances;

namespace FlowAISystem.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _repository;

    public AttendanceService(IAttendanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AttendanceListItemDto>> GetAllAsync(AttendanceSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }

    public async Task<AttendanceDto?> GetByIdAsync(int id)
    {
        var attendance = await _repository.GetByIdAsync(id);

        if (attendance == null)
            return null;

        var student = attendance.Enrollment?.Student;
        var courseOffering = attendance.Enrollment?.CourseOffering;

        return new AttendanceDto
        {
            Id = attendance.Id,
            AttendanceDate = attendance.AttendanceDate,
            Status = attendance.Status,
            Remark = attendance.Remark,
            EnrollmentId = attendance.EnrollmentId,

            // Fixed: Accessing properties through student.Name
            StudentName = student != null && student.Name != null
                ? $"{student.Name.FirstName} {student.Name.LastName}".Trim()
                : "Unknown Student",

            CourseName = courseOffering?.Subject?.Name ?? "Unknown Subject",
            ClassName = courseOffering?.ClassName ?? "Unknown Class"
        };
    }

    public async Task CreateAsync(CreateAttendanceDto dto)
    {
        var exists = await _repository.ExistsAsync(dto.EnrollmentId, dto.AttendanceDate);

        if (exists)
            throw new InvalidOperationException("Attendance record already exists for this date and enrollment.");

        var attendance = new Attendance
        {
            EnrollmentId = dto.EnrollmentId,
            AttendanceDate = dto.AttendanceDate,
            Status = dto.Status,
            Remark = dto.Remark
        };

        await _repository.CreateAsync(attendance);
    }

    public async Task UpdateAsync(UpdateAttendanceDto dto)
    {
        var attendance = await _repository.GetByIdAsync(dto.Id);

        if (attendance == null)
            throw new KeyNotFoundException("Attendance record not found.");

        attendance.EnrollmentId = dto.EnrollmentId;
        attendance.AttendanceDate = dto.AttendanceDate;
        attendance.Status = dto.Status;
        attendance.Remark = dto.Remark;

        await _repository.UpdateAsync(attendance);
    }

    public async Task DeleteAsync(int id)
    {
        var attendance = await _repository.GetByIdAsync(id);

        if (attendance == null)
            throw new KeyNotFoundException("Attendance record not found.");

        await _repository.DeleteAsync(attendance);
    }
}