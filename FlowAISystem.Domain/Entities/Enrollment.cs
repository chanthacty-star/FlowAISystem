using FlowAISystem.Domain.Common;
using FlowAISystem.Shared.Enums;
namespace FlowAISystem.Domain.Entities;

public class Enrollment : BaseEntity
{
    public DateOnly EnrollmentDate { get; set; }


    public EnrollmentStatus Status { get; set; }
        = EnrollmentStatus.Active;



    // Student

    public int StudentId { get; set; }

    public Student? Student { get; set; }



    // Course Offering

    public int CourseOfferingId { get; set; }

    public CourseOffering? CourseOffering { get; set; }



    // Grades

    public ICollection<Score> Scores { get; set; }
        = new List<Score>();

    public ICollection<Feedback> Feedbacks { get; set; }
    = new List<Feedback>();

    // Atendence 
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}