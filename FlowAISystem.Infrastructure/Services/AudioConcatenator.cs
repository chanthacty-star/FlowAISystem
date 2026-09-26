using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace FlowAISystem.Infrastructure.Services;

public class AudioRegion
{
    public double Start { get; set; }
    public double End { get; set; }
    public string Label { get; set; } = "Speech / Ducked";
    public string Color { get; set; } = "rgba(239, 83, 80, 0.35)"; // Highlight color overlay
}

public class AudioDurationStats
{
    public TimeSpan TotalDuration { get; set; }
    public TimeSpan SpeechDuration { get; set; }
    public TimeSpan MusicOnlyDuration { get; set; }
    public double SpeechPercentage { get; set; }
    public double MusicOnlyPercentage { get; set; }
}

public static partial class AudioConcatenator
{
    /// <summary>
    /// Mixes narration audio with a background music track applying dynamic audio ducking via exponential smoothing.
    /// </summary>
    public static async Task MixWithDuckingAsync(
        string narrationFilePath,
        string backgroundFilePath,
        string outputPath,
        float normalMusicVolume = 0.30f,
        float duckedMusicVolume = 0.06f,
        float thresholdDb = -32.0f)
    {
        await Task.Run(() =>
        {
            using var narrationReader = new AudioFileReader(narrationFilePath);
            using var backgroundReader = new AudioFileReader(backgroundFilePath);

            var narrationProvider = narrationReader.ToSampleProvider();
            var musicProvider = backgroundReader.ToSampleProvider();

            // Wrap music provider in dynamic ducking provider linked to narration input
            // CORRECT: Matches constructor parameter names ("narration" and "music")
            var duckedMusic = new AudioDuckingSampleProvider(
                narration: narrationProvider,
                music: musicProvider,
                normalVolume: normalMusicVolume,
                duckedVolume: duckedMusicVolume,
                thresholdDb: thresholdDb
            );

            // Mix narration and ducked background music together
            var mixer = new MixingSampleProvider(new[] { narrationProvider, duckedMusic });
            var masterProvider = mixer.Take(narrationReader.TotalTime);

            WaveFileWriter.CreateWaveFile16(outputPath, masterProvider);
        });
    }

    /// <summary>
    /// Analyzes the narration audio track to find timestamps where ducking will trigger.
    /// </summary>
    public static List<AudioRegion> DetectSpeechRegions(string narrationFilePath, float thresholdDb = -32.0f)
    {
        var regions = new List<AudioRegion>();
        using var reader = new AudioFileReader(narrationFilePath);

        float thresholdLinear = (float)Math.Pow(10, thresholdDb / 20.0);
        int sampleRate = reader.WaveFormat.SampleRate;
        int channels = reader.WaveFormat.Channels;

        // Process in 50ms blocks
        int blockSize = (int)(sampleRate * channels * 0.05);
        float[] buffer = new float[blockSize];

        bool inSpeech = false;
        double speechStart = 0;
        double totalTime = 0;

        int samplesRead;
        while ((samplesRead = reader.Read(buffer, 0, blockSize)) > 0)
        {
            float maxVal = 0f;
            for (int i = 0; i < samplesRead; i++)
            {
                float absVal = Math.Abs(buffer[i]);
                if (absVal > maxVal) maxVal = absVal;
            }

            double blockDuration = (double)samplesRead / (sampleRate * channels);

            if (maxVal >= thresholdLinear)
            {
                if (!inSpeech)
                {
                    inSpeech = true;
                    speechStart = totalTime;
                }
            }
            else
            {
                if (inSpeech)
                {
                    inSpeech = false;
                    regions.Add(new AudioRegion
                    {
                        Start = Math.Round(speechStart, 2),
                        End = Math.Round(totalTime, 2),
                        Label = "Ducked"
                    });
                }
            }

            totalTime += blockDuration;
        }

        if (inSpeech)
        {
            regions.Add(new AudioRegion
            {
                Start = Math.Round(speechStart, 2),
                End = Math.Round(totalTime, 2),
                Label = "Ducked"
            });
        }

        return regions;
    }

    /// <summary>
    /// Calculates audio stats based on detected ducking speech regions and overall audio duration.
    /// </summary>
    public static AudioDurationStats CalculateDurationStats(string outputPath, List<AudioRegion> speechRegions)
    {
        using var reader = new AudioFileReader(outputPath);
        TimeSpan totalDuration = reader.TotalTime;

        double totalSpeechSeconds = speechRegions.Sum(r => r.End - r.Start);
        TimeSpan speechDuration = TimeSpan.FromSeconds(Math.Min(totalSpeechSeconds, totalDuration.TotalSeconds));

        TimeSpan musicOnlyDuration = totalDuration - speechDuration;
        if (musicOnlyDuration < TimeSpan.Zero) musicOnlyDuration = TimeSpan.Zero;

        double totalSecs = totalDuration.TotalSeconds > 0 ? totalDuration.TotalSeconds : 1;

        return new AudioDurationStats
        {
            TotalDuration = totalDuration,
            SpeechDuration = speechDuration,
            MusicOnlyDuration = musicOnlyDuration,
            SpeechPercentage = Math.Round((speechDuration.TotalSeconds / totalSecs) * 100, 1),
            MusicOnlyPercentage = Math.Round((musicOnlyDuration.TotalSeconds / totalSecs) * 100, 1)
        };
    }

    /// <summary>
    /// Custom NAudio ISampleProvider that dynamically attenuates music volume based on narration energy.
    /// </summary>
    private class AudioDuckingSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider _narration;
        private readonly ISampleProvider _music;
        private readonly float _normalVolume;
        private readonly float _duckedVolume;
        private readonly float _thresholdLinear;

        private float _currentVolume;
        private readonly float _attackCoeff;
        private readonly float _releaseCoeff;

        public WaveFormat WaveFormat => _music.WaveFormat;

        public AudioDuckingSampleProvider(
            ISampleProvider narration,
            ISampleProvider music,
            float normalVolume = 0.30f,
            float duckedVolume = 0.06f,
            float thresholdDb = -32.0f,
            double attackTimeMs = 30.0,
            double releaseTimeMs = 300.0)
        {
            _narration = narration;
            _music = music;
            _normalVolume = normalVolume;
            _duckedVolume = duckedVolume;
            _currentVolume = normalVolume;

            _thresholdLinear = (float)Math.Pow(10, thresholdDb / 20.0);

            int sampleRate = WaveFormat.SampleRate;
            _attackCoeff = (float)Math.Exp(-1.0 / (sampleRate * (attackTimeMs / 1000.0)));
            _releaseCoeff = (float)Math.Exp(-1.0 / (sampleRate * (releaseTimeMs / 1000.0)));
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = _music.Read(buffer, offset, count);
            if (samplesRead == 0) return 0;

            float[] speechBuffer = new float[samplesRead];
            int speechSamples = _narration.Read(speechBuffer, 0, samplesRead);

            for (int i = 0; i < samplesRead; i++)
            {
                float speechSample = i < speechSamples ? Math.Abs(speechBuffer[i]) : 0f;
                float targetVolume = (speechSample > _thresholdLinear) ? _duckedVolume : _normalVolume;

                float coeff = (targetVolume < _currentVolume) ? _attackCoeff : _releaseCoeff;
                _currentVolume = (coeff * _currentVolume) + ((1.0f - coeff) * targetVolume);

                buffer[offset + i] *= _currentVolume;
            }

            return samplesRead;
        }
    }
}