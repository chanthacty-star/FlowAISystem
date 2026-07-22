using FlowAISystem.Shared.DTOs.Dashboard;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace FlowAISystem.Infrastructure.Export.PDF.Documents;


public class DashboardReportDocument : IDocument
{
    private readonly DashboardSummaryDto _summary;


    public DashboardReportDocument(
        DashboardSummaryDto summary)
    {
        _summary = summary;
    }


    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;



    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);

            page.Margin(30);


            page.Header()
                .Text("FlowAISystem - Dashboard Summary Report")
                .FontSize(20)
                .Bold();



            page.Content()
                .PaddingVertical(15)
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });


                    table.Header(header =>
                    {
                        header.Cell()
                            .Text("Metric")
                            .Bold();

                        header.Cell()
                            .Text("Total")
                            .Bold();
                    });


                    AddRow(
                        table,
                        "Students",
                        _summary.TotalStudents);


                    AddRow(
                        table,
                        "Teachers",
                        _summary.TotalTeachers);


                    AddRow(
                        table,
                        "Departments",
                        _summary.TotalDepartments);


                    AddRow(
                        table,
                        "Subjects",
                        _summary.TotalSubjects);


                    AddRow(
                        table,
                        "AI Knowledge",
                        _summary.TotalAIKnowledge);


                    AddRow(
                        table,
                        "Users",
                        _summary.TotalUsers);
                });



            page.Footer()
                .AlignCenter()
                .Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                });

        });
    }



    private static void AddRow(
        TableDescriptor table,
        string name,
        int value)
    {
        table.Cell()
            .Text(name);

        table.Cell()
            .Text(value.ToString());
    }
}