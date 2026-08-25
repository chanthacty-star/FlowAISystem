using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface ILessonKnowledgeRepository
{

    // ==================================================
    // Teacher Lesson Management
    // ==================================================

    Task<List<LessonKnowledgeListItemDto>> GetAllAsync(
        LessonKnowledgeSearchDto search);



    Task<LessonKnowledgeDto?> GetByIdAsync(
        int id);



    Task CreateAsync(
        CreateLessonKnowledgeDto dto);



    Task UpdateAsync(
        UpdateLessonKnowledgeDto dto);



    Task DeleteAsync(
        int id);



    // ==================================================
    // Teacher Lessons
    // ==================================================

    Task<List<LessonKnowledgeListItemDto>> GetByTeacherAsync(
        int teacherId);



    // ==================================================
    // Student AI Search
    // ==================================================

    Task<List<LessonKnowledgeDto>> SearchAsync(
        IEnumerable<string> keywords);

    Task<List<LessonKnowledgeDto>> GetAllForTutorialAsync(
    LessonKnowledgeSearchDto search); // Tutorials lesson


    Task<LessonKnowledgeDto?> GetLessonForAIAsync(
        int lessonId);

}