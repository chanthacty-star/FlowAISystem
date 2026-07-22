using ClosedXML.Excel;

using FlowAISystem.Application.Interfaces.Export;
using FlowAISystem.Shared.DTOs.Dashboard;
using FlowAISystem.Shared.DTOs.Reports;
using FlowAISystem.Shared.DTOs.Students;


namespace FlowAISystem.Infrastructure.Export.Excel;


public class ExcelReportGenerator : IExcelReportGenerator
{

    // ==========================================
    // Student Report
    // ==========================================

    public byte[] GenerateStudentReport(
        List<StudentListItemDto> students)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Students");


        // Title

        worksheet.Cell(1, 1).Value =
            "FlowAISystem Student Report";

        worksheet.Cell(1, 1)
            .Style.Font.Bold = true;

        worksheet.Cell(1, 1)
            .Style.Font.FontSize = 18;



        // Headers

        worksheet.Cell(3, 1).Value = "Student No";
        worksheet.Cell(3, 2).Value = "Full Name";
        worksheet.Cell(3, 3).Value = "Email";
        worksheet.Cell(3, 4).Value = "Department";
        worksheet.Cell(3, 5).Value = "Enrollment Date";


        var header =
            worksheet.Range(3, 1, 3, 5);


        header.Style.Font.Bold = true;

        header.Style.Fill.BackgroundColor =
            XLColor.LightBlue;


        header.Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;



        // Data

        int row = 4;


        foreach (var student in students)
        {
            worksheet.Cell(row, 1).Value =
                student.StudentNumber;


            worksheet.Cell(row, 2).Value =
                student.FullName;


            worksheet.Cell(row, 3).Value =
                student.Email;


            worksheet.Cell(row, 4).Value =
                student.DepartmentName;


            worksheet.Cell(row, 5).Value =
                student.EnrollmentDate.ToString();


            row++;
        }


        worksheet.Columns()
            .AdjustToContents();



        using var stream = new MemoryStream();


        workbook.SaveAs(stream);


        return stream.ToArray();
    }





    // ==========================================
    // Dashboard Summary Report
    // ==========================================

    public byte[] GenerateDashboardSummaryReport(
        DashboardSummaryDto summary)
    {

        using var workbook = new XLWorkbook();


        var worksheet =
            workbook.Worksheets.Add("Dashboard Summary");



        // Title

        worksheet.Cell(1, 1).Value =
            "FlowAISystem Dashboard Summary Report";


        worksheet.Cell(1, 1)
            .Style.Font.Bold = true;


        worksheet.Cell(1, 1)
            .Style.Font.FontSize = 18;



        // Headers

        worksheet.Cell(3, 1).Value = "Metric";

        worksheet.Cell(3, 2).Value = "Total";


        var header =
            worksheet.Range(3, 1, 3, 2);


        header.Style.Font.Bold = true;


        header.Style.Fill.BackgroundColor =
            XLColor.LightBlue;



        // Data

        var data = new Dictionary<string, int>
        {
            {
                "Students",
                summary.TotalStudents
            },

            {
                "Teachers",
                summary.TotalTeachers
            },

            {
                "Departments",
                summary.TotalDepartments
            },

            {
                "Subjects",
                summary.TotalSubjects
            },

            {
                "AI Knowledge",
                summary.TotalAIKnowledge
            },

            {
                "Users",
                summary.TotalUsers
            }
        };



        int row = 4;


        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value =
                item.Key;


            worksheet.Cell(row, 2).Value =
                item.Value;


            row++;
        }



        worksheet.Columns()
            .AdjustToContents();



        using var stream =
            new MemoryStream();


        workbook.SaveAs(stream);



        return stream.ToArray();
    }





    // ==========================================
    // AI Knowledge Report
    // ==========================================

    public byte[] GenerateAIKnowledgeReport(
        List<AIKnowledgeReportDto> knowledge)
    {

        using var workbook =
            new XLWorkbook();


        var worksheet =
            workbook.Worksheets.Add("AI Knowledge");



        // Title

        worksheet.Cell(1, 1).Value =
            "FlowAISystem AI Knowledge Report";


        worksheet.Cell(1, 1)
            .Style.Font.Bold = true;


        worksheet.Cell(1, 1)
            .Style.Font.FontSize = 18;



        // Headers

        worksheet.Cell(3, 1).Value =
            "Question";


        worksheet.Cell(3, 2).Value =
            "Category";


        worksheet.Cell(3, 3).Value =
            "Created Date";



        var header =
            worksheet.Range(3, 1, 3, 3);



        header.Style.Font.Bold = true;


        header.Style.Fill.BackgroundColor =
            XLColor.LightBlue;


        header.Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;



        // Data

        int row = 4;


        foreach (var item in knowledge)
        {

            worksheet.Cell(row, 1).Value =
                item.Question;


            worksheet.Cell(row, 2).Value =
                item.Category;


            worksheet.Cell(row, 3).Value =
                item.CreatedAt
                    .ToString("yyyy-MM-dd");



            row++;

        }



        worksheet.Columns()
            .AdjustToContents();



        using var stream =
            new MemoryStream();


        workbook.SaveAs(stream);



        return stream.ToArray();
    }

}