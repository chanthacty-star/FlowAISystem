using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.English;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Tests.Application.AI.Teacher.LessonEnhancement.Expertise;

public class EnglishExpertiseTests
{
    private readonly EnglishExpertise _expertise;

    public EnglishExpertiseTests()
    {
        _expertise = new EnglishExpertise();
    }


    // ==================================================
    // CanHandle
    // ==================================================

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForEnglishLesson()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 1,
            Title = "Past Simple",
            Description =
                "Learn how to use the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple"
        };

        // Act

        var result =
            _expertise.CanHandle(lesson);

        // Assert

        Assert.True(result);
    }


    [Fact]
    public void CanHandle_ShouldReturnTrue_ForGrammarLesson()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 2,
            Title = "English Grammar",
            Description =
                "Learn basic English grammar rules.",
            Category = "Grammar",
            Keywords =
                "grammar, sentence, verbs"
        };

        // Act

        var result =
            _expertise.CanHandle(lesson);

        // Assert

        Assert.True(result);
    }


    [Fact]
    public void CanHandle_ShouldReturnTrue_ForVocabularyLesson()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 3,
            Title = "English Vocabulary",
            Description =
                "Learn common English vocabulary.",
            Category = "English",
            Keywords =
                "vocabulary, words, English"
        };

        // Act

        var result =
            _expertise.CanHandle(lesson);

        // Assert

        Assert.True(result);
    }


    [Fact]
    public void CanHandle_ShouldReturnFalse_ForProgrammingLesson()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 4,
            Title = "C# Variables",
            Description =
                "Learn how variables work in C#.",
            Category = "Programming",
            Keywords =
                "C#, variables, programming"
        };

        // Act

        var result =
            _expertise.CanHandle(lesson);

        // Assert

        Assert.False(result);
    }


    [Fact]
    public void CanHandle_ShouldReturnFalse_ForHistoryLesson()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 5,
            Title = "World War II",
            Description =
                "Learn about important events in World War II.",
            Category = "History",
            Keywords =
                "history, war, world war"
        };

        // Act

        var result =
            _expertise.CanHandle(lesson);

        // Assert

        Assert.False(result);
    }


    [Fact]
    public void CanHandle_ShouldReturnFalse_WhenLessonIsNull()
    {
        // Act

        var result =
            _expertise.CanHandle(null!);

        // Assert

        Assert.False(result);
    }


    // ==================================================
    // Analyze - Content
    // ==================================================

    [Fact]
    public void Analyze_ShouldRecognizeEnglishLearningContent()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 6,
            Title = "Past Simple",
            Description =
                "Learn how to use the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple",

            Content = """
                # Past Simple

                The past simple is used to talk about
                completed actions in the past.

                For example:

                I visited my friend yesterday.

                She watched a movie last night.

                ## Practice

                Write five sentences using the past simple.
                """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.Strengths,
            x =>
                x.Contains(
                    "English lesson contains learning content",
                    StringComparison.OrdinalIgnoreCase));
    }


    // ==================================================
    // Analyze - Example
    // ==================================================

    [Fact]
    public void Analyze_ShouldRecognizeExample()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 7,
            Title = "Past Simple",
            Description =
                "Learn the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple",

            Content = """
                # Past Simple

                The past simple describes completed
                actions in the past.

                ## Example

                I visited my grandmother yesterday.

                She watched television last night.

                ## Practice

                Write three sentences.
                """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.Strengths,
            x =>
                x.Contains(
                    "example",
                    StringComparison.OrdinalIgnoreCase));
    }


    // ==================================================
    // Analyze - Missing Example
    // ==================================================

    [Fact]
    public void Analyze_ShouldSuggestExample_WhenMissing()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 8,
            Title = "Past Simple",
            Description =
                "Learn the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple",

            Content = """
                # Past Simple

                The past simple is used for
                completed actions in the past.

                The verb changes according to
                the tense rules.
                """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Contains(
                    "Example",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Contains(
                    "example",
                    StringComparison.OrdinalIgnoreCase));
    }


    // ==================================================
    // Analyze - Practice
    // ==================================================

    [Fact]
    public void Analyze_ShouldRecognizeEnglishPractice()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 9,
            Title = "Past Simple",
            Description =
                "Learn the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple",

            Content = """
                # Past Simple

                The past simple describes completed
                actions in the past.

                ## Example

                I visited my friend yesterday.

                ## Practice

                Write five sentences using
                the past simple tense.
                """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.Strengths,
            x =>
                x.Contains(
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
            Id = 10,
            Title = "Past Simple",
            Description =
                "Learn the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple",

            Content = """
                # Past Simple

                The past simple describes completed
                actions in the past.

                ## Example

                I visited my friend yesterday.
                """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Contains(
                    "Practice",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Contains(
                    "exercise",
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
            Id = 11,
            Title = "Past Simple",
            Description =
                "Learn the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple",
            Content = string.Empty
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Contains(
                    "English content",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            result.Suggestions,
            x =>
                x.Contains(
                    "English explanation",
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
            Id = 12,
            Title = "Past Simple",
            Description =
                "Learn the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar, past simple",

            Content = """
                # Past Simple

                The past simple talks about
                completed actions.

                ## Example

                I visited my friend.

                ## Practice

                Write a sentence.
                """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.MissingAreas,
            x =>
                x.Contains(
                    "English Explanation Depth",
                    StringComparison.OrdinalIgnoreCase));
    }


    // ==================================================
    // Analyze - Vocabulary
    // ==================================================

    [Fact]
    public void Analyze_ShouldRecognizeVocabulary()
    {
        // Arrange

        var lesson = new LessonKnowledgeDto
        {
            Id = 13,
            Title = "Past Simple Vocabulary",
            Description =
                "Learn vocabulary related to past activities.",
            Category = "English",
            Keywords =
                "English, vocabulary, past simple",

            Content = """
                # Past Simple Vocabulary

                Important vocabulary:

                visit
                watch
                travel
                study

                ## Example

                I visited my friend yesterday.

                ## Practice

                Write sentences using the vocabulary.
                """
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

        // Assert

        Assert.Contains(
            result.Strengths,
            x =>
                x.Contains(
                    "vocabulary",
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
            Id = 14,
            Title = "Past Simple",
            Description =
                "Learn the past simple tense.",
            Category = "English",
            Keywords =
                "English, grammar",

            Content = "The past simple describes actions."
        };

        var result = new SubjectAnalysisResult
        {
            Subject = _expertise.SubjectName
        };

        // Act

        _expertise.Analyze(
            lesson,
            result);

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
    public void SubjectName_ShouldBeEnglish()
    {
        // Act

        var subjectName =
            _expertise.SubjectName;

        // Assert

        Assert.Equal(
            "English",
            subjectName);
    }
}