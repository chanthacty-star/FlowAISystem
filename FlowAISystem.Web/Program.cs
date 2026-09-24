using System.Text;
using ApexCharts;
using FlowAISystem.Infrastructure.Services;
using FlowAISystem.Application;
using FlowAISystem.Application.AI;
using FlowAISystem.Application.AI.Handlers;
using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.AI.Memory;
using FlowAISystem.Application.AI.Services;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.AI.Student.Services;
using FlowAISystem.Application.AI.Teacher.TeacherAssistant;
using FlowAISystem.Application.AI.Teacher.TeacherAssistant.Interfaces;
using FlowAISystem.Application.Common.Interfaces;
using FlowAISystem.Application.Interfaces.Export;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.Interfaces.Services.Reports;
using FlowAISystem.Application.Security;
using FlowAISystem.Application.Services;
using FlowAISystem.Application.Services.Reports;
using FlowAISystem.Infrastructure;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Infrastructure.Export.Excel;
using FlowAISystem.Infrastructure.Export.PDF;
using FlowAISystem.Infrastructure.Repositories;
using FlowAISystem.Web.Authentication;
using FlowAISystem.Web.Components;
using FlowAISystem.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Layer Extension Registrations
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration); // Passed Configuration

// 2. Database Context Factory
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 3. Authentication & Authorization
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// 4. Application Business Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IAIService, AIService>();
builder.Services.AddScoped<IAICategoryService, AICategoryService>();
builder.Services.AddScoped<IAIKnowledgeManagementService, AIKnowledgeManagementService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

builder.Services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
builder.Services.AddScoped<IAcademicYearService, AcademicYearService>();

builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<ICourseOfferingService, CourseOfferingService>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();

// 5. Reports & Export Services
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPdfReportGenerator, PdfReportGenerator>();
builder.Services.AddScoped<IExcelReportGenerator, ExcelReportGenerator>();

// 6. AI System Pipeline & Handlers
builder.Services.AddScoped<IAIIntentDetector, AIIntentDetector>();
builder.Services.AddScoped<AIRequestRouter>();
builder.Services.AddScoped<IStudentAIHandler, StudentAIHandler>();
builder.Services.AddScoped<IDashboardAIHandler, DashboardAIHandler>();
builder.Services.AddScoped<IAIKnowledgeHandler, AIKnowledgeHandler>();
builder.Services.AddScoped<IGeneralAIHandler, GeneralAIHandler>();
builder.Services.AddScoped<IReportAIHandler, ReportAIHandler>();
builder.Services.AddScoped<AIConversationMemory>();
builder.Services.AddScoped<AIConversationContext>();
builder.Services.AddScoped<IAIConversationService, AIConversationService>();
builder.Services.AddScoped<IMarkdownService, MarkdownService>();

// Student & Teacher AI Assistants
builder.Services.AddScoped<IStudentAIService, StudentAIService>();
builder.Services.AddScoped<IStudentIntentDetector, StudentIntentDetector>();
builder.Services.AddScoped<IKeywordExtractor, KeywordExtractor>();
builder.Services.AddScoped<ITeacherAssistant, TeacherAssistant>();

// 7. Video & Speech Orchestration (Application Layer)
// Note: ISpeechPreparationService & ITextToSpeechService are handles inside AddInfrastructure
builder.Services.AddScoped<IVideoTimelineOrchestrator, VideoTimelineOrchestrator>();

// 8. Blazor Web & UI Services
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthenticationStateProvider>());

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddApexCharts();

// Notifications
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ConfirmationService>();
// nees =======================

// ==================================================
// Application Pipeline Build & Middleware Configuration
// ==================================================
var app = builder.Build();

// Database Seeding
using (var scope = app.Services.CreateScope())
{
    try
    {
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var db = await factory.CreateDbContextAsync();
        await DbSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database seeding failed: {ex.Message}");
    }
}

// HTTP Request Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();