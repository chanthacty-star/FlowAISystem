using BCrypt.Net;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Users;

namespace FlowAISystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ITeacherService _teacherService;
    private readonly IStudentService _studentService;

    public UserService(
        
        IUserRepository repository, 
        
        ITeacherService teacherService,
        IStudentService studentService )
    {
        _repository = repository;
        _teacherService = teacherService;   
        _studentService = studentService;
    }

    public async Task<List<UserListItemDto>> GetAllAsync(UserSearchDto search)
    {
        var users = await _repository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search.SearchText))
        {
            users = users
                .Where(x =>
                    x.Username.Contains(search.SearchText,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    x.Email.Contains(search.SearchText,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (search.RoleId.HasValue)
        {
            users = users
                .Where(x => x.RoleId == search.RoleId.Value)
                .ToList();
        }

        if (search.IsActive.HasValue)
        {
            users = users
                .Where(x => x.IsActive == search.IsActive.Value)
                .ToList();
        }

        return users
            .Select(x => new UserListItemDto
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                RoleName = x.Role?.Name ?? "",
                IsActive = x.IsActive
            })
            .ToList();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name ?? "",
            IsActive = user.IsActive
        };
    }

    public async Task CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = dto.RoleId,
            IsActive = dto.IsActive
        };


        await _repository.AddAsync(user);

        await _repository.SaveChangesAsync();



        // Get created user with Role information
        var createdUser =
            await _repository.GetByUsernameAsync(dto.Username);



        if (createdUser == null)
        {
            throw new Exception(
                "User was created but could not be retrieved.");
        }



        // Auto create Teacher profile
        if (createdUser.Role?.Name == "Teacher")
        {
            await _teacherService.CreateProfileForUserAsync(
                createdUser.Id,
                createdUser.Username,
                createdUser.Email);
        }



        // Auto create Student profile
        if (createdUser.Role?.Name == "Student")
        {
            await _studentService.CreateProfileForUserAsync(
                createdUser.Id,
                createdUser.Username,
                createdUser.Email);
        }
    }

    public async Task UpdateAsync(UpdateUserDto dto)
    {
        var user = await _repository.GetByIdAsync(dto.Id);

        if (user == null)
            throw new Exception("User not found.");

        user.Username = dto.Username;
        user.Email = dto.Email;
        user.RoleId = dto.RoleId;
        user.IsActive = dto.IsActive;

        await _repository.UpdateAsync(user);
        await _repository.SaveChangesAsync();
    }

    //public async Task DeleteAsync(int id)
    //{
    //    await _repository.DeleteAsync(id);
    //    await _repository.SaveChangesAsync();
    //}

    public async Task DeleteAsync(int id)
    {
        var user =
            await _repository.GetByIdAsync(id);


        if (user == null)
            throw new Exception("User not found");


        user.IsActive = false;


        await _repository.UpdateAsync(user);

        await _repository.SaveChangesAsync();
    }
    // emplemen user activate 
    public async Task ActivateAsync(int id)
    {
        var user =
            await _repository.GetByIdAsync(id);


        if (user == null)
            throw new Exception("User not found");


        user.IsActive = true;


        await _repository.UpdateAsync(user);

        await _repository.SaveChangesAsync();
    }
    public async Task<List<Role>> GetRolesAsync()
    {
        return await _repository.GetRolesAsync();
    }
    // Change password
    public async Task<bool> ChangePasswordAsync(
        ChangePasswordDto dto)
    {
        var user =
            await _repository.GetByIdAsync(
                dto.UserId);


        if (user == null)
            return false;



        // Verify current password

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                dto.CurrentPassword,
                user.PasswordHash);



        if (!valid)
            return false;



        // Confirm new password

        if (dto.NewPassword != dto.ConfirmPassword)
            return false;



        // Generate new BCrypt hash

        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                dto.NewPassword);



        user.UpdatedAt =
            DateTime.UtcNow;



        await _repository.UpdateAsync(user);


        // IMPORTANT: Save database

        await _repository.SaveChangesAsync();



        return true;
    }
    // reset pp
    public async Task<bool> ResetPasswordAsync(
    ResetPasswordDto dto)
    {
        var user =
            await _repository.GetByIdAsync(dto.UserId);

        if (user == null)
            return false;

        if (dto.NewPassword != dto.ConfirmPassword)
            return false;

        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                dto.NewPassword);

        user.UpdatedAt =
            DateTime.UtcNow;

        await _repository.UpdateAsync(user);

        return true;
    }
}