using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Students;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IStudentRepository
{
    // ==================================================
    // Student List
    // ==================================================

    Task<List<StudentListItemDto>> GetAllAsync(
        StudentSearchDto search);

    // ==================================================
    // Student Details
    // ==================================================

    Task<StudentDetailsDto?> GetDetailsAsync(
        int id);

    // ==================================================
    // Student Edit
    // ==================================================

    Task<StudentDto?> GetByIdAsync(
        int id);

    Task AddAsync(Student student);
    // ==================================================
    // Create
    // ==================================================

    Task CreateAsync(
        CreateStudentDto student);

    // ==================================================
    // Update
    // ==================================================

    Task UpdateAsync(
        UpdateStudentDto student);

    Task UpdateEntityAsync(
        Student student);


    // ==================================================
    // Delete
    // ==================================================

    Task DeleteAsync(
        int id);

    // user
    //Task<Student?> GetByUserIdAsync(int userId);
    Task<Student?> GetByUserIdAsync(int userId);

    // Updat photo
    Task UpdateProfileImageAsync(
    int userId,
    string imagePath);


    // ==================================================
    // Departments
    // ==================================================

    Task<List<Department>> GetDepartmentsAsync();

    // ==================================================
    // Reports
    // ==================================================

    Task<List<StudentListItemDto>> GetStudentReportAsync();

    // ==================================================
    // AI
    // ==================================================

    Task<int> GetCountAsync();

    Task<List<StudentListItemDto>> GetRecentStudentsAsync(
        int count = 5);

    Task<StudentListItemDto?> FindByStudentNumberAsync(
        string studentNumber);

    Task<StudentListItemDto?> FindByNameAsync(
        string name);
}