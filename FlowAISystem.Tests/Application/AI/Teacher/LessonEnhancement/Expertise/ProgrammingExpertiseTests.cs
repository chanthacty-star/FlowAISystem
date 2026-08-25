using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Programming;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Tests.Application.AI.Teacher.LessonEnhancement.Expertise;

public class ProgrammingExpertiseTests
{
    private readonly ProgrammingExpertise _expertise;

    public ProgrammingExpertiseTests()
    {
        _expertise = new ProgrammingExpertise();
    }

    // ==================================================
    // CanHandle
    // ==================================================

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForCSharpLesson()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 1,
            Title = "C# Variables",
            Description = "Learn how variables work in C#.",
            Category = "Programming",
            Keywords = "C#, variables, programming"
        };

        // Act
        var result = _expertise.CanHandle(lesson);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForPythonLesson()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 2,
            Title = "Python Variables",
            Description = "Learn variables in Python.",
            Category = "Programming",
            Keywords = "Python, variables"
        };

        // Act
        var result = _expertise.CanHandle(lesson);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForNonProgrammingLesson()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 3,
            Title = "Past Simple",
            Description = "Learn how to use the past simple tense.",
            Category = "English",
            Keywords = "English, grammar, past simple"
        };

        // Act
        var result = _expertise.CanHandle(lesson);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_WhenLessonIsNull()
    {
        // Act
        var result = _expertise.CanHandle(null!);

        // Assert
        Assert.False(result);
    }

    // ==================================================
    // Analyze - Code Example
    // ==================================================

    [Fact]
    public void Analyze_ShouldRecognizeCodeExample()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 4,
            Title = "C# Variables",
            Description = "Learn variables in C#.",
            Category = "Programming",
            Keywords = "C#, variables",
            Content = """
                       # C# Variables

                       Variables store data in a program.

                       ```csharp
                       int age = 20;
                       Console.WriteLine(age);
                       ```

                       ## Practice

                       Create your own variable.
                       """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act
        _expertise.Analyze(lesson, result);

        // Assert
        Assert.Contains(
            result.Strengths,
            x => x.Contains(
                "code example",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // Analyze - Missing Code Example
    // ==================================================

    [Fact]
    public void Analyze_ShouldSuggestCodeExample_WhenMissing()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 5,
            Title = "C# Variables",
            Description = "Learn variables in C#.",
            Category = "Programming",
            Keywords = "C#, variables",
            Content = """
                       # C# Variables

                       Variables store values in a program.

                       Variables can contain numbers or text.
                       """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act
        _expertise.Analyze(lesson, result);

        // Assert
        Assert.Contains(
            result.MissingAreas,
            x => x.Contains(
                "Code Example",
                StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x => x.Contains(
                "code example",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // Analyze - Practice
    // ==================================================

    [Fact]
    public void Analyze_ShouldRecognizeProgrammingPractice()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 6,
            Title = "C# Variables",
            Description = "Learn variables in C#.",
            Category = "Programming",
            Keywords = "C#, variables",
            Content = """
                       # C# Variables

                       Variables store values.

                       ```csharp
                       int age = 20;
                       ```

                       ## Practice

                       Create three variables and print their values.
                       """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act
        _expertise.Analyze(lesson, result);

        // Assert
        Assert.Contains(
            result.Strengths,
            x => x.Contains(
                "practice",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // Analyze - Missing Practice
    // ==================================================

    [Fact]
    public void Analyze_ShouldSuggestPractice_WhenMissing()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 7,
            Title = "C# Variables",
            Description = "Learn variables in C#.",
            Category = "Programming",
            Keywords = "C#, variables",
            Content = """
                       # C# Variables

                       Variables store values.

                       ```csharp
                       int age = 20;
                       ```
                       """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act
        _expertise.Analyze(lesson, result);

        // Assert
        Assert.Contains(
            result.MissingAreas,
            x => x.Contains(
                "Programming Practice",
                StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x => x.Contains(
                "programming exercise",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // Analyze - Empty Content
    // ==================================================

    [Fact]
    public void Analyze_ShouldReportMissingContent()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 8,
            Title = "C# Variables",
            Description = "Learn variables in C#.",
            Category = "Programming",
            Keywords = "C#"
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act
        _expertise.Analyze(lesson, result);

        // Assert
        Assert.Contains(
            result.MissingAreas,
            x => x.Contains(
                "Programming content",
                StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x => x.Contains(
                "programming explanation",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // Analyze - Explanation Depth
    // ==================================================

    [Fact]
    public void Analyze_ShouldReportExplanationDepth_WhenContentIsShort()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 9,
            Title = "C# Variables",
            Description = "Learn variables in C#.",
            Category = "Programming",
            Keywords = "C#, variables",
            Content = """
                       # C# Variables

                       ```csharp
                       int age = 20;
                       ```

                       ## Practice

                       Create a variable.
                       """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act
        _expertise.Analyze(lesson, result);

        // Assert
        Assert.Contains(
            result.MissingAreas,
            x => x.Contains(
                "Programming Explanation Depth",
                StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // Score Adjustment
    // ==================================================

    [Fact]
    public void Analyze_ShouldKeepScoreAdjustmentWithinExpectedRange()
    {
        // Arrange
        var lesson = new LessonKnowledgeDto
        {
            Id = 10,
            Title = "C# Variables",
            Description = "Learn variables.",
            Category = "Programming",
            Keywords = "C#",
            Content = "Variables store values."
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act
        _expertise.Analyze(lesson, result);

        // Assert
        Assert.InRange(
            result.ScoreAdjustment,
            -20,
            10);
    }

    // ==================================================
    // Subject Name
    // ==================================================

    [Fact]
    public void SubjectName_ShouldBeProgramming()
    {
        // Act
        var subjectName = _expertise.SubjectName;

        // Assert
        Assert.Equal(
            "Programming",
            subjectName);
    }
}