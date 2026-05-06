namespace MP3_Project
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            songtrackBar = new TrackBar();
            PausePlayButton = new Button();
            songList = new ListBox();
            currentSong = new Label();
            currentSongArtist = new Label();
            songTime = new Label();
            previousSongButton = new Button();
            nextSongButton = new Button();
            addSongButton = new Button();
            removeSongButton = new Button();
            shuffleButton = new Button();
            repeatButton = new Button();
            volumeTrackBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)songtrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)volumeTrackBar).BeginInit();
            SuspendLayout();
            // 
            // songtrackBar
            // 
            songtrackBar.Location = new Point(12, 256);
            songtrackBar.Name = "songtrackBar";
            songtrackBar.Size = new Size(370, 45);
            songtrackBar.TabIndex = 0;
            // 
            // PausePlayButton
            // 
            PausePlayButton.Location = new Point(154, 329);
            PausePlayButton.Name = "PausePlayButton";
            PausePlayButton.Size = new Size(75, 23);
            PausePlayButton.TabIndex = 1;
            PausePlayButton.Text = "Play";
            PausePlayButton.UseVisualStyleBackColor = true;
            PausePlayButton.Click += button1_Click;
            // 
            // songList
            // 
            songList.FormattingEnabled = true;
            songList.Location = new Point(27, 12);
            songList.Name = "songList";
            songList.Size = new Size(184, 229);
            songList.TabIndex = 2;
            // 
            // currentSong
            // 
            currentSong.AutoSize = true;
            currentSong.Location = new Point(257, 12);
            currentSong.Name = "currentSong";
            currentSong.Size = new Size(72, 15);
            currentSong.TabIndex = 3;
            currentSong.Text = "currentSong";
            currentSong.TextAlign = ContentAlignment.TopCenter;
            // 
            // currentSongArtist
            // 
            currentSongArtist.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            currentSongArtist.AutoSize = true;
            currentSongArtist.Location = new Point(277, 38);
            currentSongArtist.Name = "currentSongArtist";
            currentSongArtist.Size = new Size(33, 15);
            currentSongArtist.TabIndex = 4;
            currentSongArtist.Text = "artist";
            currentSongArtist.TextAlign = ContentAlignment.TopRight;
            currentSongArtist.Click += currentSongArtist_Click;
            // 
            // songTime
            // 
            songTime.AutoSize = true;
            songTime.Location = new Point(173, 286);
            songTime.Name = "songTime";
            songTime.Size = new Size(38, 15);
            songTime.TabIndex = 5;
            songTime.Text = "label1";
            songTime.TextAlign = ContentAlignment.TopCenter;
            songTime.Click += songTime_Click;
            // 
            // previousSongButton
            // 
            previousSongButton.Location = new Point(73, 329);
            previousSongButton.Name = "previousSongButton";
            previousSongButton.Size = new Size(75, 23);
            previousSongButton.TabIndex = 6;
            previousSongButton.Text = "Previous";
            previousSongButton.UseVisualStyleBackColor = true;
            // 
            // nextSongButton
            // 
            nextSongButton.Location = new Point(235, 329);
            nextSongButton.Name = "nextSongButton";
            nextSongButton.Size = new Size(75, 23);
            nextSongButton.TabIndex = 7;
            nextSongButton.Text = "Next";
            nextSongButton.UseVisualStyleBackColor = true;
            nextSongButton.Click += NextSongButton_Click;
            // 
            // addSongButton
            // 
            addSongButton.Location = new Point(20, 370);
            addSongButton.Name = "addSongButton";
            addSongButton.Size = new Size(75, 23);
            addSongButton.TabIndex = 8;
            addSongButton.Text = "Add Song";
            addSongButton.UseVisualStyleBackColor = true;
            // 
            // removeSongButton
            // 
            removeSongButton.Location = new Point(101, 370);
            removeSongButton.Name = "removeSongButton";
            removeSongButton.Size = new Size(110, 23);
            removeSongButton.TabIndex = 9;
            removeSongButton.Text = "Remove Song";
            removeSongButton.UseVisualStyleBackColor = true;
            // 
            // shuffleButton
            // 
            shuffleButton.Location = new Point(217, 370);
            shuffleButton.Name = "shuffleButton";
            shuffleButton.Size = new Size(75, 23);
            shuffleButton.TabIndex = 10;
            shuffleButton.Text = "Shuffle";
            shuffleButton.UseVisualStyleBackColor = true;
            // 
            // repeatButton
            // 
            repeatButton.Location = new Point(298, 370);
            repeatButton.Name = "repeatButton";
            repeatButton.Size = new Size(75, 23);
            repeatButton.TabIndex = 11;
            repeatButton.Text = "Repeat";
            repeatButton.UseVisualStyleBackColor = true;
            // 
            // volumeTrackBar
            // 
            volumeTrackBar.Location = new Point(217, 205);
            volumeTrackBar.Maximum = 100;
            volumeTrackBar.Name = "volumeTrackBar";
            volumeTrackBar.Size = new Size(156, 45);
            volumeTrackBar.TabIndex = 12;
            volumeTrackBar.Value = 50;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 419);
            Controls.Add(volumeTrackBar);
            Controls.Add(repeatButton);
            Controls.Add(shuffleButton);
            Controls.Add(removeSongButton);
            Controls.Add(addSongButton);
            Controls.Add(nextSongButton);
            Controls.Add(previousSongButton);
            Controls.Add(songTime);
            Controls.Add(currentSongArtist);
            Controls.Add(currentSong);
            Controls.Add(songList);
            Controls.Add(PausePlayButton);
            Controls.Add(songtrackBar);
            Name = "Form1";
            Text = "MP3 Player";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)songtrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)volumeTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TrackBar songtrackBar;
        private Button PausePlayButton;
        private ListBox songList;
        private Label currentSong;
        private Label currentSongArtist;
        private Label songTime;
        private Button previousSongButton;
        private Button nextSongButton;
        private Button addSongButton;
        private Button removeSongButton;
        private Button shuffleButton;
        private Button repeatButton;
        private TrackBar volumeTrackBar;
    }
}
