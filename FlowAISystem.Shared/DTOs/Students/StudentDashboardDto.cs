//namespace FlowAISystem.Shared.DTOs.Students;

//public class StudentDashboardDto
//{
//    public string StudentNumber { get; set; } = string.Empty;

//    public string FirstName { get; set; } = string.Empty;

//    public string LastName { get; set; } = string.Empty;

//    public string DepartmentName { get; set; } = string.Empty;

//    public DateOnly EnrollmentDate { get; set; }

//    public string? ProfileImage { get; set; }

//    public string Username { get; set; } = string.Empty;

//    public bool AccountActive { get; set; }
//}

namespace FlowAISystem.Shared.DTOs.Students;

public class StudentDashboardDto
{
    // =====================================================
    // Student Information
    // =====================================================

    public string StudentNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public DateOnly EnrollmentDate { get; set; }

    public string? ProfileImage { get; set; }

    public string Username { get; set; } = string.Empty;

    public bool AccountActive { get; set; }

    // =====================================================
    // Dashboard Summary
    // =====================================================

    public int CurrentSubjects { get; set; }

    public decimal CurrentGPA { get; set; }

    public decimal AttendancePercentage { get; set; }

    public int AnnouncementCount { get; set; }

    // =====================================================
    // Dashboard Lists
    // =====================================================

    public List<StudentSubjectSummaryDto> RecentSubjects { get; set; }
        = new();

    public List<StudentAnnouncementSummaryDto> RecentAnnouncements { get; set; }
        = new();
}