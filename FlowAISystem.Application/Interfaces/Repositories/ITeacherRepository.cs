using FlowAISystem.Domain.Entities;

namespace FlowAISystem.Application.Interfaces.Repositories;


public interface ITeacherRepository
{

    Task<List<Teacher>> GetAllAsync();


    Task<Teacher?> GetByIdAsync(
        int id);


    Task AddAsync(
        Teacher teacher);


    Task UpdateAsync(
        Teacher teacher);


    Task DeleteAsync(
        int id);

    Task<Teacher?> GetByUserIdAsync(int userId); // 
    Task<List<Department>> GetDepartmentsAsync();

}