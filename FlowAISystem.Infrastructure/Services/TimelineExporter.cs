using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FlowAISystem.Application.Common.Models;

namespace FlowAISystem.Infrastructure.Services;

public static partial class TimelineExporter
{
    public static async Task<byte[]> ExportBundleToZipAsync(
        IReadOnlyList<WordBookmark> wordBookmarks,
        string projectName = "Narration_Project",
        double frameRate = 29.97)
    {
        if (wordBookmarks == null || !wordBookmarks.Any())
        {
            return Array.Empty<byte>();
        }

        string cleanName = SanitizeFileName(projectName);

        string jsonContent = ExportToJson(wordBookmarks, cleanName, frameRate);
        string srtContent = ToSrt(wordBookmarks, maxWordsPerCaption: 8);
        string vttContent = ToWebVtt(wordBookmarks, maxWordsPerCaption: 8);
        string fcpxmlContent = ToFcpxml(wordBookmarks, sequenceName: cleanName, frameRate: frameRate);
        string edlContent = ToEdl(wordBookmarks, title: cleanName, frameRate: frameRate, markerColor: "CYAN");

        using var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            await AddFileToArchiveAsync(archive, $"{cleanName}_timeline.json", jsonContent);
            await AddFileToArchiveAsync(archive, $"{cleanName}_subtitles.srt", srtContent);
            await AddFileToArchiveAsync(archive, $"{cleanName}_subtitles.vtt", vttContent);
            await AddFileToArchiveAsync(archive, $"{cleanName}_sequence.fcpxml", fcpxmlContent);
            await AddFileToArchiveAsync(archive, $"{cleanName}_markers.edl", edlContent);
        }

        return zipStream.ToArray();
    }

    public static async Task ExportBundleToZipFileAsync(
        IReadOnlyList<WordBookmark> wordBookmarks,
        string outputZipPath,
        string projectName = "Narration_Project",
        double frameRate = 29.97)
    {
        byte[] zipData = await ExportBundleToZipAsync(wordBookmarks, projectName, frameRate);
        await File.WriteAllBytesAsync(outputZipPath, zipData);
    }

    private static async Task AddFileToArchiveAsync(ZipArchive archive, string entryName, string content)
    {
        var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
        using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
        await writer.WriteAsync(content);
    }

    private static string SanitizeFileName(string name)
    {
        return string.Concat(name.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");
    }

    public static string ExportToJson(IReadOnlyList<WordBookmark> bookmarks, string projectName, double frameRate)
    {
        var lastBm = bookmarks.LastOrDefault();
        double totalDuration = lastBm != null ? (lastBm.Offset + lastBm.Duration).TotalSeconds : 0;

        var payload = new
        {
            Project = projectName,
            FrameRate = frameRate,
            TotalWords = bookmarks.Count,
            Duration = totalDuration,
            Bookmarks = bookmarks
        };

        return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
    }

    public static string ToSrt(IReadOnlyList<WordBookmark> bookmarks, int maxWordsPerCaption = 8)
    {
        var sb = new StringBuilder();
        int captionIndex = 1;

        for (int i = 0; i < bookmarks.Count; i += maxWordsPerCaption)
        {
            var chunk = bookmarks.Skip(i).Take(maxWordsPerCaption).ToList();
            if (!chunk.Any()) break;

            double startSec = chunk.First().Offset.TotalSeconds;
            double endSec = (chunk.Last().Offset + chunk.Last().Duration).TotalSeconds;

            sb.AppendLine(captionIndex.ToString());
            sb.AppendLine($"{FormatSrtTime(startSec)} --> {FormatSrtTime(endSec)}");
            sb.AppendLine(string.Join(" ", chunk.Select(w => w.Word)));
            sb.AppendLine();

            captionIndex++;
        }

        return sb.ToString();
    }

    public static string ToWebVtt(IReadOnlyList<WordBookmark> bookmarks, int maxWordsPerCaption = 8)
    {
        var sb = new StringBuilder();
        sb.AppendLine("WEBVTT - Narration Subtitles");
        sb.AppendLine();

        int captionIndex = 1;
        for (int i = 0; i < bookmarks.Count; i += maxWordsPerCaption)
        {
            var chunk = bookmarks.Skip(i).Take(maxWordsPerCaption).ToList();
            if (!chunk.Any()) break;

            double startSec = chunk.First().Offset.TotalSeconds;
            double endSec = (chunk.Last().Offset + chunk.Last().Duration).TotalSeconds;

            sb.AppendLine($"{captionIndex}");
            sb.AppendLine($"{FormatVttTime(startSec)} --> {FormatVttTime(endSec)}");
            sb.AppendLine(string.Join(" ", chunk.Select(w => w.Word)));
            sb.AppendLine();

            captionIndex++;
        }

        return sb.ToString();
    }

    public static string ToFcpxml(IReadOnlyList<WordBookmark> bookmarks, string sequenceName, double frameRate)
    {
        var lastBm = bookmarks.LastOrDefault();
        double totalSeconds = lastBm != null ? (lastBm.Offset + lastBm.Duration).TotalSeconds : 0;

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<!DOCTYPE fcpxml>");
        sb.AppendLine("<fcpxml version=\"1.9\">");
        sb.AppendLine("  <resources>");
        sb.AppendLine($"    <format id=\"r1\" name=\"FFVideoFormat1080p{Math.Round(frameRate)}\" frameDuration=\"100/{Math.Round(frameRate * 100)}s\"/>");
        sb.AppendLine("  </resources>");
        sb.AppendLine("  <library>");
        sb.AppendLine($"    <event name=\"{sequenceName}\">");
        sb.AppendLine($"      <project name=\"{sequenceName}\">");
        sb.AppendLine($"        <sequence format=\"r1\" duration=\"{FormatTimeDuration(totalSeconds)}\">");
        sb.AppendLine("          <spine>");

        foreach (var bm in bookmarks)
        {
            sb.AppendLine($"            <gap name=\"{bm.Word}\" start=\"{FormatTimeDuration(bm.Offset.TotalSeconds)}\" duration=\"{FormatTimeDuration(bm.Duration.TotalSeconds)}\"/>");
        }

        sb.AppendLine("          </spine>");
        sb.AppendLine("        </sequence>");
        sb.AppendLine("      </project>");
        sb.AppendLine("    </event>");
        sb.AppendLine("  </library>");
        sb.AppendLine("</fcpxml>");

        return sb.ToString();
    }

    public static string ToEdl(IReadOnlyList<WordBookmark> bookmarks, string title, double frameRate, string markerColor)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"TITLE: {title}");
        sb.AppendLine($"FCM: NON-DROP FRAME");
        sb.AppendLine();

        int eventNum = 1;
        foreach (var bm in bookmarks)
        {
            double startSec = bm.Offset.TotalSeconds;
            double endSec = (bm.Offset + bm.Duration).TotalSeconds;

            string timeIn = SecondsToTimecode(startSec, frameRate);
            string timeOut = SecondsToTimecode(endSec, frameRate);

            sb.AppendLine($"{eventNum,3:D3}  AX       V     C        {timeIn} {timeOut} {timeIn} {timeOut}");
            sb.AppendLine($"* FROM CLIP NAME: {bm.Word} [Color: {markerColor}]");
            sb.AppendLine();
            eventNum++;
        }

        return sb.ToString();
    }

    private static string FormatSrtTime(double seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);
        return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2},{ts.Milliseconds:D3}";
    }

    private static string FormatVttTime(double seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);
        return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}.{ts.Milliseconds:D3}";
    }

    private static string FormatTimeDuration(double seconds)
    {
        return $"{Math.Round(seconds * 1000)}s";
    }

    private static string SecondsToTimecode(double seconds, double frameRate)
    {
        int totalFrames = (int)Math.Round(seconds * frameRate);
        int fps = (int)Math.Round(frameRate);

        int frames = totalFrames % fps;
        int totalSecs = totalFrames / fps;
        int secs = totalSecs % 60;
        int totalMins = totalSecs / 60;
        int mins = totalMins % 60;
        int hours = totalMins / 60;

        return $"{hours:D2}:{mins:D2}:{secs:D2}:{frames:D2}";
    }
}