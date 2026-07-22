using FlowAISystem.Shared.DTOs.Teachers;
using FlowAISystem.Domain.Entities;

namespace FlowAISystem.Application.Interfaces.Services;


public interface ITeacherService
{

    Task<List<TeacherListItemDto>> GetAllAsync(
        TeacherSearchDto search);


    Task<TeacherDto?> GetByIdAsync(
        int id);


    Task CreateAsync(
        CreateTeacherDto dto);


    Task UpdateAsync(
        UpdateTeacherDto dto);


    Task DeleteAsync(
        int id);
    Task<TeacherProfileDto?> GetProfileAsync(int userId);

    Task UpdateProfileImageAsync(
    int userId,
    string imagePath);

    Task CreateProfileForUserAsync(
    int userId,
    string username,
    string email);
    //Task<TeacherProfileDto> GetTeacherProfileAsync(int userId);
    Task<List<Department>> GetDepartmentsAsync();



}
