using Music.Models;
using Music.Other;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Music.ViewModels
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private Database database;
        private Player player;
        private DispatcherTimer timer;
        private bool isPlaying;

        public event PropertyChangedEventHandler PropertyChanged;

        public AnotherCommandImplementation PlayCommand { get; set; }
        public AnotherCommandImplementation PauseCommand { get; set; }
        public AnotherCommandImplementation PlayNextCommand { get; set; }
        public AnotherCommandImplementation PlayPreviousCommand { get; set; }
        public AnotherCommandImplementation ToFavouritesCommand { get; set; }

        public Database Database { get => database; } 
        public Player Player { get => player; }

        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPlaying)));
            }
        }

        public PlayerViewModel(Database databaseOut)
        {
            database = databaseOut;
            database.Songs.PropertyChanged += OnSelectedSongChanged;
            player = new Player(databaseOut.Songs.SelectedSong);

            PlayCommand = new AnotherCommandImplementation(
                _ =>
                {
                    player.Play();
                    IsPlaying = player.IsPlaying;
                }
                );

            PauseCommand = new AnotherCommandImplementation(
                _ =>
                {
                    player.Pause();
                    IsPlaying = player.IsPlaying;
                }
                );

            PlayNextCommand = new AnotherCommandImplementation(
                _ =>
                {
                    
                    if (database.Songs.SelectedIndex < database.Songs.SongsCollection.Count - 1)
                    {
                        database.Songs.SelectedSong = database.Songs.SongsCollection[++database.Songs.SelectedIndex];
                    }
                    else
                    {
                        database.Songs.SelectedIndex = 0;
                        database.Songs.SelectedSong = database.Songs.SongsCollection[database.Songs.SelectedIndex];
                    }
                    IsPlaying = player.IsPlaying;
                }
                );

            PlayPreviousCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if (database.Songs.SelectedIndex > 0)
                    {
                        database.Songs.SelectedSong = database.Songs.SongsCollection[--database.Songs.SelectedIndex];
                    }
                    else
                    {
                        database.Songs.SelectedIndex = database.Songs.SongsCollection.Count - 1;
                        database.Songs.SelectedSong = database.Songs.SongsCollection[database.Songs.SelectedIndex];
                    }
                }
                );

            ToFavouritesCommand = new AnotherCommandImplementation(
                obj =>
                {
                    ToggleButton buttonFavourites = (ToggleButton)obj;

                    if (buttonFavourites.IsChecked == true)
                    {
                        database.Favourites.AddSong(database.Songs.SelectedSong);
                    }
                    else
                    {
                        database.Favourites.RemoveSong(database.Songs.SelectedSong);
                    }
                }
                );
        }

        private void OnSelectedSongChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSong")
            {
                Song song = (sender as Songs).SelectedSong;
                player.OpenSong(song);
            }
        }
        //private void OnIsPlayingChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    Player player = (sender as Player);
        //    if (player != null)
        //    {
        //        IsPlaying = player.IsPlaying;
        //    }
        //}
    }
}
