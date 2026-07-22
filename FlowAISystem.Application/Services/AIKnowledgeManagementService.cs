using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.DTOs.AI;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Application.DTOs.AI.Category;

namespace FlowAISystem.Application.Services;

public class AIKnowledgeManagementService
    : IAIKnowledgeManagementService
{

    private readonly IAIKnowledgeRepository _repository;
    private readonly IAICategoryRepository _categoryRepository;

    public AIKnowledgeManagementService(
        IAIKnowledgeRepository repository,
        IAICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }



    public async Task<List<AIKnowledgeDto>> GetAllAsync()
    {

        var items =
            await _repository.GetAllAsync();



        return items.Select(x => new AIKnowledgeDto
        {

            Id = x.Id,

            Question = x.Question,

            Keywords = x.Keywords,

            Answer = x.Answer,


            CategoryId = x.CategoryId,


            CategoryName =
                x.Category != null
                ? x.Category.Name
                : string.Empty,


            CreatedAt = x.CreatedAt


        }).ToList();

    }





    public async Task<AIKnowledgeDto?> GetByIdAsync(
        int id)
    {

        var item =
            await _repository.GetByIdAsync(id);



        if (item == null)
            return null;



        return new AIKnowledgeDto
        {

            Id = item.Id,

            Question = item.Question,

            Keywords = item.Keywords,

            Answer = item.Answer,


            CategoryId = item.CategoryId,


            CategoryName =
                item.Category != null
                ? item.Category.Name
                : string.Empty,


            CreatedAt = item.CreatedAt

        };

    }





    public async Task CreateAsync(
        CreateAIKnowledgeDto dto)
    {

        var entity = new AIKnowledge
        {

            Question = dto.Question,

            Keywords = dto.Keywords,

            Answer = dto.Answer,


            CategoryId = dto.CategoryId,


            CreatedAt = DateTime.UtcNow

        };



        await _repository.AddAsync(entity);

    }





    public async Task UpdateAsync(
        UpdateAIKnowledgeDto dto)
    {

        var entity =
            await _repository.GetByIdAsync(dto.Id);



        if (entity == null)
            return;



        entity.Question = dto.Question;

        entity.Keywords = dto.Keywords;

        entity.Answer = dto.Answer;


        entity.CategoryId = dto.CategoryId;



        await _repository.UpdateAsync(entity);

    }





    public async Task DeleteAsync(
        int id)
    {

        await _repository.DeleteAsync(id);

    }

    public async Task<List<AICategoryDto>> GetCategoriesAsync()
    {
        var categories =
            await _categoryRepository.GetAllAsync();


        return categories
            .Select(x => new AICategoryDto
            {
                Id = x.Id,

                Name = x.Name,

                Description = x.Description,

                CreatedAt = x.CreatedAt
            })
            .ToList();
    }

}

