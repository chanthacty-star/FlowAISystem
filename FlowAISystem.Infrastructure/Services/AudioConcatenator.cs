using System.IO;
using System.Collections.Generic;
using FlowAISystem.Application.Common.Models;

public static class AudioConcatenator
{
    /// <summary>
    /// Concatenates multiple timeline segment audio byte arrays into a single master MP3 byte array.
    /// </summary>
    public static byte[] CombineAudioSegments(IEnumerable<TimelineSegment> segments)
    {
        using var masterStream = new MemoryStream();

        foreach (var segment in segments)
        {
            if (segment.SynthesizedAudio != null && segment.SynthesizedAudio.Length > 0)
            {
                // Write each segment's MP3 byte stream sequentially into the master buffer
                masterStream.Write(segment.SynthesizedAudio, 0, segment.SynthesizedAudio.Length);
            }
        }

        return masterStream.ToArray();
    }
}