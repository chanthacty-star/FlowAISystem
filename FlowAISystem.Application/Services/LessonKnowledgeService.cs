using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.Services;

public class LessonKnowledgeService : ILessonKnowledgeService
{

    private readonly ILessonKnowledgeRepository _repository;


    public LessonKnowledgeService(
        ILessonKnowledgeRepository repository)
    {
        _repository = repository;
    }



    // ==================================================
    // Get All Lessons
    // ==================================================

    public async Task<List<LessonKnowledgeListItemDto>> GetAllAsync(
        LessonKnowledgeSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }



    // ==================================================
    // Get Lesson By Id
    // ==================================================

    public async Task<LessonKnowledgeDto?> GetByIdAsync(
        int id)
    {
        return await _repository.GetByIdAsync(id);
    }



    // ==================================================
    // Create Lesson
    // ==================================================

    public async Task CreateAsync(
        CreateLessonKnowledgeDto dto)
    {

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new Exception(
                "Lesson title is required.");
        }


        if (string.IsNullOrWhiteSpace(dto.Content))
        {
            throw new Exception(
                "Lesson content is required.");
        }


        await _repository.CreateAsync(dto);
    }



    // ==================================================
    // Update Lesson
    // ==================================================

    public async Task UpdateAsync(
        UpdateLessonKnowledgeDto dto)
    {

        if (dto.Id <= 0)
        {
            throw new Exception(
                "Invalid lesson id.");
        }


        await _repository.UpdateAsync(dto);
    }



    // ==================================================
    // Delete Lesson
    // ==================================================

    public async Task DeleteAsync(
        int id)
    {

        if (id <= 0)
        {
            throw new Exception(
                "Invalid lesson id.");
        }


        await _repository.DeleteAsync(id);
    }



    // ==================================================
    // Teacher Lessons
    // ==================================================

    public async Task<List<LessonKnowledgeListItemDto>>
        GetByTeacherAsync(
            int teacherId)
    {

        return await _repository
            .GetByTeacherAsync(teacherId);

    }



    // ==================================================
    // AI Search
    // ==================================================

    public async Task<List<LessonKnowledgeDto>>
        SearchAsync(
            string keyword)
    {

        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<LessonKnowledgeDto>();
        }


        return await _repository
            .SearchAsync(keyword);

    }



    // ==================================================
    // Student AI Lesson Retrieval
    // ==================================================

    public async Task<LessonKnowledgeDto?>
        GetLessonForAIAsync(
            int lessonId)
    {

        return await _repository
            .GetLessonForAIAsync(lessonId);

    }

}