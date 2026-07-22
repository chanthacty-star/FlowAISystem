using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace FlowAISystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {

        services.AddScoped<
            IAIKnowledgeRepository,
            AIKnowledgeRepository>();


        services.AddScoped<
            IAICategoryRepository,
            AICategoryRepository>();

        services.AddScoped<IStudentRepository, StudentRepository>();

        services.AddScoped<ITeacherRepository, TeacherRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();

        services.AddScoped<ICourseOfferingRepository, CourseOfferingRepository>();

        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IScoreRepository, ScoreRepository>();// Score

        services.AddScoped<IAttendanceRepository, AttendanceRepository>(); // attendance 

        // Feedback
        services.AddScoped<IFeedbackRepository, FeedbackRepository>(); // Infrastructure DependencyInjection for Repository

        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

        // Reports
        services.AddScoped<IReportRepository, ReportRepository>();


        return services;
    }
}