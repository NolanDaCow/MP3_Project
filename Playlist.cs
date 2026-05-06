using System.Collections.Generic;

namespace MP3_Project
{
    public class Playlist
    {
        private readonly List<Song> songs = new List<Song>();
        private Song? currentlyPlayingSong = null;

        public IReadOnlyList<Song> Songs => songs.AsReadOnly();

        public void AddSong(Song s)
        {
            if (s == null) return;
            songs.Add(s);
            if (currentlyPlayingSong == null && songs.Count > 0)
                currentlyPlayingSong = songs[0];
        }

        public void RemoveSong(Song s)
        {
            if (s == null) return;
            var idx = songs.IndexOf(s);
            if (idx < 0) return;
            songs.RemoveAt(idx);
            if (songs.Count == 0) currentlyPlayingSong = null;
            else if (currentlyPlayingSong == s) currentlyPlayingSong = songs[0];
        }

        public Song? GetNext()
        {
            if (songs.Count == 0) return null;

            int currentIdx = songs.IndexOf(currentlyPlayingSong);
            // If current not found, start from first song
            if (currentIdx < 0)
                return songs[0];

            // Normal next: wrap to 0 after last
            if (currentIdx >= songs.Count - 1)
                return songs[0];

            return songs[currentIdx + 1];
        }

        public Song? GetPrevious()
        {
            if (songs.Count == 0) return null;

            int currentIdx = songs.IndexOf(currentlyPlayingSong);
            // If current not found, go to last song
            if (currentIdx < 0)
            {
                return songs[songs.Count - 1];
            }

            // Normal previous: wrap to last when at 0
            if (currentIdx == 0)
                return songs[songs.Count - 1];

            return songs[currentIdx - 1];
        }

        public Song? GetCurrent()
        {
            return currentlyPlayingSong;
        }

        public void SetCurrentSong(Song song)
        {
            if (song == null) return;
            var idx = songs.IndexOf(song);
            if (idx >= 0)
            {
                currentlyPlayingSong = song;
            }
        }

        // Set the current song by index. If index out of range, no-op.
        public void SetCurrentIndex(int index)
        {
            if (index >= 0 && index < songs.Count)
            {
                currentlyPlayingSong = songs[index];
            }
        }

        // Return the current index (or -1 if none)
        public int GetCurrentIndex()
        {
            if (currentlyPlayingSong == null) return -1;
            return songs.IndexOf(currentlyPlayingSong);
        }
    }
}
