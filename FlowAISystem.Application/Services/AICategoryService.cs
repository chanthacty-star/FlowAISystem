using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.DTOs.AI.Category;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;

namespace FlowAISystem.Application.Services;

public class AICategoryService
    : IAICategoryService
{

    private readonly IAICategoryRepository _repository;


    public AICategoryService(
        IAICategoryRepository repository)
    {
        _repository = repository;
    }



    public async Task<List<AICategoryDto>> GetAllAsync()
    {

        var categories =
            await _repository.GetAllAsync();


        return categories.Select(x => new AICategoryDto
        {
            Id = x.Id,

            Name = x.Name,

            Description = x.Description,

            CreatedAt = x.CreatedAt

        }).ToList();

    }




    public async Task<AICategoryDto?> GetByIdAsync(
        int id)
    {

        var category =
            await _repository.GetByIdAsync(id);


        if (category == null)
            return null;


        return new AICategoryDto
        {
            Id = category.Id,

            Name = category.Name,

            Description = category.Description,

            CreatedAt = category.CreatedAt
        };

    }




    public async Task CreateAsync(
        CreateAICategoryDto dto)
    {

        var category = new AICategory
        {
            Name = dto.Name,

            Description = dto.Description,

            CreatedAt = DateTime.UtcNow
        };


        await _repository.AddAsync(category);

    }




    public async Task UpdateAsync(
        UpdateAICategoryDto dto)
    {

        var category =
            await _repository.GetByIdAsync(dto.Id);


        if (category == null)
            return;


        category.Name = dto.Name;

        category.Description = dto.Description;


        await _repository.UpdateAsync(category);

    }




    public async Task DeleteAsync(
        int id)
    {

        await _repository.DeleteAsync(id);

    }

}

