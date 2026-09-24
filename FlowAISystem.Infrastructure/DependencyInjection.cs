using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.AI.Knowledge.Interfaces;
using FlowAISystem.Application.Common.Interfaces;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Infrastructure.AI.Knowledge.Providers;
using FlowAISystem.Infrastructure.Configuration;
using FlowAISystem.Infrastructure.Repositories;
using FlowAISystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration; // Ensures GetSection and Bind extension methods are available
using Microsoft.Extensions.DependencyInjection;

namespace FlowAISystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Repositories
        services.AddScoped<IAIKnowledgeRepository, AIKnowledgeRepository>();
        services.AddScoped<IAICategoryRepository, AICategoryRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseOfferingRepository, CourseOfferingRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IScoreRepository, ScoreRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<ILessonKnowledgeRepository, LessonKnowledgeRepository>();
        services.AddScoped<IAIConversationRepository, AIConversationRepository>();

        // Knowledge Providers
        services.AddScoped<IKnowledgeProvider, LessonKnowledgeProvider>();

        // Speech & Audio Infrastructure Configuration
        //services.Configure<AzureSpeechOptions>(
        //    configuration.GetSection(AzureSpeechOptions.SectionName));

        // Register TTS, STT, and Video Pipeline Services
        services.RegisterTtsServices();

        return services;
    }

    private static IServiceCollection RegisterTtsServices(this IServiceCollection services)
    {
        services.AddScoped<ITextToSpeechService, AzureTextToSpeechService>();
        services.AddScoped<ISpeechPreparationService, SpeechPreparationServiceV3>();
        services.AddScoped<IVideoTimelineOrchestrator, VideoTimelineOrchestrator>();

        return services;
    }
}