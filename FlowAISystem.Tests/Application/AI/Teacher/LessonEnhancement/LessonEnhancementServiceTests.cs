using FlowAISystem.Application.AI.Teacher.LessonEnhancement;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Programming;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Tests.Application.AI.Teacher.LessonEnhancement;

public class LessonEnhancementServiceTests
{
    // ==================================================
    // Test Lesson
    // ==================================================

    private static LessonKnowledgeDto CreateLesson(
        int id = 1,
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
        string keywords = "C#, variables, programming")
    {
        return new LessonKnowledgeDto
        {
            Id = id,

            Title = title,

            Description = description,

            Content = content,

            Keywords = keywords,

            Category = "Programming",

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
    // Create real Agent
    // ==================================================

    private static ILessonEnhancementAgent CreateAgent()
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
    // Create Service
    // ==================================================

    private static LessonEnhancementService CreateService(
        LessonKnowledgeDto lesson)
    {
        var lessonKnowledgeService =
            new FakeLessonKnowledgeService(lesson);

        var agent =
            CreateAgent();

        return new LessonEnhancementService(
            lessonKnowledgeService,
            agent);
    }

    // ==================================================
    // 1. AnalyzeAsync
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ValidLesson_ReturnsAnalysis()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.AnalyzeAsync(
                lesson.Id);

        Assert.NotNull(result);
    }

    // ==================================================
    // 2. AnalyzeAsync expected score
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ValidLesson_ReturnsExpectedScore()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.AnalyzeAsync(
                lesson.Id);

        Assert.NotNull(result);

        Assert.InRange(
            result.Score,
            0,
            100);
    }

    // ==================================================
    // 3. Programming expertise
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_ProgrammingLesson_UsesProgrammingExpertise()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.AnalyzeAsync(
                lesson.Id);

        Assert.NotNull(result);

        Assert.Contains(
            result.Strengths,
            x =>
                x.Contains(
                    "Programming",
                    StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 4. Missing lesson
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_MissingLesson_ReturnsNull()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.AnalyzeAsync(999);

        Assert.Null(result);
    }

    // ==================================================
    // 5. Invalid lesson ID
    // ==================================================

    [Fact]
    public async Task AnalyzeAsync_InvalidLessonId_ThrowsArgumentException()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        await Assert.ThrowsAsync<ArgumentException>(
            () =>
                service.AnalyzeAsync(0));
    }

    // ==================================================
    // 6. ImproveAsync
    // ==================================================

    [Fact]
    public async Task ImproveAsync_ValidLesson_ReturnsImprovement()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.ImproveAsync(
                lesson.Id);

        Assert.NotNull(result);

        Assert.NotNull(
            result.ImprovedContent);

        Assert.NotNull(
            result.Suggestions);
    }

    // ==================================================
    // 7. ImproveAsync missing lesson
    // ==================================================

    [Fact]
    public async Task ImproveAsync_MissingLesson_ReturnsNull()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.ImproveAsync(999);

        Assert.Null(result);
    }

    // ==================================================
    // 8. ImproveAsync invalid ID
    // ==================================================

    [Fact]
    public async Task ImproveAsync_InvalidLessonId_ThrowsArgumentException()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        await Assert.ThrowsAsync<ArgumentException>(
            () =>
                service.ImproveAsync(0));
    }

    // ==================================================
    // 9. GenerateExamplesAsync
    // ==================================================

    [Fact]
    public async Task GenerateExamplesAsync_ValidLesson_ReturnsExample()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.GenerateExamplesAsync(
                lesson.Id);

        Assert.NotNull(result);

        Assert.Contains(
            "## Example",
            result);
    }

    // ==================================================
    // 10. GenerateExamplesAsync missing lesson
    // ==================================================

    [Fact]
    public async Task GenerateExamplesAsync_MissingLesson_ReturnsNull()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.GenerateExamplesAsync(999);

        Assert.Null(result);
    }

    // ==================================================
    // 11. GeneratePracticeAsync
    // ==================================================

    [Fact]
    public async Task GeneratePracticeAsync_ValidLesson_ReturnsPractice()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.GeneratePracticeAsync(
                lesson.Id);

        Assert.NotNull(result);

        Assert.Contains(
            "## Practice",
            result);
    }

    // ==================================================
    // 12. GeneratePracticeAsync missing lesson
    // ==================================================

    [Fact]
    public async Task GeneratePracticeAsync_MissingLesson_ReturnsNull()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.GeneratePracticeAsync(999);

        Assert.Null(result);
    }

    // ==================================================
    // 13. GenerateSummaryAsync
    // ==================================================

    [Fact]
    public async Task GenerateSummaryAsync_ValidLesson_ReturnsSummary()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.GenerateSummaryAsync(
                lesson.Id);

        Assert.NotNull(result);

        Assert.Contains(
            "## Summary",
            result);
    }

    // ==================================================
    // 14. GenerateSummaryAsync missing lesson
    // ==================================================

    [Fact]
    public async Task GenerateSummaryAsync_MissingLesson_ReturnsNull()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        var result =
            await service.GenerateSummaryAsync(999);

        Assert.Null(result);
    }

    // ==================================================
    // 15. GenerateSummaryAsync invalid ID
    // ==================================================

    [Fact]
    public async Task GenerateSummaryAsync_InvalidLessonId_ThrowsArgumentException()
    {
        var lesson = CreateLesson();

        var service = CreateService(lesson);

        await Assert.ThrowsAsync<ArgumentException>(
            () =>
                service.GenerateSummaryAsync(0));
    }

    // ==================================================
    // Fake LessonKnowledgeService
    // ==================================================

    private sealed class FakeLessonKnowledgeService
        : ILessonKnowledgeService
    {
        private readonly LessonKnowledgeDto _lesson;

        public FakeLessonKnowledgeService(
            LessonKnowledgeDto lesson)
        {
            _lesson = lesson;
        }

        // ==================================================
        // GetAllAsync
        // ==================================================

        public Task<List<LessonKnowledgeListItemDto>> GetAllAsync(
            LessonKnowledgeSearchDto search)
        {
            return Task.FromResult(
                new List<LessonKnowledgeListItemDto>());
        }

        // ==================================================
        // GetByIdAsync
        // ==================================================

        public Task<LessonKnowledgeDto?> GetByIdAsync(
            int id)
        {
            if (id == _lesson.Id)
            {
                return Task.FromResult<
                    LessonKnowledgeDto?>(_lesson);
            }

            return Task.FromResult<
                LessonKnowledgeDto?>(null);
        }

        // ==================================================
        // CreateAsync
        // ==================================================

        public Task CreateAsync(
            CreateLessonKnowledgeDto dto)
        {
            return Task.CompletedTask;
        }

        // ==================================================
        // UpdateAsync
        // ==================================================

        public Task UpdateAsync(
            UpdateLessonKnowledgeDto dto)
        {
            return Task.CompletedTask;
        }

        // ==================================================
        // DeleteAsync
        // ==================================================

        public Task DeleteAsync(
            int id)
        {
            return Task.CompletedTask;
        }

        // ==================================================
        // GetByTeacherAsync
        // ==================================================

        public Task<List<LessonKnowledgeListItemDto>> GetByTeacherAsync(
            int teacherId)
        {
            return Task.FromResult(
                new List<LessonKnowledgeListItemDto>());
        }

        // ==================================================
        // SearchAsync
        // ==================================================

        public Task<List<LessonKnowledgeDto>> SearchAsync(
            IEnumerable<string> keywords)
        {
            return Task.FromResult(
                new List<LessonKnowledgeDto>());
        }

        // ==================================================
        // GetAllForTutorialAsync
        // ==================================================

        public Task<List<LessonKnowledgeDto>> GetAllForTutorialAsync(
            LessonKnowledgeSearchDto search)
        {
            return Task.FromResult(
                new List<LessonKnowledgeDto>());
        }

        // ==================================================
        // GetLessonForAIAsync
        // ==================================================

        public Task<LessonKnowledgeDto?> GetLessonForAIAsync(
            int lessonId)
        {
            if (lessonId == _lesson.Id)
            {
                return Task.FromResult<
                    LessonKnowledgeDto?>(_lesson);
            }

            return Task.FromResult<
                LessonKnowledgeDto?>(null);
        }
    }
}