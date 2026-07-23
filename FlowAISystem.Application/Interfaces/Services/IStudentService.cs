//using FlowAISystem.Domain.Entities;
//using FlowAISystem.Shared.DTOs.Students;

//namespace FlowAISystem.Application.Interfaces.Services;

//public interface IStudentService
//{

//    Task<List<StudentListItemDto>> GetAllAsync(
//        StudentSearchDto search);


//    Task<StudentDetailsDto?> GetDetailsAsync(
//        int id);


//    Task<StudentDto?> GetByIdAsync(
//        int id);


//    Task CreateAsync(
//        CreateStudentDto student);


//    Task UpdateAsync(
//        UpdateStudentDto student);


//    Task DeleteAsync(
//        int id);



//    // Auto create profile
//    Task CreateProfileForUserAsync(
//        int userId,
//        string username,
//        string email);



//    // Student Profile

//    Task<StudentProfileDto?> GetProfileAsync(
//        int userId);



//    // Student Dashboard

//    Task<StudentDashboardDto?> GetDashboardAsync(
//        int userId);

//    // Upadate profile
//    Task UpdateProfileImageAsync(
//    int userId,
//    string imagePath);


//    Task<List<Department>> GetDepartmentsAsync();

//}

using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Students;
using FlowAISystem.Shared.DTOs.Announcements;
namespace FlowAISystem.Application.Interfaces.Services;

public interface IStudentService
{

    // ==================================================
    // Student Management
    // ==================================================

    Task<List<StudentListItemDto>> GetAllAsync(
        StudentSearchDto search);


    Task<StudentDetailsDto?> GetDetailsAsync(
        int id);


    Task<StudentDto?> GetByIdAsync(
        int id);



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



    // ==================================================
    // Delete
    // ==================================================

    Task DeleteAsync(
        int id);



    // ==================================================
    // Auto Create Profile
    // ==================================================

    Task CreateProfileForUserAsync(
        int userId,
        string username,
        string email);



    // ==================================================
    // Student Profile
    // ==================================================

    Task<StudentProfileDto?> GetProfileAsync(
        int userId);



    Task UpdateProfileImageAsync(
        int userId,
        string imagePath);



    // ==================================================
    // Student Dashboard
    // ==================================================

    Task<StudentDashboardDto?> GetDashboardAsync(
        int userId);

    // subject
    Task<List<StudentSubjectDto>> GetSubjectsAsync(
    int userId);

    // ==================================================
    // Recent Subjects
    // ==================================================

    Task<List<StudentSubjectSummaryDto>> GetRecentSubjectsAsync(
        int userId);
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
    // Recent Announcements
    // ==================================================

    Task<List<StudentAnnouncementSummaryDto>> GetRecentAnnouncementsAsync(
        int userId,
        int count = 5);
    // ==================================================
    // Student Announcements Page
    // ==================================================

    //Task<List<AnnouncementDto>> GetAnnouncementsAsync(
    //    int userId);


    // ==================================================
    // Departments
    // ==================================================

    Task<List<Department>> GetDepartmentsAsync();

}