using System;
using System.IO;
using TagLib;

namespace MP3_Project
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; }
        public TimeSpan Duration { get; set; }

        public Song(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Title = Path.GetFileNameWithoutExtension(filePath);
            Artist = string.Empty;
            Duration = TimeSpan.Zero;
            LoadMetadata();
        }

        public void LoadMetadata()
        {
            try
            {
                var file = TagLib.File.Create(FilePath);

                // Try to get title from metadata, fall back to filename
                if (!string.IsNullOrEmpty(file.Tag.Title))
                {
                    Title = file.Tag.Title;
                }

                // Try to get artist from metadata
                if (file.Tag.FirstPerformer != null)
                {
                    Artist = file.Tag.FirstPerformer;
                }
                else if (file.Tag.Performers.Length > 0)
                {
                    Artist = string.Join(", ", file.Tag.Performers);
                }

                // Get duration
                Duration = file.Properties.Duration;
            }
            catch (Exception ex)
            {
                // If metadata extraction fails, keep defaults
                System.Diagnostics.Debug.WriteLine($"Error loading metadata for {FilePath}: {ex.Message}");
            }
        }
    }
}
