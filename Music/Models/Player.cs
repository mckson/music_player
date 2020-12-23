using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Music.Models
{
    public class Player : INotifyPropertyChanged
    {
        private Song song;
        private MediaPlayer player;
        private bool isPlaying;
        private Duration duration;
        //private double speedRatio;

        public event PropertyChangedEventHandler PropertyChanged;

        public Duration Duration 
        { 
            get => player.NaturalDuration;
            set
            {
                duration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Duration)));
            }
        }

        public TimeSpan Position
        {
            get => player.Position;
            set
            {
                player.Position = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
            }
        }

        public double Volume
        {
            get => player.Volume;
            set
            {
                if (value <= 1)
                {
                    player.Volume = value;
                }
            }
        }

        public Player(Song song, bool isPlaying, TimeSpan position)
        {
            player = new MediaPlayer();
            Song = song;
            IsPlaying = isPlaying;
            Position = position;
        }

        public Player(Song song)
        {
            IsPlaying = false;
            player = new MediaPlayer();
            this.song = song;
            OpenSong(this.song);
        }

        public void OpenSong(Song song)
        {
            if (song != null)
            {
                Song = song;
                Open(song.Path);

                if (IsPlaying)
                    Play();
            }
        }

        public void Play()
        {
            player.Play();
            IsPlaying = true;
        }

        public void Pause()
        {
            player.Pause();
            IsPlaying = false;
        }

        public void Open(string filePath)
        {
            if (filePath != null)
            {
                player.Open(new Uri(filePath));
                Duration = player.NaturalDuration;
            }
        }

        public bool IsPlaying 
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPlaying)));
            }
        }

        public Song Song
        {
            get => song;
            set
            {
                song = value;
            }
        }

        public void Close()
        {
            player.Close();
            Thread.Sleep(100);  //файл должен успеть закрыться
        }

        public Player SavePlayerOptions()
        {
            return new Player(Song, IsPlaying, Position);
        }

        public void RestorePlayerOptions(Player player)
        {
            Song = player.Song;
            OpenSong(Song);
            IsPlaying = player.IsPlaying;
            Position = player.Position;
            Duration = player.Duration;

        }
    }
}
