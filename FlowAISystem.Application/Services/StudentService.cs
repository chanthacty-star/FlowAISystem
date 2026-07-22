using FlowAISystem.Domain.ValueObjects;
using FlowAISystem.Shared.Enums;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.Interfaces.Repositories;

using FlowAISystem.Domain.Entities;

using FlowAISystem.Shared.DTOs.Students;

namespace FlowAISystem.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(
        IStudentRepository repository)
    {
        _repository = repository;
    }

    // ==================================================
    // Student List
    // ==================================================

    public async Task<List<StudentListItemDto>> GetAllAsync(
        StudentSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }

    // ==================================================
    // Student Details
    // ==================================================

    public async Task<StudentDetailsDto?> GetDetailsAsync(
        int id)
    {
        return await _repository.GetDetailsAsync(id);
    }

    // ==================================================
    // Student Edit
    // ==================================================

    public async Task<StudentDto?> GetByIdAsync(
        int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    // ==================================================
    // Create
    // ==================================================

    public async Task CreateAsync(
        CreateStudentDto student)
    {
        await _repository.CreateAsync(student);
    }

    // ==================================================
    // Update
    // ==================================================

    public async Task UpdateAsync(
        UpdateStudentDto student)
    {
        await _repository.UpdateAsync(student);
    }

    // ==================================================
    // Delete
    // ==================================================

    public async Task DeleteAsync(
        int id)
    {
        await _repository.DeleteAsync(id);
    }

    // user 
    public async Task CreateProfileForUserAsync(
        int userId,
        string username,
        string email)
    {
        var departments =
            await _repository.GetDepartmentsAsync();


        if (!departments.Any())
        {
            throw new Exception(
                "No department exists.");
        }


        var student =
            new Student
            {
                UserId = userId,


                StudentNumber =
                    $"TEMP-{userId:D6}",


                Name =
                    new PersonName(
                        username,
                        ""),


                Email = email,


                PhoneNumber = "",


                Gender =
                    Gender.Male,


                DateOfBirth =
                    DateOnly.FromDateTime(
                        DateTime.Today),


                EnrollmentDate =
                    DateOnly.FromDateTime(
                        DateTime.Today),


                DepartmentId =
                    departments.First().Id
            };


        await _repository.AddAsync(student);
    }

    // ==================================================
    // Departments
    // ==================================================

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _repository.GetDepartmentsAsync();
    }

    // Student Dashboard
    public async Task<StudentDashboardDto?> GetDashboardAsync(int userId)
    {
        var student = await _repository.GetByUserIdAsync(userId);

        if (student == null)
            return null;

        return new StudentDashboardDto
        {
            StudentNumber = student.StudentNumber,

            FirstName = student.Name.FirstName,

            LastName = student.Name.LastName,

            DepartmentName = student.Department?.Name ?? "",

            EnrollmentDate = student.EnrollmentDate,

            ProfileImage = student.ProfileImage,

            Username = student.User?.Username ?? "",

            AccountActive = student.User?.IsActive ?? false
        };
    }
    // ==================================================
    // Student Profile
    // ==================================================

    public async Task<StudentProfileDto?> GetProfileAsync(
        int userId)
    {
        var student =
            await _repository.GetByUserIdAsync(userId);



        if (student == null)
            return null;



        return new StudentProfileDto
        {

            Id = student.Id,


            StudentNumber =
                student.StudentNumber,


            FirstName =
                student.Name.FirstName,


            LastName =
                student.Name.LastName,


            Email =
                student.Email,


            PhoneNumber =
                student.PhoneNumber,


            Gender =
                student.Gender.ToString(),


            DateOfBirth =
                student.DateOfBirth,


            EnrollmentDate =
                student.EnrollmentDate,


            DepartmentName =
                student.Department?.Name ?? "",



            UserId =
                student.UserId,


            Username =
                student.User?.Username ?? "",


            AccountActive =
                student.User?.IsActive ?? false,


            ProfileImage =
                student.ProfileImage
        };
    }
    // Update profile

    public async Task UpdateProfileImageAsync(
        int userId,
        string imagePath)
    {
        var student =
            await _repository.GetByUserIdAsync(userId);


        if (student == null)
            throw new Exception(
                "Student not found");


        student.ProfileImage =
            imagePath;


        student.UpdatedAt =
            DateTime.UtcNow;


        await _repository.UpdateEntityAsync(student);
    }
}

