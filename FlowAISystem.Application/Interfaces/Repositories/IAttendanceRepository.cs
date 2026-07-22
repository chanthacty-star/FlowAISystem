using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Attendances;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IAttendanceRepository
{

    Task<List<AttendanceListItemDto>> GetAllAsync(
        AttendanceSearchDto search);



    Task<Attendance?> GetByIdAsync(
        int id);



    Task CreateAsync(
        Attendance attendance);



    Task UpdateAsync(
        Attendance attendance);



    Task DeleteAsync(
        Attendance attendance);



    Task<bool> ExistsAsync(
        int enrollmentId,
        DateOnly attendanceDate,
        int? ignoreId = null);

}