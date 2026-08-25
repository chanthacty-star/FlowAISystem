using FlowAISystem.Application.AI.Teacher.LessonEnhancement;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Programming;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Tests.Application.AI.Teacher.LessonEnhancement;

public class LessonEnhancementAgentAdvancedTests
{
    // ==================================================
    // Test Lesson Factory
    // ==================================================

    private static LessonKnowledgeDto CreateLesson(
        string title = "C# Variables",
        string description =
            "Learn how variables store data in C#.",
        string content = """
            # C# Variables

            Variables are used to store data in a C# program.

            ## Example

            ```csharp
            int age = 20;
            string name = "Chantha";

            Console.WriteLine(age);
            Console.WriteLine(name);
            ```

            ## Practice

            Create three variables using different data types.

            ## Summary

            Variables allow a program to store and use data.
            """,
        string keywords = "C#, variables, programming",
        string category = "Programming")
    {
        return new LessonKnowledgeDto
        {
            Id = 1,

            Title = title,

            Description = description,

            Content = content,

            Keywords = keywords,

            Category = category,

            Difficulty = LessonDifficulty.Beginner,

            ActivityType = "Practice",

            Order = 1,

            TeacherId = 1,

            TeacherName = "Test Teacher",

            CourseOfferingId = 1,

            CourseName = "Programming",

            ReferenceUrl = null,

            AttachmentPath = null,

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            UpdatedAt = null
        };
    }


    // ==================================================
    // Create Agent
    // ==================================================

    private static LessonEnhancementAgent CreateAgent()
    {
        IEnumerable<ISubjectExpertise> expertises =
            new List<ISubjectExpertise>
            {
                new ProgrammingExpertise()
            };

        return new LessonEnhancementAgent(
            expertises);
    }


    // ==================================================
    // 1. Excellent Lesson
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ExcellentLesson_ReturnsHighScore()
    {
        // Arrange

        var lesson =
            CreateLesson();

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.NotNull(result);

        Assert.InRange(
            result.Score,
            70,
            100);
    }


    // ==================================================
    // 2. Missing Title
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingTitle_AddsTitleSuggestion()
    {
        // Arrange

        var lesson =
            CreateLesson(
                title: "");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Equals(
                    "Title",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Area == "Title");
    }


    // ==================================================
    // 3. Missing Description
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingDescription_AddsDescriptionSuggestion()
    {
        // Arrange

        var lesson =
            CreateLesson(
                description: "");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Equals(
                    "Description",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Area == "Description");
    }


    // ==================================================
    // 4. Missing Content
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingContent_AddsContentSuggestion()
    {
        // Arrange

        var lesson =
            CreateLesson(
                content: "");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Equals(
                    "Content",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Area == "Content");
    }


    // ==================================================
    // 5. Missing Examples
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingExamples_AddsExampleSuggestion()
    {
        // Arrange

        var content = """
            # C# Variables

            Variables are used to store data.

            ## Practice

            Create three variables.

            ## Summary

            Variables store values.
            """;

        var lesson =
            CreateLesson(
                content: content);

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Equals(
                    "Examples",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Area == "Examples");
    }


    // ==================================================
    // 6. Missing Practice
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingPractice_AddsPracticeSuggestion()
    {
        // Arrange

        var content = """
            # C# Variables

            Variables are used to store data.

            ## Example

            ```csharp
            int age = 20;
            ```

            ## Summary

            Variables store values.
            """;

        var lesson =
            CreateLesson(
                content: content);

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Equals(
                    "Practice",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Area == "Practice");
    }


    // ==================================================
    // 7. Missing Summary
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingSummary_AddsSummarySuggestion()
    {
        // Arrange

        var content = """
            # C# Variables

            Variables are used to store data.

            ## Example

            ```csharp
            int age = 20;
            ```

            ## Practice

            Create three variables.
            """;

        var lesson =
            CreateLesson(
                content: content);

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Equals(
                    "Summary",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Area == "Summary");
    }


    // ==================================================
    // 8. Missing Keywords
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingKeywords_AddsKeywordSuggestion()
    {
        // Arrange

        var lesson =
            CreateLesson(
                keywords: "");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Equals(
                    "Keywords",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Area == "Keywords");
    }


    // ==================================================
    // 9. Programming Expertise
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ProgrammingLesson_UsesProgrammingExpertise()
    {
        // Arrange

        var lesson =
            CreateLesson(
                category: "Programming");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.Contains(
            result.Strengths,
            x =>
                x.Contains(
                    "Programming",
                    StringComparison.OrdinalIgnoreCase));
    }


    // ==================================================
    // 10. Non-Programming Lesson
    // ==================================================
    [Fact]
    public async Task AnalyzeAsync_NonProgrammingLesson_DoesNotUseProgrammingExpertise()
    {
        // Arrange

        var lesson =
            CreateLesson(
                title: "Ancient Khmer History",
                description:
                    "Learn about important events and developments in Khmer history.",
                content: """
                # Ancient Khmer History

                This lesson introduces important events in Khmer history.

                ## Example

                Students examine an historical event and identify
                its causes and consequences.

                ## Practice

                Identify three important events and explain why
                they were significant.

                ## Summary

                Historical knowledge helps students understand
                how societies developed over time.
                """,
                keywords:
                    "history, Khmer history, ancient history",
                category: "History");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.NotNull(result);

        Assert.DoesNotContain(
            result.Strengths,
            x =>
                x.Contains(
                    "[Programming]",
                    StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(
            result.Suggestions,
            x =>
                x.Area.Equals(
                    "Programming",
                    StringComparison.OrdinalIgnoreCase));
    }


    // ==================================================
    // 11. Score Always Between 0 and 100
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_VeryPoorLesson_ScoreRemainsBetweenZeroAndOneHundred()
    {
        // Arrange

        var lesson =
            CreateLesson(
                title: "",
                description: "",
                content: "",
                keywords: "");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.InRange(
            result.Score,
            0,
            100);
    }


    // ==================================================
    // 12. Null Lesson
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_NullLesson_ThrowsArgumentNullException()
    {
        // Arrange

        var agent =
            CreateAgent();

        // Act & Assert

        await Assert.ThrowsAsync<ArgumentNullException>(
            () =>
                agent.AnalyzeAsync(
                    null!));
    }


    // ==================================================
    // 13. Strengths Are Returned
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_CompleteLesson_ReturnsStrengths()
    {
        // Arrange

        var lesson =
            CreateLesson();

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.NotEmpty(
            result.Strengths);
    }


    // ==================================================
    // 14. Suggestions Are Returned For Weak Lesson
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_WeakLesson_ReturnsSuggestions()
    {
        // Arrange

        var lesson =
            CreateLesson(
                title: "",
                description: "",
                content: "Short content",
                keywords: "");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.NotEmpty(
            result.Suggestions);
    }


    // ==================================================
    // 15. Missing Areas Are Returned For Weak Lesson
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_WeakLesson_ReturnsMissingAreas()
    {
        // Arrange

        var lesson =
            CreateLesson(
                title: "",
                description: "",
                content: "",
                keywords: "");

        var agent =
            CreateAgent();

        // Act

        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert

        Assert.NotEmpty(
            result.MissingAreas);
    }
}