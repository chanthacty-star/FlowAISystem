using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Students;
using FlowAISystem.Shared.DTOs.Announcements;
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

    //// user
    ////Task<Student?> GetByUserIdAsync(int userId);
    //Task<Student?> GetByUserIdAsync(int userId);

    // addd new 
    // ==================================================
    // Student Dashboard
    // ==================================================

    Task<Student?> GetByUserIdAsync(int userId);

    Task<List<StudentSubjectSummaryDto>> GetRecentSubjectsAsync(
        int userId);
    // ==================================================
    // Student Announcements Page
    // ==================================================
    //Task<List<AnnouncementDto>> GetAnnouncementsAsync(
    //    int userId);

    Task<List<StudentAnnouncementSummaryDto>> GetRecentAnnouncementsAsync(
        int userId,
        int count = 5);
    // ==================================================
    // Student Grades
    // ==================================================

    Task<List<StudentGradeDto>> GetGradesAsync(
        int userId);

    // ==================================================
    // Student Attendance
    // ==================================================

    Task<List<StudentAttendanceDto>> GetAttendanceAsync(
        int userId);
    // ==================================================
    // Student Feedback
    // ==================================================

    Task<List<StudentFeedbackDto>> GetFeedbackAsync(
        int userId);

    // ==================================================
    // Update Profile Image
    // ==================================================

    Task UpdateProfileImageAsync(
    int userId,
    string imagePath);

    Task<List<StudentSubjectDto>> GetSubjectsAsync(
    int userId);
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