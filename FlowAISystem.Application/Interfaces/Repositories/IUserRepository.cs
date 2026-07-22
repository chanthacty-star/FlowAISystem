using FlowAISystem.Domain.Entities;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IUserRepository
{
    // Existing Authentication
    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByEmailAsync(string email);

    Task AddAsync(User user);

    Task<bool> ExistsByUsernameAsync(string username);

    Task<bool> ExistsByEmailAsync(string email);

    Task SaveChangesAsync();

    // User Management

    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task UpdateAsync(User user);

    Task DeleteAsync(int id);

    Task<List<Role>> GetRolesAsync();
}