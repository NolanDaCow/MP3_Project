using System;
using System.Collections.Generic;
using System.IO;

namespace MP3_Project
{
    public class FileManager
    {
        private static readonly string[] SupportedFormats = { ".mp3", ".ogg", ".wav", ".flac" };

        public static List<Song> LoadSongsFromDirectory(string directoryPath)
        {
            var songs = new List<Song>();

            if (!Directory.Exists(directoryPath))
            {
                return songs;
            }

            try
            {
                foreach (var file in Directory.GetFiles(directoryPath))
                {
                    var ext = Path.GetExtension(file).ToLower();
                    if (Array.Exists(SupportedFormats, format => format == ext))
                    {
                        var song = new Song(file);
                        songs.Add(song);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle error silently
                Console.WriteLine($"Error loading songs from directory: {ex.Message}");
            }

            return songs;
        }

        public static bool ValidateFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            var ext = Path.GetExtension(filePath).ToLower();
            return Array.Exists(SupportedFormats, format => format == ext);
        }

        public static List<Song> LoadSongsFromFiles(params string[] filePaths)
        {
            var songs = new List<Song>();
            foreach (var filePath in filePaths)
            {
                if (ValidateFile(filePath))
                {
                    songs.Add(new Song(filePath));
                }
            }
            return songs;
        }
    }
}
