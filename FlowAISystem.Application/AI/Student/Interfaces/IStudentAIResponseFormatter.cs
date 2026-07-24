namespace FlowAISystem.Application.AI.Student.Interfaces;

public interface IStudentAIResponseFormatter
{
    string Format(
        string title,
        string content);
}