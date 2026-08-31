using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement;

public class LessonEnhancementAgent : ILessonEnhancementAgent
{
    private readonly IEnumerable<ISubjectExpertise> _subjectExpertises;

    public LessonEnhancementAgent(
        IEnumerable<ISubjectExpertise> subjectExpertises)
    {
        _subjectExpertises = subjectExpertises;
    }

    // ==================================================
    // Analyze Lesson
    // ==================================================

    public Task<LessonAnalysisResult> AnalyzeAsync(
        LessonKnowledgeDto lesson)
    {
        if (lesson == null)
        {
            throw new ArgumentNullException(nameof(lesson));
        }

        Console.WriteLine(
            $"[Teacher AI Agent] Analysis started. " +
            $"LessonId={lesson.Id}, Title='{lesson.Title}'");

        var result = new LessonAnalysisResult();

        int score = 100;

        // ==================================================
        // 1. GENERAL LESSON ANALYSIS
        // ==================================================

        AnalyzeTitle(
            lesson,
            result,
            ref score);

        AnalyzeDescription(
            lesson,
            result,
            ref score);

        AnalyzeContent(
            lesson,
            result,
            ref score);

        AnalyzeKeywords(
            lesson,
            result,
            ref score);

        // ==================================================
        // 2. SUBJECT-SPECIFIC EXPERTISE
        // ==================================================

        AnalyzeSubjectExpertise(
            lesson,
            result,
            ref score);

        // ==================================================
        // 3. FINAL SCORE
        // ==================================================

        result.Score = Math.Clamp(
            score,
            0,
            100);

        Console.WriteLine(
            $"[Teacher AI Agent] Analysis completed. " +
            $"Score={result.Score}, " +
            $"Strengths={result.Strengths.Count}, " +
            $"MissingAreas={result.MissingAreas.Count}, " +
            $"Suggestions={result.Suggestions.Count}");

        return Task.FromResult(result);
    }

    // ==================================================
    // Subject Expertise
    // ==================================================

    private void AnalyzeSubjectExpertise(
        LessonKnowledgeDto lesson,
        LessonAnalysisResult result,
        ref int score)
    {
        foreach (var expertise in _subjectExpertises)
        {
            try
            {
                if (!expertise.CanHandle(lesson))
                {
                    continue;
                }

                Console.WriteLine(
                    $"[Teacher AI Agent] " +
                    $"Using expertise: {expertise.SubjectName}");

                var subjectResult =
                    new SubjectAnalysisResult
                    {
                        Subject = expertise.SubjectName
                    };

                expertise.Analyze(
                    lesson,
                    subjectResult);

                // ------------------------------------------
                // Strengths
                // ------------------------------------------

                foreach (var strength in subjectResult.Strengths)
                {
                    result.Strengths.Add(
                        $"[{expertise.SubjectName}] {strength}");
                }

                // ------------------------------------------
                // Suggestions
                // ------------------------------------------

                foreach (var suggestion in subjectResult.Suggestions)
                {
                    result.Suggestions.Add(
                        new LessonSuggestion
                        {
                            Area = expertise.SubjectName,
                            Message = suggestion,
                            Priority = "Medium"
                        });
                }

                // ------------------------------------------
                // Missing Areas
                // ------------------------------------------

                foreach (var missing in subjectResult.MissingAreas)
                {
                    result.MissingAreas.Add(
                        $"{expertise.SubjectName}: {missing}");
                }

                // ------------------------------------------
                // Score Adjustment
                // ------------------------------------------

                score += subjectResult.ScoreAdjustment;

                Console.WriteLine(
                    $"[Teacher AI Agent] " +
                    $"Expertise={expertise.SubjectName}, " +
                    $"ScoreAdjustment={subjectResult.ScoreAdjustment}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[Teacher AI Agent] " +
                    $"Expertise '{expertise.SubjectName}' failed: " +
                    $"{ex.Message}");
            }
        }
    }

    // ==================================================
    // Improve Lesson
    // ==================================================

    public Task<LessonImprovementResult> ImproveAsync(
        LessonKnowledgeDto lesson)
    {
        if (lesson == null)
        {
            throw new ArgumentNullException(nameof(lesson));
        }

        var suggestions =
            new List<LessonSuggestion>();

        var content =
            lesson.Content?.Trim()
            ?? string.Empty;

        if (string.IsNullOrWhiteSpace(content))
        {
            return Task.FromResult(
                new LessonImprovementResult
                {
                    ImprovedContent = content,
                    Suggestions =
                    {
                        new LessonSuggestion
                        {
                            Area = "Content",
                            Message =
                                "There is no lesson content to improve.",
                            Priority = "High"
                        }
                    }
                });
        }

        if (!ContainsExample(content))
        {
            suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Examples",
                    Message =
                        "Add a practical example related to the main concept.",
                    Priority = "Medium"
                });
        }

        if (!ContainsPractice(content))
        {
            suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Practice",
                    Message =
                        "Add practice exercises so students can apply the concept.",
                    Priority = "Medium"
                });
        }

        if (!ContainsSummary(content))
        {
            suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Summary",
                    Message =
                        "Add a short summary of the key concepts.",
                    Priority = "Low"
                });
        }

        var improvedContent = content;

        if (!ContainsSummary(content))
        {
            improvedContent += """

                            ## Summary

                            Review the main concepts from this lesson and make sure you understand the key ideas before moving to the next lesson.
                            """;
        }
        //Improve after analysis
        foreach (var expertise in _subjectExpertises)
        {
            try
            {
                if (!expertise.CanHandle(lesson))
                {
                    continue;
                }

                Console.WriteLine(
                    $"[Teacher AI Agent] Using improvement expertise: {expertise.SubjectName}");

                var subjectImprovement = new LessonImprovementResult();

                expertise.Improve(
                    lesson,
                    subjectImprovement);

                suggestions.AddRange(
                    subjectImprovement.Suggestions);

                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[Teacher AI Agent] Improvement expertise '{expertise.SubjectName}' failed: {ex.Message}");
            }
        }

        //Return result after imprve
        return Task.FromResult(
            new LessonImprovementResult
            {
                ImprovedContent = improvedContent,
                Suggestions = suggestions
            });
    }

    // ==================================================
    // Generate Examples
    // ==================================================

    public Task<string> GenerateExamplesAsync(
        LessonKnowledgeDto lesson)
    {
        if (lesson == null)
        {
            throw new ArgumentNullException(nameof(lesson));
        }

        var title =
            lesson.Title?.Trim()
            ?? "this lesson";

        var result = $"""
                ## Example

                Here is a simple example related to **{title}**.

                Review the example step by step and identify how the main concept from the lesson is being used.

                Try changing the example and observe what happens.
                """;

        return Task.FromResult(result);
    }

    // ==================================================
    // Generate Practice
    // ==================================================

    public Task<string> GeneratePracticeAsync(
        LessonKnowledgeDto lesson)
    {
        if (lesson == null)
        {
            throw new ArgumentNullException(nameof(lesson));
        }

        var title =
            lesson.Title?.Trim()
            ?? "this lesson";

        var result = $"""
## Practice

Try this practice activity based on **{title}**:

1. Review the main concept.
2. Create your own small example.
3. Explain what your example does.
4. Change one part of the example.
5. Describe what changed and why.
""";

        return Task.FromResult(result);
    }

    // ==================================================
    // Generate Summary
    // ==================================================

    public Task<string> GenerateSummaryAsync(
        LessonKnowledgeDto lesson)
    {
        if (lesson == null)
        {
            throw new ArgumentNullException(nameof(lesson));
        }

        var title =
            lesson.Title?.Trim()
            ?? "this lesson";

        var result = $"""
## Summary

In this lesson, you learned the main concepts of **{title}**.

Before continuing, make sure you can:

- Explain the main concept.
- Give a simple example.
- Apply the concept in a small exercise.
- Explain your answer in your own words.
""";

        return Task.FromResult(result);
    }

    // ==================================================
    // General Analysis
    // ==================================================

    private static void AnalyzeTitle(
        LessonKnowledgeDto lesson,
        LessonAnalysisResult result,
        ref int score)
    {
        if (string.IsNullOrWhiteSpace(lesson.Title))
        {
            score -= 15;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Title",
                    Message = "Add a clear lesson title.",
                    Priority = "High"
                });

            result.MissingAreas.Add("Title");
        }
        else
        {
            result.Strengths.Add(
                "Lesson has a title.");
        }
    }

    private static void AnalyzeDescription(
        LessonKnowledgeDto lesson,
        LessonAnalysisResult result,
        ref int score)
    {
        if (string.IsNullOrWhiteSpace(lesson.Description))
        {
            score -= 10;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Description",
                    Message =
                        "Add a short description explaining what this lesson teaches.",
                    Priority = "Medium"
                });

            result.MissingAreas.Add(
                "Description");
        }
        else
        {
            result.Strengths.Add(
                "Lesson has a description.");
        }
    }

    private static void AnalyzeContent(
        LessonKnowledgeDto lesson,
        LessonAnalysisResult result,
        ref int score)
    {
        if (string.IsNullOrWhiteSpace(lesson.Content))
        {
            score -= 30;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Content",
                    Message =
                        "Lesson content is required.",
                    Priority = "High"
                });

            result.MissingAreas.Add(
                "Content");

            return;
        }

        result.Strengths.Add(
            "Lesson contains learning content.");

        if (lesson.Content.Length < 300)
        {
            score -= 10;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Content Depth",
                    Message =
                        "The lesson content is quite short. Consider adding explanations and examples.",
                    Priority = "Medium"
                });

            result.MissingAreas.Add(
                "Detailed Explanation");
        }

        if (!ContainsExample(lesson.Content))
        {
            score -= 10;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Examples",
                    Message =
                        "Consider adding at least one practical example.",
                    Priority = "Medium"
                });

            result.MissingAreas.Add(
                "Examples");
        }
        else
        {
            result.Strengths.Add(
                "Lesson contains an example.");
        }

        if (!ContainsPractice(lesson.Content))
        {
            score -= 10;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Practice",
                    Message =
                        "Consider adding practice questions or exercises.",
                    Priority = "Medium"
                });

            result.MissingAreas.Add(
                "Practice");
        }
        else
        {
            result.Strengths.Add(
                "Lesson contains practice material.");
        }

        if (!ContainsSummary(lesson.Content))
        {
            score -= 5;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Summary",
                    Message =
                        "Consider adding a short summary at the end of the lesson.",
                    Priority = "Low"
                });

            result.MissingAreas.Add(
                "Summary");
        }
        else
        {
            result.Strengths.Add(
                "Lesson contains a summary.");
        }
    }

    private static void AnalyzeKeywords(
        LessonKnowledgeDto lesson,
        LessonAnalysisResult result,
        ref int score)
    {
        if (string.IsNullOrWhiteSpace(lesson.Keywords))
        {
            score -= 5;

            result.Suggestions.Add(
                new LessonSuggestion
                {
                    Area = "Keywords",
                    Message =
                        "Add keywords to improve lesson discovery and AI retrieval.",
                    Priority = "Medium"
                });

            result.MissingAreas.Add(
                "Keywords");
        }
        else
        {
            result.Strengths.Add(
                "Lesson has keywords.");
        }
    }

    // ==================================================
    // Helpers
    // ==================================================

    private static bool ContainsExample(
        string content)
    {
        return content.Contains(
                   "example",
                   StringComparison.OrdinalIgnoreCase)
               ||
               content.Contains(
                   "for example",
                   StringComparison.OrdinalIgnoreCase)
               ||
               content.Contains(
                   "```",
                   StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsPractice(
        string content)
    {
        return content.Contains(
                   "practice",
                   StringComparison.OrdinalIgnoreCase)
               ||
               content.Contains(
                   "exercise",
                   StringComparison.OrdinalIgnoreCase)
               ||
               content.Contains(
                   "try this",
                   StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsSummary(
        string content)
    {
        return content.Contains(
                   "summary",
                   StringComparison.OrdinalIgnoreCase)
               ||
               content.Contains(
                   "in conclusion",
                   StringComparison.OrdinalIgnoreCase)
               ||
               content.Contains(
                   "key points",
                   StringComparison.OrdinalIgnoreCase);
    }
}

//using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;
//using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;
//using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
//using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;
//using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

//namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement;

//public class LessonEnhancementAgent : ILessonEnhancementAgent
//{
//    private readonly IEnumerable<ISubjectExpertise> _subjectExpertises;

//    public LessonEnhancementAgent(IEnumerable<ISubjectExpertise> subjectExpertises)
//    {
//        _subjectExpertises = subjectExpertises;
//    }

//    public Task<LessonAnalysisResult> AnalyzeAsync(LessonKnowledgeDto lesson)
//    {
//        if (lesson == null)
//        {
//            throw new ArgumentNullException(nameof(lesson));
//        }

//        Console.WriteLine($"[Teacher AI Agent] Analysis started. LessonId={lesson.Id}, Title='{lesson.Title}'");

//        var result = new LessonAnalysisResult();
//        int score = 100;

//        AnalyzeTitle(lesson, result, ref score);
//        AnalyzeDescription(lesson, result, ref score);
//        AnalyzeContent(lesson, result, ref score);
//        AnalyzeKeywords(lesson, result, ref score);

//        AnalyzeSubjectExpertise(lesson, result, ref score);

//        result.Score = Math.Clamp(score, 0, 100);

//        Console.WriteLine($"[Teacher AI Agent] Analysis completed. Score={result.Score}, Strengths={result.Strengths.Count}, MissingAreas={result.MissingAreas.Count}, Suggestions={result.Suggestions.Count}");

//        return Task.FromResult(result);
//    }

//    private void AnalyzeSubjectExpertise(LessonKnowledgeDto lesson, LessonAnalysisResult result, ref int score)
//    {
//        foreach (var expertise in _subjectExpertises)
//        {
//            try
//            {
//                if (!expertise.CanHandle(lesson))
//                {
//                    continue;
//                }

//                Console.WriteLine($"[Teacher AI Agent] Using expertise: {expertise.SubjectName}");

//                var subjectResult = new SubjectAnalysisResult
//                {
//                    Subject = expertise.SubjectName
//                };

//                expertise.Analyze(lesson, subjectResult);

//                foreach (var strength in subjectResult.Strengths)
//                {
//                    result.Strengths.Add($"[{expertise.SubjectName}] {strength}");
//                }

//                foreach (var suggestion in subjectResult.Suggestions)
//                {
//                    result.Suggestions.Add(new LessonSuggestion
//                    {
//                        Area = expertise.SubjectName,
//                        Message = suggestion,
//                        Priority = "Medium"
//                    });
//                }

//                foreach (var missing in subjectResult.MissingAreas)
//                {
//                    result.MissingAreas.Add($"{expertise.SubjectName}: {missing}");
//                }

//                score += subjectResult.ScoreAdjustment;

//                Console.WriteLine($"[Teacher AI Agent] Expertise={expertise.SubjectName}, ScoreAdjustment={subjectResult.ScoreAdjustment}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"[Teacher AI Agent] Expertise '{expertise.SubjectName}' failed: {ex.Message}");
//            }
//        }
//    }

//    public Task<LessonImprovementResult> ImproveAsync(LessonKnowledgeDto lesson)
//    {
//        if (lesson == null)
//        {
//            throw new ArgumentNullException(nameof(lesson));
//        }

//        var suggestions = new List<LessonSuggestion>();
//        var content = lesson.Content?.Trim() ?? string.Empty;

//        if (string.IsNullOrWhiteSpace(content))
//        {
//            return Task.FromResult(new LessonImprovementResult
//            {
//                ImprovedContent = content,
//                Suggestions =
//                {
//                    new LessonSuggestion
//                    {
//                        Area = "Content",
//                        Message = "There is no lesson content to improve.",
//                        Priority = "High"
//                    }
//                }
//            });
//        }

//        if (!ContainsExample(content))
//        {
//            suggestions.Add(new LessonSuggestion
//            {
//                Area = "Examples",
//                Message = "Add a practical example related to the main concept.",
//                Priority = "Medium"
//            });
//        }

//        if (!ContainsPractice(content))
//        {
//            suggestions.Add(new LessonSuggestion
//            {
//                Area = "Practice",
//                Message = "Add practice exercises so students can apply the concept.",
//                Priority = "Medium"
//            });
//        }

//        if (!ContainsSummary(content))
//        {
//            suggestions.Add(new LessonSuggestion
//            {
//                Area = "Summary",
//                Message = "Add a short summary of the key concepts.",
//                Priority = "Low"
//            });
//        }

//        var improvedContent = content;

//        if (!ContainsSummary(content))
//        {
//            improvedContent += """

//## Summary

//Review the main concepts from this lesson and make sure you understand the key ideas before moving to the next lesson.
//""";
//        }

//        return Task.FromResult(new LessonImprovementResult
//        {
//            ImprovedContent = improvedContent,
//            Suggestions = suggestions
//        });
//    }

//    public Task<string> GenerateExamplesAsync(LessonKnowledgeDto lesson)
//    {
//        if (lesson == null)
//        {
//            throw new ArgumentNullException(nameof(lesson));
//        }

//        var title = lesson.Title?.Trim() ?? "this lesson";

//        var result = $"""
//## Example

//Here is a simple example related to **{title}**.

//Review the example step by step and identify how the main concept from the lesson is being used.

//Try changing the example and observe what happens.
//""";

//        return Task.FromResult(result);
//    }

//    public Task<string> GeneratePracticeAsync(LessonKnowledgeDto lesson)
//    {
//        if (lesson == null)
//        {
//            throw new ArgumentNullException(nameof(lesson));
//        }

//        var title = lesson.Title?.Trim() ?? "this lesson";

//        var result = $"""
//## Practice

//Try this practice activity based on **{title}**:

//1. Review the main concept.
//2. Create your own small example.
//3. Explain what your example does.
//4. Change one part of the example.
//5. Describe what changed and why.
//""";

//        return Task.FromResult(result);
//    }

//    public Task<string> GenerateSummaryAsync(LessonKnowledgeDto lesson)
//    {
//        if (lesson == null)
//        {
//            throw new ArgumentNullException(nameof(lesson));
//        }

//        var title = lesson.Title?.Trim() ?? "this lesson";

//        var result = $"""
//## Summary

//In this lesson, you learned the main concepts of **{title}**.

//Before continuing, make sure you can:

//- Explain the main concept.
//- Give a simple example.
//- Apply the concept in a small exercise.
//- Explain your answer in your own words.
//""";

//        return Task.FromResult(result);
//    }

//    private static void AnalyzeTitle(LessonKnowledgeDto lesson, LessonAnalysisResult result, ref int score)
//    {
//        if (string.IsNullOrWhiteSpace(lesson.Title))
//        {
//            score -= 15;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Title",
//                Message = "Add a clear lesson title.",
//                Priority = "High"
//            });
//            result.MissingAreas.Add("Title");
//        }
//        else
//        {
//            result.Strengths.Add("Lesson has a title.");
//        }
//    }

//    private static void AnalyzeDescription(LessonKnowledgeDto lesson, LessonAnalysisResult result, ref int score)
//    {
//        if (string.IsNullOrWhiteSpace(lesson.Description))
//        {
//            score -= 10;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Description",
//                Message = "Add a short description explaining what this lesson teaches.",
//                Priority = "Medium"
//            });
//            result.MissingAreas.Add("Description");
//        }
//        else
//        {
//            result.Strengths.Add("Lesson has a description.");
//        }
//    }

//    private static void AnalyzeContent(LessonKnowledgeDto lesson, LessonAnalysisResult result, ref int score)
//    {
//        if (string.IsNullOrWhiteSpace(lesson.Content))
//        {
//            score -= 30;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Content",
//                Message = "Lesson content is required.",
//                Priority = "High"
//            });
//            result.MissingAreas.Add("Content");
//            return;
//        }

//        result.Strengths.Add("Lesson contains learning content.");

//        if (lesson.Content.Length < 300)
//        {
//            score -= 10;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Content Depth",
//                Message = "The lesson content is quite short. Consider adding explanations and examples.",
//                Priority = "Medium"
//            });
//            result.MissingAreas.Add("Detailed Explanation");
//        }

//        if (!ContainsExample(lesson.Content))
//        {
//            score -= 10;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Examples",
//                Message = "Consider adding at least one practical example.",
//                Priority = "Medium"
//            });
//            result.MissingAreas.Add("Examples");
//        }
//        else
//        {
//            result.Strengths.Add("Lesson contains an example.");
//        }

//        if (!ContainsPractice(lesson.Content))
//        {
//            score -= 10;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Practice",
//                Message = "Consider adding practice questions or exercises.",
//                Priority = "Medium"
//            });
//            result.MissingAreas.Add("Practice");
//        }
//        else
//        {
//            result.Strengths.Add("Lesson contains practice material.");
//        }

//        if (!ContainsSummary(lesson.Content))
//        {
//            score -= 5;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Summary",
//                Message = "Consider adding a short summary at the end of the lesson.",
//                Priority = "Low"
//            });
//            result.MissingAreas.Add("Summary");
//        }
//        else
//        {
//            result.Strengths.Add("Lesson contains a summary.");
//        }
//    }

//    private static void AnalyzeKeywords(LessonKnowledgeDto lesson, LessonAnalysisResult result, ref int score)
//    {
//        if (string.IsNullOrWhiteSpace(lesson.Keywords))
//        {
//            score -= 5;
//            result.Suggestions.Add(new LessonSuggestion
//            {
//                Area = "Keywords",
//                Message = "Add keywords to improve lesson discovery and AI retrieval.",
//                Priority = "Medium"
//            });
//            result.MissingAreas.Add("Keywords");
//        }
//        else
//        {
//            result.Strengths.Add("Lesson has keywords.");
//        }
//    }

//    private static bool ContainsExample(string content)
//    {
//        return content.Contains("example", StringComparison.OrdinalIgnoreCase) ||
//               content.Contains("for example", StringComparison.OrdinalIgnoreCase) ||
//               content.Contains("```", StringComparison.OrdinalIgnoreCase);
//    }

//    private static bool ContainsPractice(string content)
//    {
//        return content.Contains("practice", StringComparison.OrdinalIgnoreCase) ||
//               content.Contains("exercise", StringComparison.OrdinalIgnoreCase) ||
//               content.Contains("try this", StringComparison.OrdinalIgnoreCase);
//    }

//    private static bool ContainsSummary(string content)
//    {
//        return content.Contains("summary", StringComparison.OrdinalIgnoreCase) ||
//               content.Contains("in conclusion", StringComparison.OrdinalIgnoreCase) ||
//               content.Contains("key points", StringComparison.OrdinalIgnoreCase);
//    }
//}