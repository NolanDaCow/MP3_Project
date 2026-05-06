using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MP3_Project
{
    public partial class Form1 : Form
    {
        private Playlist currentPlaylist;
        private AudioPlayer audioPlayer;
        private Timer progressTimer;
        private bool isShuffle = false;
        private bool isRepeat = false;
        private bool isTrackBarBeingDragged = false;
        private bool isNavigatingPlaylist = false;

        public Form1()
        {
            InitializeComponent();
            currentPlaylist = new Playlist();
            audioPlayer = new AudioPlayer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Load music from the music folder
                string musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "music");

                if (!Directory.Exists(musicPath))
                {
                    MessageBox.Show($"Music folder not found at: {musicPath}\n\nPlease ensure the music folder exists in the application directory.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                var songs = FileManager.LoadSongsFromDirectory(musicPath);

                if (songs.Count > 0)
                {
                    foreach (var song in songs)
                    {
                        currentPlaylist.AddSong(song);
                        songList.Items.Add(song.Title);
                    }
                }
                else if (Directory.Exists(musicPath))
                {
                    MessageBox.Show($"No supported audio files found in: {musicPath}\n\nSupported formats: .mp3, .ogg, .wav, .flac", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Setup progress timer
                progressTimer = new Timer();
                progressTimer.Interval = 100;
                progressTimer.Tick += ProgressTimer_Tick;

                // Setup trackbar
                songtrackBar.MouseDown += TrackBar_MouseDown;
                songtrackBar.MouseUp += TrackBar_MouseUp;

                // Setup event handlers
                audioPlayer.PlaybackStopped += AudioPlayer_PlaybackStopped;
                audioPlayer.PlaybackProgressChanged += AudioPlayer_PlaybackProgressChanged;

                // Wire button events
                PausePlayButton.Click += PausePlayButton_Click;
                nextSongButton.Click += NextSongButton_Click;
                previousSongButton.Click += PreviousSongButton_Click;
                addSongButton.Click += AddSongButton_Click;
                removeSongButton.Click += RemoveSongButton_Click;
                shuffleButton.Click += ShuffleButton_Click;
                repeatButton.Click += RepeatButton_Click;
                volumeTrackBar.ValueChanged += VolumeTrackBar_ValueChanged;
                songList.DoubleClick += SongList_DoubleClick;
                songList.SelectedIndexChanged += SongList_SelectedIndexChanged;

                // Set initial volume
                audioPlayer.SetVolume(volumeTrackBar.Value / 100f);

                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading songs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PausePlayButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (currentPlaylist.GetCurrent() == null && currentPlaylist.Songs.Count > 0)
                {
                    PlaySong(currentPlaylist.Songs[0]);
                }
                else if (audioPlayer.IsPlaying)
                {
                    audioPlayer.Pause();
                    PausePlayButton.Text = "Resume";
                }
                else
                {
                    var current = currentPlaylist.GetCurrent();
                    if (current != null)
                    {
                        if (audioPlayer.CurrentTime == TimeSpan.Zero)
                        {
                            PlaySong(current);
                        }
                        else
                        {
                            audioPlayer.Pause();
                        }
                    }
                    PausePlayButton.Text = "Pause";
                }
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing song: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NextSongButton_Click(object? sender, EventArgs e)
        {
            try
            {
                var nextSong = currentPlaylist.GetNext();
                if (nextSong != null)
                {
                    PlaySong(nextSong, updateIndex: true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing next song: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PreviousSongButton_Click(object? sender, EventArgs e)
        {
            try
            {

                var prevSong = currentPlaylist.GetPrevious();
                if (prevSong != null)
                {
                    PlaySong(prevSong, updateIndex: true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing previous song: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSongButton_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Filter = "Audio Files (*.mp3;*.ogg;*.wav;*.flac)|*.mp3;*.ogg;*.wav;*.flac|All Files (*.*)|*.*";
                    dialog.Multiselect = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var filePath in dialog.FileNames)
                        {
                            if (FileManager.ValidateFile(filePath))
                            {
                                var song = new Song(filePath);
                                currentPlaylist.AddSong(song);
                                songList.Items.Add(song.Title);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding songs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveSongButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (songList.SelectedIndex >= 0 && songList.SelectedIndex < currentPlaylist.Songs.Count)
                {
                    var songToRemove = currentPlaylist.Songs[songList.SelectedIndex];
                    currentPlaylist.RemoveSong(songToRemove);
                    songList.Items.RemoveAt(songList.SelectedIndex);
                    UpdateUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing song: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShuffleButton_Click(object? sender, EventArgs e)
        {
            isShuffle = !isShuffle;
            shuffleButton.BackColor = isShuffle ? SystemColors.Highlight : SystemColors.Control;
            shuffleButton.ForeColor = isShuffle ? Color.White : SystemColors.ControlText;
        }

        private void RepeatButton_Click(object? sender, EventArgs e)
        {
            isRepeat = !isRepeat;
            repeatButton.BackColor = isRepeat ? SystemColors.Highlight : SystemColors.Control;
            repeatButton.ForeColor = isRepeat ? Color.White : SystemColors.ControlText;
        }

        private void VolumeTrackBar_ValueChanged(object? sender, EventArgs e)
        {
            audioPlayer.SetVolume(volumeTrackBar.Value / 100f);
        }

        private void SongList_DoubleClick(object? sender, EventArgs e)
        {
            try
            {
                if (songList.SelectedIndex >= 0 && songList.SelectedIndex < currentPlaylist.Songs.Count)
                {
                    var song = currentPlaylist.Songs[songList.SelectedIndex];
                    PlaySong(song);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing song: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SongList_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                if (songList.SelectedIndex >= 0 && songList.SelectedIndex < currentPlaylist.Songs.Count)
                {
                    // Update the playlist current song to match the selected item
                    currentPlaylist.SetCurrentIndex(songList.SelectedIndex);
                }
            }
            catch
            {
                // ignore selection errors
            }

            UpdateUI();
        }

        private void TrackBar_MouseDown(object? sender, MouseEventArgs e)
        {
            isTrackBarBeingDragged = true;
        }

        private void TrackBar_MouseUp(object? sender, MouseEventArgs e)
        {
            isTrackBarBeingDragged = false;
            if (audioPlayer.TotalTime.TotalSeconds > 0)
            {
                double seekPosition = (songtrackBar.Value / (double)songtrackBar.Maximum) * audioPlayer.TotalTime.TotalSeconds;
                audioPlayer.Seek(TimeSpan.FromSeconds(seekPosition));
            }
        }

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            UpdateProgressBar();
        }

        private void AudioPlayer_PlaybackStopped()
        {
            System.Diagnostics.Debug.WriteLine(">>> AudioPlayer_PlaybackStopped called");

            if (isRepeat)
            {
                var current = currentPlaylist.GetCurrent();
                if (current != null)
                {
                    System.Diagnostics.Debug.WriteLine($">>> AudioPlayer_PlaybackStopped: repeat mode, playing current={current.Title}");
                    PlaySong(current, updateIndex: false);
                }
            }
            else
            {
                // Automatically play the next song when current one finishes
                var nextSong = currentPlaylist.GetNext();
                System.Diagnostics.Debug.WriteLine($">>> AudioPlayer_PlaybackStopped: auto-advance mode, got nextSong={nextSong?.Title}");

                if (nextSong != null)
                {
                    PlaySong(nextSong, updateIndex: true);
                }
                else
                {
                    audioPlayer.Stop();
                    progressTimer.Stop();
                    UpdateUI();
                }
            }
        }

        private void AudioPlayer_PlaybackProgressChanged(TimeSpan current, TimeSpan total)
        {
            // Handled by timer
        }

        private void PlaySong(Song song, bool updateIndex = true)
        {
            try
            {
                // Validate song and file path
                if (song == null)
                {
                    MessageBox.Show("No song selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(song.FilePath) || !File.Exists(song.FilePath))
                {
                    MessageBox.Show($"File not found: {song.FilePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update the playlist's current song
                if (updateIndex)
                {
                    currentPlaylist.SetCurrentSong(song);
                }

                audioPlayer.Play(song.FilePath);
                progressTimer.Start();
                PausePlayButton.Text = "Pause";
                UpdateUI();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Playback error: {ex.InnerException?.Message ?? ex.Message}\n\nTechnical details: {ex.Message}", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing song: {ex.GetType().Name}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProgressBar()
        {
            if (!isTrackBarBeingDragged)
            {
                if (audioPlayer.TotalTime.TotalSeconds > 0)
                {
                    songtrackBar.Maximum = 100;
                    songtrackBar.Value = (int)((audioPlayer.CurrentTime.TotalSeconds / audioPlayer.TotalTime.TotalSeconds) * 100);
                }

                songTime.Text = $"{audioPlayer.CurrentTime:mm\\:ss} / {audioPlayer.TotalTime:mm\\:ss}";
            }
        }

        private void UpdateUI()
        {
            var current = currentPlaylist.GetCurrent();
            if (current != null)
            {
                currentSong.Text = current.Title;
                // If no artist metadata, show filename or a default
                if (string.IsNullOrEmpty(current.Artist))
                {
                    currentSongArtist.Text = Path.GetFileName(current.FilePath);
                }
                else
                {
                    currentSongArtist.Text = current.Artist;
                }

                // Find index in the songs list
                for (int i = 0; i < currentPlaylist.Songs.Count; i++)
                {
                    if (currentPlaylist.Songs[i] == current)
                    {
                        if (i >= 0 && i < songList.Items.Count)
                        {
                            songList.SelectedIndex = i;
                        }
                        break;
                    }
                }
            }
            else
            {
                currentSong.Text = "No Song Selected";
                currentSongArtist.Text = "";
            }

            if (!audioPlayer.IsPlaying)
            {
                PausePlayButton.Text = "Play";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // This is wired to Play/Pause button in designer
        }

        private void songTime_Click(object sender, EventArgs e)
        {
            // Label click event
        }

        private void currentSongArtist_Click(object sender, EventArgs e)
        {
            var current = currentPlaylist.GetCurrent();
        }
    }
}
