using System;
using System.IO;
using System.Runtime.InteropServices;
using NAudio.Wave;

namespace MP3_Project
{
    // Lightweight wrapper around NAudio for playback with robust error handling.
    public class AudioPlayer : IDisposable
    {
        private IWavePlayer? outputDevice;
        private AudioFileReader? audioFile;
        private readonly object syncLock = new object();

        public event Action<TimeSpan, TimeSpan>? PlaybackProgressChanged;
        public event Action? PlaybackStopped;

        public bool IsPlaying { get; private set; }

        public void Play(string filePath)
        {
            lock (syncLock)
            {
                try
                {
                    Stop();

                    // Validate file exists
                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException($"Audio file not found: {filePath}");
                    }

                    // Create audio file reader with proper error handling
                    try
                    {
                        audioFile = new AudioFileReader(filePath);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Failed to read audio file format: {ex.Message}", ex);
                    }

                    // Try to initialize output device with fallback
                    try
                    {
                        outputDevice = new WaveOutEvent();
                        outputDevice.Init(audioFile);
                    }
                    catch (COMException)
                    {
                        // If WaveOutEvent fails due to COM/audio device issues, try DirectSoundOut
                        outputDevice?.Dispose();
                        try
                        {
                            outputDevice = new DirectSoundOut();
                            outputDevice.Init(audioFile);
                        }
                        catch
                        {
                            // Last resort - try WaveOutEvent again with fresh initialization
                            outputDevice?.Dispose();
                            outputDevice = new WaveOutEvent();
                            outputDevice.Init(audioFile);
                        }
                    }

                    outputDevice.PlaybackStopped += OnPlaybackStopped;
                    outputDevice.Play();
                    IsPlaying = true;
                }
                catch (Exception ex)
                {
                    Stop();
                    throw new InvalidOperationException($"Failed to play audio file: {ex.Message}", ex);
                }
            }
        }

        public void Pause()
        {
            try
            {
                if (outputDevice == null) return;
                if (IsPlaying)
                {
                    outputDevice.Pause();
                    IsPlaying = false;
                }
                else
                {
                    outputDevice.Play();
                    IsPlaying = true;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to pause/resume playback: {ex.Message}", ex);
            }
        }

        public void Stop()
        {
            try
            {
                if (outputDevice != null)
                {
                    try
                    {
                        outputDevice.Stop();
                    }
                    catch { }

                    try
                    {
                        outputDevice.PlaybackStopped -= OnPlaybackStopped;
                    }
                    catch { }

                    try
                    {
                        outputDevice.Dispose();
                    }
                    catch { }

                    outputDevice = null;
                }

                if (audioFile != null)
                {
                    try
                    {
                        audioFile.Dispose();
                    }
                    catch { }

                    audioFile = null;
                }

                IsPlaying = false;
            }
            catch { }
        }

        public void SetVolume(float volume)
        {
            if (audioFile != null) audioFile.Volume = volume;
        }

        public void Seek(TimeSpan pos)
        {
            try
            {
                if (audioFile != null) 
                {
                    if (pos < TimeSpan.Zero) pos = TimeSpan.Zero;
                    if (pos > audioFile.TotalTime) pos = audioFile.TotalTime;
                    audioFile.CurrentTime = pos;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to seek: {ex.Message}", ex);
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            IsPlaying = false;
            PlaybackStopped?.Invoke();
        }

        public TimeSpan CurrentTime => audioFile?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalTime => audioFile?.TotalTime ?? TimeSpan.Zero;

        public void Dispose()
        {
            Stop();
        }
    }
}
