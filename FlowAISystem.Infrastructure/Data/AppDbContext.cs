using FlowAISystem.Domain.Entities.AI; // student AI
using FlowAISystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Teacher> Teachers => Set<Teacher>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Subject> Subjects => Set<Subject>();

    public DbSet<CourseOffering> CourseOfferings { get; set; }// course ofering 

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    public DbSet<Score> Scores => Set<Score>();// score

    public DbSet<Attendance> Attendances { get; set; } // Atencdance 

    public DbSet<Announcement> Announcements => Set<Announcement>(); // nnouncements

    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();

    public DbSet<Semester> Semesters => Set<Semester>();

    public DbSet<Feedback> Feedbacks => Set<Feedback>();

    public DbSet<Prediction> Predictions => Set<Prediction>();

    public DbSet<TrainingData> TrainingData => Set<TrainingData>();


    public DbSet<AIConversation> AIConversations { get; set; }


    public DbSet<AIMessage> AIMessages { get; set; }




    // AI System
    public DbSet<LessonKnowledge> LessonKnowledges
    => Set<LessonKnowledge>();// teacher part
    public DbSet<AIKnowledge> AIKnowledge => Set<AIKnowledge>();

    public DbSet<AICategory> AICategories => Set<AICategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // AI Category -> AI Knowledge
        modelBuilder.Entity<AIKnowledge>()
            .HasOne(x => x.Category)
            .WithMany(x => x.KnowledgeItems)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

       // AI Conversation->AI Messages
        modelBuilder.Entity<AIConversation>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}