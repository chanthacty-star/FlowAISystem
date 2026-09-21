namespace FlowAISystem.Shared.DTOs.Attendances;

public class AttendanceSearchDto
{
    public string? SearchTerm { get; set; }

    //public string? StudentNumber { get; set; } // Changed to StudentNumber
    public int? StudentId { get; set; } // Added StudentId filter

    public int? EnrollmentId { get; set; }

    // DateTime? allows clean binding with HTML <input type="date"> in Blazor
    public DateTime? AttendanceDate { get; set; }

    public int? CourseOfferingId { get; set; }
}