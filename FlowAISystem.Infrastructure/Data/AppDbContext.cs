using FlowAISystem.Domain.Entities;
using FlowAISystem.Domain.Entities.AI;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    // ==================================================
    // Core
    // ==================================================

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Teacher> Teachers => Set<Teacher>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Subject> Subjects => Set<Subject>();


    // ==================================================
    // Academic
    // ==================================================

    public DbSet<CourseOffering> CourseOfferings
        => Set<CourseOffering>();

    public DbSet<Enrollment> Enrollments
        => Set<Enrollment>();

    public DbSet<Score> Scores
        => Set<Score>();

    public DbSet<Attendance> Attendances
        => Set<Attendance>();

    public DbSet<Announcement> Announcements
        => Set<Announcement>();

    public DbSet<AcademicYear> AcademicYears
        => Set<AcademicYear>();

    public DbSet<Semester> Semesters
        => Set<Semester>();

    public DbSet<Feedback> Feedbacks
        => Set<Feedback>();


    // ==================================================
    // Prediction / Training
    // ==================================================

    public DbSet<Prediction> Predictions
        => Set<Prediction>();

    public DbSet<TrainingData> TrainingData
        => Set<TrainingData>();


    // ==================================================
    // Student AI Conversation
    // ==================================================

    public DbSet<AIConversation> AIConversations
        => Set<AIConversation>();

    public DbSet<AIMessage> AIMessages
        => Set<AIMessage>();


    // ==================================================
    // AI Knowledge
    // ==================================================

    public DbSet<LessonKnowledge> LessonKnowledges
        => Set<LessonKnowledge>();

    public DbSet<AIKnowledge> AIKnowledge
        => Set<AIKnowledge>();

    public DbSet<AICategory> AICategories
        => Set<AICategory>();


    // ==================================================
    // Model Configuration
    // ==================================================

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // ==================================================
        // AI Category -> AI Knowledge
        // ==================================================

        modelBuilder.Entity<AIKnowledge>()
            .HasOne(x => x.Category)
            .WithMany(x => x.KnowledgeItems)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);


        // ==================================================
        // AI Conversation -> AI Messages
        // ==================================================

        modelBuilder.Entity<AIConversation>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);


        // ==================================================
        // Entity Configurations
        // ==================================================

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}