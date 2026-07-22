using FlowAISystem.Domain.ValueObjects;
using FlowAISystem.Shared.Enums;

using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Teachers;

namespace FlowAISystem.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repository;


    public TeacherService(
        ITeacherRepository repository)
    {
        _repository = repository;
    }



    public async Task<List<TeacherListItemDto>> GetAllAsync(
    TeacherSearchDto search)
    {
        var teachers =
            await _repository.GetAllAsync();


        if (!string.IsNullOrWhiteSpace(search.SearchText))
        {
            teachers =
                teachers
                .Where(x =>
                    x.Name.FirstName.Contains(
                        search.SearchText,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    x.Name.LastName.Contains(
                        search.SearchText,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    x.Email.Contains(
                        search.SearchText,
                        StringComparison.OrdinalIgnoreCase)

                )
                .ToList();
        }



        if (search.DepartmentId.HasValue)
        {
            teachers =
                teachers
                .Where(x =>
                    x.DepartmentId ==
                    search.DepartmentId.Value)

                .ToList();
        }



        return teachers
            .Select(x => new TeacherListItemDto
            {
                Id = x.Id,


                TeacherName = x.TeacherName,


                FirstName =
                    x.Name.FirstName,


                LastName =
                    x.Name.LastName,


                Email =
                    x.Email,


                PhoneNumber =
                    x.PhoneNumber,


                DepartmentName =
                    x.Department != null
                    ? x.Department.Name
                    : string.Empty

            })
            .ToList();
    }


    public async Task<TeacherDto?> GetByIdAsync(
        int id)
    {
        var teacher =
            await _repository.GetByIdAsync(id);



        if (teacher == null)
            return null;



        return new TeacherDto
        {
            Id = teacher.Id,


            FirstName =
                teacher.Name.FirstName,


            LastName =
                teacher.Name.LastName,


            Email =
                teacher.Email,


            PhoneNumber =
                teacher.PhoneNumber,


            Gender =
                teacher.Gender,


            DateOfBirth =
                teacher.DateOfBirth,


            HireDate =
                teacher.HireDate,


            DepartmentId =
                teacher.DepartmentId,


            DepartmentName =
                teacher.Department != null
                ? teacher.Department.Name
                : string.Empty
        };
    }

    public async Task<TeacherProfileDto?> GetProfileAsync(int userId)
    {
        var teacher =
            await _repository.GetByUserIdAsync(userId);


        if (teacher == null)
            return null;



        return new TeacherProfileDto
        {
            Id = teacher.Id,


            TeacherName = teacher.TeacherName,


            FirstName = teacher.Name.FirstName,

            LastName = teacher.Name.LastName,


            Email = teacher.Email,


            PhoneNumber = teacher.PhoneNumber,


            Gender = teacher.Gender.ToString(),


            DateOfBirth = teacher.DateOfBirth,


            HireDate = teacher.HireDate,


            DepartmentName =
                teacher.Department?.Name ?? "",



            UserId = teacher.UserId,


            Username =
                teacher.User?.Username ?? "",


            AccountActive =
                teacher.User?.IsActive ?? false,



            ProfileImage =
                teacher.ProfileImage
        };
    }
    // create profile for users
    public async Task CreateProfileForUserAsync(
    int userId,
    string username,
    string email)
    {
        var teacher = new Teacher
        {
            UserId = userId,

            TeacherName = username,

            Name = new PersonName(
                username,
                ""),

            Email = email,

            PhoneNumber = "",

            Gender = Gender.Male,

            DateOfBirth = DateOnly.FromDateTime(
                DateTime.Today),

            HireDate = DateOnly.FromDateTime(
                DateTime.Today),

            DepartmentId = 1,

            ProfileImage = null
        };

        await _repository.AddAsync(teacher);
    }

    // logic upload img
    public async Task UpdateProfileImageAsync(
    int userId,
    string imagePath)
    {
        var teacher =
            await _repository
            .GetByUserIdAsync(userId);


        if (teacher == null)
            throw new Exception(
                "Teacher profile not found");


        teacher.ProfileImage =
            imagePath;


        await _repository.UpdateAsync(
            teacher);
    }

    public async Task CreateAsync(
        CreateTeacherDto dto)
    {
        var teacher = new Teacher
        {
            Name =
                new(
                    dto.FirstName,
                    dto.LastName),


            Email =
                dto.Email,


            PhoneNumber =
                dto.PhoneNumber,


            Gender =
                dto.Gender,


            DateOfBirth =
                dto.DateOfBirth,


            HireDate =
                dto.HireDate,


            DepartmentId =
                dto.DepartmentId
        };



        await _repository.AddAsync(teacher);
    }



    public async Task UpdateAsync(
        UpdateTeacherDto dto)
    {
        var teacher =
            await _repository.GetByIdAsync(dto.Id);



        if (teacher == null)
            throw new Exception("Teacher not found");



        teacher.Name =
            new(
                dto.FirstName,
                dto.LastName);



        teacher.Email =
            dto.Email;


        teacher.PhoneNumber =
            dto.PhoneNumber;


        teacher.Gender =
            dto.Gender;


        teacher.DateOfBirth =
            dto.DateOfBirth;


        teacher.HireDate =
            dto.HireDate;


        teacher.DepartmentId =
            dto.DepartmentId;



        await _repository.UpdateAsync(teacher);
    }







    public async Task DeleteAsync(
        int id)
    {
        await _repository.DeleteAsync(id);
    }







    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _repository.GetDepartmentsAsync();
    }
}