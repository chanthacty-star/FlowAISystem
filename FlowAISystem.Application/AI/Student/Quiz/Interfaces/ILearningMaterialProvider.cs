namespace FlowAISystem.Application.AI.Student.Quiz.Interfaces;

public interface ILearningMaterialProvider
{
    Task<string?> GetMaterialAsync(
        int studentId,
        int? lessonId);
}