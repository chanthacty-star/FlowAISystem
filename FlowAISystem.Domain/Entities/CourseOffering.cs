using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class CourseOffering : BaseEntity
{
    public string ClassName { get; set; } = string.Empty;


    public int Capacity { get; set; }


    public string? Room { get; set; }



    // Subject

    public int SubjectId { get; set; }

    public Subject? Subject { get; set; }



    // Teacher

    public int TeacherId { get; set; }

    public Teacher? Teacher { get; set; }



    // Semester

    public int SemesterId { get; set; }

    public Semester? Semester { get; set; }



    // Enrollment

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}