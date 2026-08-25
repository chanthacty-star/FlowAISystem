using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;

public interface ISubjectExpertise
{
    string SubjectName { get; }

    bool CanHandle(LessonKnowledgeDto lesson);

    void Analyze(
        LessonKnowledgeDto lesson,
        SubjectAnalysisResult result);
}