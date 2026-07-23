using FlowAISystem.Web.Services;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.AI.Memory;
using FlowAISystem.Application.Services;
using FlowAISystem.Application.AI;
using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.AI.Services;
using FlowAISystem.Application.AI.Handlers;

using FlowAISystem.Application.Interfaces.Services.Reports;
using FlowAISystem.Application.Services.Reports;
using FlowAISystem.Application.Interfaces.Export;
using FlowAISystem.Infrastructure.Export.PDF;
using FlowAISystem.Infrastructure.Export.Excel;
using FlowAISystem.Application;
using FlowAISystem.Infrastructure;
using ApexCharts;
using MudBlazor.Services; // Mud

using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Security;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Infrastructure.Repositories;
using FlowAISystem.Web.Authentication;
using FlowAISystem.Web.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// only two register for Dependency
builder.Services.AddApplication();

builder.Services.AddInfrastructure();
// ==================================================
// Database
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Authentication
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
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key!))
        };
    });
// Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IAIService, AIService>();
builder.Services.AddScoped<IAICategoryService, AICategoryService>();
builder.Services.AddScoped<IAIKnowledgeManagementService, AIKnowledgeManagementService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<
    IAcademicYearRepository,
    AcademicYearRepository>();

builder.Services.AddScoped<
    IAcademicYearService,
    AcademicYearService>();
//semester register
builder.Services.AddScoped<
    ISemesterRepository,
    SemesterRepository>();
// Courses
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddScoped<ICourseService, CourseService>();
// Couse ofering 
builder.Services.AddScoped<
    ICourseOfferingRepository,
    CourseOfferingRepository>();


builder.Services.AddScoped<
    ICourseOfferingService,
    CourseOfferingService>();


builder.Services.AddScoped<
    ISemesterService,
    SemesterService>();
// Departmet 
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
builder.Services.AddScoped<IAIKnowledgeRepository, AIKnowledgeRepository>();
builder.Services.AddScoped<IAICategoryRepository, AICategoryRepository>();
// Reports & Export
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPdfReportGenerator, PdfReportGenerator>();
builder.Services.AddScoped<IExcelReportGenerator, ExcelReportGenerator>();
// AI SYSTEM
builder.Services.AddScoped<IAIIntentDetector, AIIntentDetector>();
builder.Services.AddScoped<AIRequestRouter>();
builder.Services.AddScoped<IStudentAIHandler, StudentAIHandler>(); // student AI
builder.Services.AddScoped<IDashboardAIHandler, DashboardAIHandler>(); // AI DashBboard
builder.Services.AddScoped<IAIKnowledgeHandler, AIKnowledgeHandler>(); // AI Knoledge
builder.Services.AddScoped<IGeneralAIHandler, GeneralAIHandler>(); // general
builder.Services.AddScoped<IReportAIHandler, ReportAIHandler>(); // report
builder.Services.AddScoped<AIConversationMemory>();// memory
builder.Services.AddScoped<AIConversationContext>(); // 
// Blazor Authentication
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
provider.GetRequiredService<JwtAuthenticationStateProvider>());
// Blazor Components
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor
builder.Services.AddMudServices();

builder.Services.AddApexCharts();// chat regiseter
//Service Notifications // Notification Framework
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ConfirmationService>();

// Build App
var app = builder.Build();
// Database Seeder
// Database Seeder
using (var scope = app.Services.CreateScope())
{
    try
    {
        var factory =
            scope.ServiceProvider
                .GetRequiredService<IDbContextFactory<AppDbContext>>();

        await using var db =
            await factory.CreateDbContextAsync();

        await DbSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"Database seeding failed: {ex.Message}");
    }
}
// HTTP Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
