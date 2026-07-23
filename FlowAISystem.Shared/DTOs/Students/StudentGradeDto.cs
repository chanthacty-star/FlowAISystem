namespace FlowAISystem.Shared.DTOs.Students;

public class StudentGradeDto
{
    public int SubjectId { get; set; }


    public string SubjectCode { get; set; }
        = string.Empty;


    public string SubjectName { get; set; }
        = string.Empty;


    public string SemesterName { get; set; }
        = string.Empty;



    // Assessment

    public string AssessmentName { get; set; }
        = string.Empty;


    public decimal Marks { get; set; }


    public decimal MaxMarks { get; set; }


    public decimal Percentage
    {
        get
        {
            if (MaxMarks == 0)
                return 0;

            return Marks / MaxMarks * 100;
        }
    }



    // Calculated Grade

    public string LetterGrade
    {
        get
        {
            if (Percentage >= 90)
                return "A";

            if (Percentage >= 80)
                return "B";

            if (Percentage >= 70)
                return "C";

            if (Percentage >= 60)
                return "D";

            return "F";
        }
    }
}