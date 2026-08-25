using Microsoft.Extensions.DependencyInjection;

using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.Services;

// ==========================================================
// Teacher AI - Lesson Enhancement
// ==========================================================

using FlowAISystem.Application.AI.Teacher.LessonEnhancement;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;

using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Programming;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.English;

// ==========================================================
// Student AI
// ==========================================================

using FlowAISystem.Application.AI.Student.Handlers;
using FlowAISystem.Application.AI.Student.Response;

using FlowAISystem.Application.AI.Student.Conversation.Interfaces;
using FlowAISystem.Application.AI.Student.Conversation.Services;

using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.AI.Student.Services;

using FlowAISystem.Application.AI.Student.Quiz.Interfaces;
using FlowAISystem.Application.AI.Student.Quiz.Services;

// ==========================================================
// General AI
// ==========================================================

using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.AI.Services;

// ==========================================================
// Reports
// ==========================================================

using FlowAISystem.Application.Interfaces.Services.Reports;
using FlowAISystem.Application.Services.Reports;


namespace FlowAISystem.Application;


public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // ==================================================
        // Core Application Services
        // ==================================================

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<
            IAdminDashboardService,
            AdminDashboardService>();

        services.AddScoped<
            IAIService,
            AIService>();

        services.AddScoped<
            IAIKnowledgeManagementService,
            AIKnowledgeManagementService>();

        services.AddScoped<
            IAICategoryService,
            AICategoryService>();


        // ==================================================
        // User / Teacher / Student
        // ==================================================

        services.AddScoped<
            IStudentService,
            StudentService>();

        services.AddScoped<
            ITeacherService,
            TeacherService>();

        services.AddScoped<
            IDepartmentService,
            DepartmentService>();

        services.AddScoped<
            IUserService,
            UserService>();


        // ==================================================
        // Course / Enrollment
        // ==================================================

        services.AddScoped<
            ICourseService,
            CourseService>();

        services.AddScoped<
            ICourseOfferingService,
            CourseOfferingService>();

        services.AddScoped<
            IEnrollmentService,
            EnrollmentService>();


        // ==================================================
        // Academic Services
        // ==================================================

        services.AddScoped<
            IScoreService,
            ScoreService>();

        services.AddScoped<
            IAttendanceService,
            AttendanceService>();

        services.AddScoped<
            IFeedbackService,
            FeedbackService>();

        services.AddScoped<
            IAnnouncementService,
            AnnouncementService>();


        // ==================================================
        // Reports
        // ==================================================

        services.AddScoped<
            IReportService,
            ReportService>();


        // ==================================================
        // TEACHER AI ASSISTANCE
        // ==================================================
        //
        // LessonKnowledgeService
        //      ↓
        // LessonEnhancementService
        //      ↓
        // LessonEnhancementAgent
        //
        // The Teacher UI calls:
        //
        // ILessonEnhancementService.AnalyzeAsync()
        //
        // ==================================================
        // ======================================================
        // TEACHER AI ASSISTANCE
        // ======================================================

        services.AddScoped<
            ILessonKnowledgeService,
            LessonKnowledgeService>();

        services.AddScoped<
            ILessonEnhancementAgent,
            LessonEnhancementAgent>();

        services.AddScoped<
            ILessonEnhancementService,
            LessonEnhancementService>();
        // ======================================================
        // Teacher AI Subject Expertise
        // ======================================================

        services.AddScoped<ISubjectExpertise, ProgrammingExpertise>();
        services.AddScoped<ISubjectExpertise, EnglishExpertise>();


        // ==================================================
        // STUDENT AI
        // ==================================================

        services.AddScoped<
            IStudentAIService,
            StudentAIService>();

        services.AddScoped<
            IStudentAIGenerator,
            StudentAIGenerator>();


        // ==================================================
        // Student AI Conversation
        // ==================================================

        services.AddScoped<
            IConversationHistoryService,
            ConversationHistoryService>();


        // ==================================================
        // Student AI Intent
        // ==================================================

        services.AddScoped<
            IStudentIntentDetector,
            StudentIntentDetector>();

        services.AddScoped<
            IKeywordExtractor,
            KeywordExtractor>();


        // ==================================================
        // Student AI Learning
        // ==================================================

        services.AddScoped<
            ILessonRankingService,
            LessonRankingService>();

        services.AddScoped<
            IStudentAIResponseFormatter,
            StudentAIResponseFormatter>();


        // ==================================================
        // Student AI Conversation Services
        // ==================================================

        services.AddScoped<
            IAIConversationService,
            AIConversationService>();

        services.AddScoped<
            IConversationTitleService,
            ConversationTitleService>();

        services.AddScoped<
            IConversationContextBuilder,
            ConversationContextBuilder>();

        services.AddScoped<
            IConversationActionDetector,
            ConversationActionDetector>();

        services.AddScoped<
            IPromptBuilder,
            PromptBuilder>();


        // ==================================================
        // Student AI Workflow Handlers
        // ==================================================

        services.AddScoped<
            IStudentAIWorkflowHandler,
            LearnHandler>();

        services.AddScoped<
            IStudentAIWorkflowHandler,
            ConversationHandler>();

        services.AddScoped<
            IStudentAIWorkflowHandler,
            GreetingHandler>();


        // ==================================================
        // Student AI Quiz
        // ==================================================

        services.AddScoped<
            IStudentAIWorkflowHandler,
            QuizHandler>();

        services.AddScoped<
            IQuizGenerator,
            QuizGenerator>();


        // ==================================================
        // Student AI Response
        // ==================================================

        services.AddScoped<
            IStudentAIResponseBuilder,
            StudentAIResponseBuilder>();


        // ==================================================
        // Return Application Services
        // ==================================================

        return services;
    }
}