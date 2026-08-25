using FlowAISystem.Application;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.English;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Programming;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
using Microsoft.Extensions.DependencyInjection;

namespace FlowAISystem.Tests.Application.AI.Teacher.LessonEnhancement;

public class LessonEnhancementDependencyInjectionTests
{
    // ==================================================
    // 1. Application DI registers Teacher AI expertise
    // ==================================================

    [Fact]
    public void AddApplication_RegistersTeacherAIExpertise()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        var descriptors =
            services
                .Where(x =>
                    x.ServiceType ==
                    typeof(ISubjectExpertise))
                .ToList();

        // Assert
        Assert.NotEmpty(descriptors);

        Assert.Contains(
            descriptors,
            x =>
                x.ImplementationType ==
                typeof(ProgrammingExpertise));

        Assert.Contains(
            descriptors,
            x =>
                x.ImplementationType ==
                typeof(EnglishExpertise));
    }

    // ==================================================
    // 2. Application DI registers LessonEnhancementAgent
    // ==================================================

    [Fact]
    public void AddApplication_RegistersLessonEnhancementAgent()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        var descriptor =
            services.FirstOrDefault(
                x =>
                    x.ServiceType ==
                    typeof(ILessonEnhancementAgent));

        // Assert
        Assert.NotNull(descriptor);

        Assert.Equal(
            typeof(LessonEnhancementAgent),
            descriptor.ImplementationType);
    }

    // ==================================================
    // 3. Application DI registers LessonEnhancementService
    // ==================================================

    [Fact]
    public void AddApplication_RegistersLessonEnhancementService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        var descriptor =
            services.FirstOrDefault(
                x =>
                    x.ServiceType ==
                    typeof(ILessonEnhancementService));

        // Assert
        Assert.NotNull(descriptor);

        Assert.Equal(
            typeof(LessonEnhancementService),
            descriptor.ImplementationType);
    }

    // ==================================================
    // 4. Application DI registers LessonKnowledgeService
    // ==================================================

    [Fact]
    public void AddApplication_RegistersLessonKnowledgeService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        var descriptor =
            services.FirstOrDefault(
                x =>
                    x.ServiceType ==
                    typeof(ILessonKnowledgeService));

        // Assert
        Assert.NotNull(descriptor);
    }

    // ==================================================
    // 5. Agent can be resolved
    // ==================================================

    [Fact]
    public void ServiceProvider_CanResolveLessonEnhancementAgent()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider =
            services.BuildServiceProvider();

        // Act
        var agent =
            provider.GetRequiredService<
                ILessonEnhancementAgent>();

        // Assert
        Assert.NotNull(agent);

        Assert.IsType<
            LessonEnhancementAgent>(
            agent);
    }

    // ==================================================
    // 6. Programming expertise can be resolved
    // ==================================================

    [Fact]
    public void ServiceProvider_ResolvesProgrammingExpertise()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider =
            services.BuildServiceProvider();

        // Act
        var expertises =
            provider
                .GetServices<ISubjectExpertise>()
                .ToList();

        // Assert
        Assert.NotEmpty(expertises);

        Assert.Contains(
            expertises,
            expertise =>
                expertise is ProgrammingExpertise);
    }

    // ==================================================
    // 7. English expertise can be resolved
    // ==================================================

    [Fact]
    public void ServiceProvider_ResolvesEnglishExpertise()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider =
            services.BuildServiceProvider();

        // Act
        var expertises =
            provider
                .GetServices<ISubjectExpertise>()
                .ToList();

        // Assert
        Assert.NotEmpty(expertises);

        Assert.Contains(
            expertises,
            expertise =>
                expertise is EnglishExpertise);
    }

    // ==================================================
    // 8. All subject expertise can be resolved
    // ==================================================

    [Fact]
    public void ServiceProvider_ResolvesAllSubjectExpertise()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider =
            services.BuildServiceProvider();

        // Act
        var expertises =
            provider
                .GetServices<ISubjectExpertise>()
                .ToList();

        // Assert
        Assert.Contains(
            expertises,
            expertise =>
                expertise is ProgrammingExpertise);

        Assert.Contains(
            expertises,
            expertise =>
                expertise is EnglishExpertise);
    }

    // ==================================================
    // 9. Complete programming workflow
    // ==================================================

    [Fact]
    public async Task Agent_CanAnalyzeProgrammingLesson()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider =
            services.BuildServiceProvider();

        var agent =
            provider.GetRequiredService<
                ILessonEnhancementAgent>();

        var lesson =
            new LessonKnowledgeDto
            {
                Id = 1,

                Title = "C# Variables",

                Description =
                    "Learn how variables store data in C#.",

                Content = """
                    # C# Variables

                    Variables store data in a C# program.

                    ## Example

                    ```csharp
                    int age = 20;
                    string name = "Chantha";
                    ```

                    ## Practice

                    Create three variables.

                    ## Summary

                    Variables store values that a program can use.
                    """,

                Keywords =
                    "C#, variables, programming",

                Category =
                    "Programming"
            };

        // Act
        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert
        Assert.NotNull(result);

        Assert.InRange(
            result.Score,
            0,
            100);

        Assert.Contains(
            result.Strengths,
            strength =>
                strength.Contains(
                    "Programming",
                    StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 10. Complete English workflow
    // ==================================================

    [Fact]
    public async Task Agent_CanAnalyzeEnglishLesson()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider =
            services.BuildServiceProvider();

        var agent =
            provider.GetRequiredService<
                ILessonEnhancementAgent>();

        var lesson =
            new LessonKnowledgeDto
            {
                Id = 2,

                Title = "English Nouns",

                Description =
                    "Learn what nouns are and how they are used.",

                Content = """
                    # English Nouns

                    A noun is a word that names a person,
                    place, thing, or idea.

                    ## Example

                    The teacher reads a book.

                    ## Practice

                    Find the nouns in these sentences.

                    ## Summary

                    Nouns name people, places, things, or ideas.
                    """,

                Keywords =
                    "English, nouns, grammar",

                Category =
                    "English"
            };

        // Act
        var result =
            await agent.AnalyzeAsync(
                lesson);

        // Assert
        Assert.NotNull(result);

        Assert.InRange(
            result.Score,
            0,
            100);

        Assert.Contains(
            result.Strengths,
            strength =>
                strength.Contains(
                    "English",
                    StringComparison.OrdinalIgnoreCase));
    }

    // ==================================================
    // 11. Verify complete registration graph
    // ==================================================

    [Fact]
    public void ServiceProvider_TeacherAIRegistrationGraph_IsCorrect()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider =
            services.BuildServiceProvider();

        // Act
        var agent =
            provider.GetRequiredService<
                ILessonEnhancementAgent>();

        var expertises =
            provider
                .GetServices<ISubjectExpertise>()
                .ToList();

        // Assert
        Assert.NotNull(agent);

        Assert.IsType<
            LessonEnhancementAgent>(
            agent);

        Assert.Contains(
            expertises,
            expertise =>
                expertise is ProgrammingExpertise);

        Assert.Contains(
            expertises,
            expertise =>
                expertise is EnglishExpertise);
    }
}