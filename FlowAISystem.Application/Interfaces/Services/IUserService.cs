using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Users;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IUserService
{
    Task<List<UserListItemDto>> GetAllAsync(
        UserSearchDto search);


    Task<UserDto?> GetByIdAsync(
        int id);


    Task CreateAsync(
        CreateUserDto dto);


    Task UpdateAsync(
        UpdateUserDto dto);


    Task DeleteAsync(
        int id);


    Task ActivateAsync(
        int id);


    Task<List<Role>> GetRolesAsync();


    // ===============================
    // Change Password
    // ===============================
    Task<bool> ChangePasswordAsync(
        ChangePasswordDto dto);
    // Reset mean forgot pp and want return it back
    Task<bool> ResetPasswordAsync(
    ResetPasswordDto dto);
}