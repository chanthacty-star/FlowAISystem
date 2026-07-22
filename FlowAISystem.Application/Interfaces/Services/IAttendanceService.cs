using FlowAISystem.Shared.DTOs.Attendances;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IAttendanceService
{

    Task<List<AttendanceListItemDto>> GetAllAsync(
        AttendanceSearchDto search);



    Task<AttendanceDto?> GetByIdAsync(
        int id);



    Task CreateAsync(
        CreateAttendanceDto dto);



    Task UpdateAsync(
        UpdateAttendanceDto dto);



    Task DeleteAsync(
        int id);

}