using Microsoft.Extensions.DependencyInjection;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.Services;

using FlowAISystem.Application.Interfaces.Services.Reports;
using FlowAISystem.Application.Services.Reports;
using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.AI.Services;

using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.AI.Student.Services; // or wherever your formatter implementation class is located

namespace FlowAISystem.Application;


public static class DependencyInjection
{

    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {

        services.AddScoped<IAuthService, AuthService>();


        services.AddScoped<IAdminDashboardService, AdminDashboardService>();


        services.AddScoped<IAIService, AIService>();


        services.AddScoped<IAIKnowledgeManagementService,
            AIKnowledgeManagementService>();


        services.AddScoped<IAICategoryService,
            AICategoryService>();


        services.AddScoped<IStudentService,
            StudentService>();


        services.AddScoped<ITeacherService,
            TeacherService>();


        services.AddScoped<IDepartmentService,
            DepartmentService>();

        services.AddScoped<IUserService, UserService>();// new registe for users 

        services.AddScoped<ICourseService, CourseService>();

        services.AddScoped<ICourseOfferingService, CourseOfferingService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();

        services.AddScoped<IScoreService, ScoreService>(); // score if add here, in program.cs only need builder.Services.AddApplication(); and  builder.Services.AddInfrastructure();

        services.AddScoped<IAttendanceService, AttendanceService>(); // 

        services.AddScoped<IFeedbackService, FeedbackService>(); // Application DependencyInjection for service 

        services.AddScoped<IAnnouncementService, AnnouncementService>();

        // Reports
        services.AddScoped<IReportService, ReportService>();

        //teacher part
        services.AddScoped<ILessonKnowledgeService, LessonKnowledgeService>();

        services.AddScoped<IStudentAIService, StudentAIService>();

        services.AddScoped<IStudentAIResponseFormatter, StudentAIResponseFormatter>(); // fomat
        services.AddScoped<ILessonRankingService, LessonRankingService>();

        //services.AddScoped<IStudentIntentDetector, StudentIntentDetector>();

        //services.AddScoped<IKeywordExtractor, KeywordExtractor>();
        services.AddScoped<IAIConversationService, AIConversationService>();

        services.AddScoped<IConversationTitleService,
                   ConversationTitleService>();

        return services;

    }

}