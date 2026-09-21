using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Application.AI.Student.Interfaces;

public interface IStudentAIResponseFormatter
{
    // Existing — kept for compatibility, no longer used by LearnHandler.
    string Format(
        string title,
        string content,
        LessonDifficulty difficulty,
        string language);

    // New: shows Explanation + Summary only, with a hint if more sections exist.
    string FormatIntro(
        string title,
        string content,
        LessonDifficulty difficulty,
        string language,
        bool hasMoreSections);

    // New: which optional sections actually exist in this lesson's content.
    List<string> GetAvailableSectionKeys(
        string content,
        LessonDifficulty difficulty);

    // New: formats one specific section (by key) as a standalone chunk.
    string FormatSection(
        string sectionKey,
        string content,
        string language);
}