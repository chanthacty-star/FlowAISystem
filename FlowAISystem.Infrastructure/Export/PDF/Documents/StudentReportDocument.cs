using FlowAISystem.Shared.DTOs.Students;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FlowAISystem.Infrastructure.Export.PDF.Documents;

public class StudentReportDocument : IDocument
{
    private readonly List<StudentListItemDto> _students;

    public StudentReportDocument(
        List<StudentListItemDto> students)
    {
        _students = students;
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);

            page.Margin(30);

            // ==========================
            // Header
            // ==========================

            page.Header()
                .Text("FlowAISystem - Student Report")
                .FontSize(20)
                .Bold();

            // ==========================
            // Content
            // ==========================

            page.Content()
                .PaddingVertical(15)
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Student No").Bold();
                        header.Cell().Text("Name").Bold();
                        header.Cell().Text("Email").Bold();
                        header.Cell().Text("Department").Bold();
                    });

                    foreach (var student in _students)
                    {
                        table.Cell().Text(student.StudentNumber);
                        table.Cell().Text(student.FullName);
                        table.Cell().Text(student.Email);
                        table.Cell().Text(student.DepartmentName);
                    }
                });

            // ==========================
            // Footer
            // ==========================

            page.Footer()
                .AlignCenter()
                .Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                });
        });
    }
}