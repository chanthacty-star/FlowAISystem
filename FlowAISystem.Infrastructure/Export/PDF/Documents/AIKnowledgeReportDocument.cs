using FlowAISystem.Shared.DTOs.Reports;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FlowAISystem.Infrastructure.Export.PDF.Documents;

public class AIKnowledgeReportDocument : IDocument
{
    private readonly List<AIKnowledgeReportDto> _knowledge;

    public AIKnowledgeReportDocument(
        List<AIKnowledgeReportDto> knowledge)
    {
        _knowledge = knowledge;
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
                .Text("FlowAISystem - AI Knowledge Report")
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
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });


                    table.Header(header =>
                    {
                        header.Cell()
                            .Text("Question")
                            .Bold();

                        header.Cell()
                            .Text("Category")
                            .Bold();

                        header.Cell()
                            .Text("Created Date")
                            .Bold();
                    });



                    foreach (var item in _knowledge)
                    {
                        table.Cell()
                            .Text(item.Question);

                        table.Cell()
                            .Text(item.Category);

                        table.Cell()
                            .Text(
                                item.CreatedAt.ToString("yyyy-MM-dd")
                            );
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