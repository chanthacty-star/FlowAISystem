using FlowAISystem.Domain.Common;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Domain.Entities;

public class Attendance : BaseEntity
{

    public DateOnly AttendanceDate { get; set; }


    public AttendanceStatus Status { get; set; }
        = AttendanceStatus.Present;


    public string? Remark { get; set; }



    // Enrollment

    public int EnrollmentId { get; set; }


    public Enrollment? Enrollment { get; set; }

}