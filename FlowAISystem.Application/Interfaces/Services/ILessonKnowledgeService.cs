using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.Interfaces.Services;

public interface ILessonKnowledgeService
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
    // Student AI Knowledge Search
    // ==================================================

    Task<List<LessonKnowledgeDto>> SearchAsync(
        string keyword);



    // ==================================================
    // Student AI Context
    // ==================================================

    Task<LessonKnowledgeDto?> GetLessonForAIAsync(
        int lessonId);

}