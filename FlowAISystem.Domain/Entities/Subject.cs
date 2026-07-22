using FlowAISystem.Domain.Common;
using FlowAISystem.Domain.Entities;


namespace FlowAISystem.Domain.Entities;

public class Subject : BaseEntity
{

    public string Code { get; set; }
        = string.Empty;


    public string Name { get; set; }
        = string.Empty;


    public int Credits { get; set; }



    public int DepartmentId { get; set; }

    public Department? Department { get; set; }



    public ICollection<CourseOffering> CourseOfferings { get; set; }
        = new List<CourseOffering>();



    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();



}