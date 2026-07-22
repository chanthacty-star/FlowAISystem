using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Students;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IStudentService
{

    Task<List<StudentListItemDto>> GetAllAsync(
        StudentSearchDto search);


    Task<StudentDetailsDto?> GetDetailsAsync(
        int id);


    Task<StudentDto?> GetByIdAsync(
        int id);


    Task CreateAsync(
        CreateStudentDto student);


    Task UpdateAsync(
        UpdateStudentDto student);


    Task DeleteAsync(
        int id);



    // Auto create profile
    Task CreateProfileForUserAsync(
        int userId,
        string username,
        string email);



    // Student Profile

    Task<StudentProfileDto?> GetProfileAsync(
        int userId);



    // Student Dashboard

    Task<StudentDashboardDto?> GetDashboardAsync(
        int userId);

    // Upadate profile
    Task UpdateProfileImageAsync(
    int userId,
    string imagePath);


    Task<List<Department>> GetDepartmentsAsync();

}