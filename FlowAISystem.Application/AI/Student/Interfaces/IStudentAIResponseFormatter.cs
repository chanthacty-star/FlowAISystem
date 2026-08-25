using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Application.AI.Student.Interfaces;

public interface IStudentAIResponseFormatter
{
    string Format(
        string title,
        string content,
        LessonDifficulty difficulty,
        string language
    );
}