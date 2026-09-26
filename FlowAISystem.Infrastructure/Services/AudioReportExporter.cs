using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FlowAISystem.Infrastructure.Services;

public static class AudioReportExporter
{
    // --- 1. CSV EXPORT ---
    public static byte[] GenerateCsvReport(AudioDurationStats stats, List<AudioRegion> regions)
    {
        var sb = new StringBuilder();

        // Summary Section
        sb.AppendLine("AUDIO COMPOSITION SUMMARY");
        sb.AppendLine($"Total Duration (s),{stats.TotalDuration.TotalSeconds}");
        sb.AppendLine($"Speech Duration (s),{stats.SpeechDuration.TotalSeconds}");
        sb.AppendLine($"Music Only Duration (s),{stats.MusicOnlyDuration.TotalSeconds}");
        sb.AppendLine($"Speech Ratio (%),{stats.SpeechPercentage}%");
        sb.AppendLine($"Music Ratio (%),{stats.MusicOnlyPercentage}%");
        sb.AppendLine();

        // Timestamps Table
        sb.AppendLine("DUCKING TIMESTAMPS");
        sb.AppendLine("Region ID,Start Time (s),End Time (s),Duration (s),Label");

        for (int i = 0; i < regions.Count; i++)
        {
            var r = regions[i];
            double duration = Math.Round(r.End - r.Start, 2);
            sb.AppendLine($"{i + 1},{r.Start},{r.End},{duration},\"{r.Label}\"");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // --- 2. PDF EXPORT (Requires QuestPDF NuGet package) ---
    public static byte[] GeneratePdfReport(AudioDurationStats stats, List<AudioRegion> regions)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                //page.Margin(2, Unit.Centimeter);
                // To this:
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                // Header
                page.Header().Text("Audio Composition & Ducking Report")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                // Body Content
                page.Content().PaddingVertical(2, Unit.Centimetre).Column(col =>
                {
                    col.Spacing(15);

                    // Summary Table
                    col.Item().Text("Composition Analytics").Bold().FontSize(14);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().Text("Total Duration:").Bold();
                        table.Cell().Text($"{stats.TotalDuration:mm\\:ss}");

                        table.Cell().Text("Speech / Ducked Time:").Bold();
                        table.Cell().Text($"{stats.SpeechDuration:mm\\:ss} ({stats.SpeechPercentage}%)");

                        table.Cell().Text("Background Music Only:").Bold();
                        table.Cell().Text($"{stats.MusicOnlyDuration:mm\\:ss} ({stats.MusicOnlyPercentage}%)");
                    });

                    // Ducking Timestamps Table
                    col.Item().Text("Ducking Region Details").Bold().FontSize(14);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("#").Bold();
                            header.Cell().Text("Start").Bold();
                            header.Cell().Text("End").Bold();
                            header.Cell().Text("Duration").Bold();
                        });

                        for (int i = 0; i < regions.Count; i++)
                        {
                            var r = regions[i];
                            table.Cell().Text((i + 1).ToString());
                            table.Cell().Text($"{r.Start}s");
                            table.Cell().Text($"{r.End}s");
                            table.Cell().Text($"{Math.Round(r.End - r.Start, 2)}s");
                        }
                    });
                });

                // Footer
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        });

        return document.GeneratePdf();
    }
}