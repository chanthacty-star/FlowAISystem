using FlowAISystem.Application.AI.Teacher.LessonEnhancement;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Programming;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Tests.Application.AI.Teacher.LessonEnhancement;

public class LessonEnhancementAgentTests
{
    // ==================================================
    // Helper
    // ==================================================

    private static LessonEnhancementAgent CreateAgent()
    {
        IEnumerable<ISubjectExpertise> expertises =
            new List<ISubjectExpertise>
            {
                new ProgrammingExpertise()
            };

        return new LessonEnhancementAgent(expertises);
    }

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
        Console.WriteLine(age);
        ```

        ## Practice

        Create a variable that stores your name.

        ## Summary

        Variables store values that can be used by a program.
        """,
    string keywords = "C#, variables, programming")
    {
        return new LessonKnowledgeDto
        {
            Id = 1,
            Title = title,
            Description = description,
            Content = content,
            Keywords = keywords,
            Category = "Programming",

            // LessonDifficulty is an enum
            Difficulty = LessonDifficulty.Beginner,

            // ActivityType is string?
            ActivityType = "Practice",

            // Lesson Order is int
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
    // 1. Valid lesson should return analysis
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ValidLesson_ReturnsResult()
    {
        // Arrange
        var agent = CreateAgent();
        var lesson = CreateLesson();

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.NotNull(result);
    }

    // ==================================================
    // 2. Valid lesson should have a high score
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_CompleteLesson_ReturnsHighScore()
    {
        // Arrange
        var agent = CreateAgent();
        var lesson = CreateLesson();

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.True(
            result.Score >= 80,
            $"Expected score >= 80 but received {result.Score}.");
    }

    // ==================================================
    // 3. Title should be recognized as a strength
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_LessonWithTitle_AddsTitleStrength()
    {
        // Arrange
        var agent = CreateAgent();
        var lesson = CreateLesson();

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.Strengths,
            x => x.Contains(
                "title",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 4. Missing title should create suggestion
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingTitle_AddsSuggestion()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson(
            title: "");

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.MissingAreas,
            x => x.Equals(
                "Title",
                StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x => x.Area.Equals(
                "Title",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 5. Missing description should create suggestion
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingDescription_AddsSuggestion()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson(
            description: "");

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.MissingAreas,
            x => x.Equals(
                "Description",
                StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x => x.Area.Equals(
                "Description",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 6. Missing content should reduce score
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingContent_ReducesScore()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson(
            content: "");

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.True(result.Score < 100);

        Assert.Contains(
            result.MissingAreas,
            x => x.Equals(
                "Content",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 7. Missing keywords should create suggestion
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingKeywords_AddsSuggestion()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson(
            keywords: "");

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.MissingAreas,
            x => x.Equals(
                "Keywords",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 8. Complete lesson should detect example
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_LessonWithExample_DetectsExample()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson();

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.Strengths,
            x => x.Contains(
                "example",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 9. Complete lesson should detect practice
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_LessonWithPractice_DetectsPractice()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson();

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.Strengths,
            x => x.Contains(
                "practice",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 10. Complete lesson should detect summary
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_LessonWithSummary_DetectsSummary()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson();

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.Strengths,
            x => x.Contains(
                "summary",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 11. Programming lesson should use expertise
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ProgrammingLesson_UsesProgrammingExpertise()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson(
            content: """
                # C# Variables

                Variables store values in a C# program.

                ## Example

                ```csharp
                int age = 20;
                string name = "Chantha";
                ```

                ## Practice

                Create three variables using different data types.

                ## Summary

                Variables allow programs to store and use data.
                """);

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.Contains(
            result.Strengths,
            x => x.Contains(
                "Programming",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 12. Null lesson should throw
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_NullLesson_ThrowsArgumentNullException()
    {
        // Arrange
        var agent = CreateAgent();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => agent.AnalyzeAsync(null!));
    }

    // ==================================================
    // 13. Score must always remain between 0 and 100
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ScoreIsAlwaysBetweenZeroAndOneHundred()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson(
            title: "",
            description: "",
            content: "",
            keywords: "");

        // Act
        var result = await agent.AnalyzeAsync(lesson);

        // Assert
        Assert.InRange(
            result.Score,
            0,
            100);
    }

    // ==================================================
    // 14. ImproveAsync should preserve existing content
    // ==================================================

    [Fact]
    public async Task ImproveAsync_ExistingContent_PreservesContent()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson();

        // Act
        var result = await agent.ImproveAsync(lesson);

        // Assert
        Assert.NotNull(result);

        Assert.Contains(
            "C# Variables",
            result.ImprovedContent);
    }

    // ==================================================
    // 15. ImproveAsync should add summary when missing
    // ==================================================

    [Fact]
    public async Task ImproveAsync_MissingSummary_AddsSummary()
    {
        // Arrange
        var agent = CreateAgent();

        var lesson = CreateLesson(
            content: """
                # C# Variables

                Variables store values.

                ## Example

                ```csharp
                int age = 20;
                ```

                ## Practice

                Create your own variable.
                """);

        // Act
        var result = await agent.ImproveAsync(lesson);

        // Assert
        Assert.Contains(
            "## Summary",
            result.ImprovedContent);
    }

    // ==================================================
    // 16. GenerateExamplesAsync should return content
    // ==================================================

    [Fact]
    public async Task GenerateExamplesAsync_ValidLesson_ReturnsExample()
    {
        // Arrange
        var agent = CreateAgent();
        var lesson = CreateLesson();

        // Act
        var result =
            await agent.GenerateExamplesAsync(lesson);

        // Assert
        Assert.NotNull(result);

        Assert.Contains(
            "## Example",
            result);
    }

    // ==================================================
    // 17. GeneratePracticeAsync should return practice
    // ==================================================

    [Fact]
    public async Task GeneratePracticeAsync_ValidLesson_ReturnsPractice()
    {
        // Arrange
        var agent = CreateAgent();
        var lesson = CreateLesson();

        // Act
        var result =
            await agent.GeneratePracticeAsync(lesson);

        // Assert
        Assert.NotNull(result);

        Assert.Contains(
            "## Practice",
            result);
    }

    // ==================================================
    // 18. GenerateSummaryAsync should return summary
    // ==================================================

    [Fact]
    public async Task GenerateSummaryAsync_ValidLesson_ReturnsSummary()
    {
        // Arrange
        var agent = CreateAgent();
        var lesson = CreateLesson();

        // Act
        var result =
            await agent.GenerateSummaryAsync(lesson);

        // Assert
        Assert.NotNull(result);

        Assert.Contains(
            "## Summary",
            result);
    }
}