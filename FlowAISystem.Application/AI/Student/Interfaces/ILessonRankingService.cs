using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Student.Interfaces;


/// <summary>
/// Responsible for selecting the most relevant lesson
/// based on student's question.
/// </summary>
public interface ILessonRankingService
{

    /// <summary>
    /// Ranks available lessons and returns
    /// the best matching lesson.
    /// </summary>
    /// <param// name="question">
    /// Student question.
    /// </param>
    /// <param //name = "lessons" >
    /// Lessons returned from knowledge search.
    /// </param>
    /// <returns>
    /// Best matching lesson or null.
    /// </returns>
    LessonKnowledgeDto ? Rank(
        string question,
        List<LessonKnowledgeDto> lessons);

}