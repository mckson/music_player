using Music.Models;
using Music.Other;
using Music.Views;
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
    //ПО МИЛЛИОН РАЗ ОБАРАБАТЫВАЕТ SELECTED SONG CHANGED
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private MusicView musicView;
        public FullPlayerView fullPlayerView { get; set; }
        public MiniPlayerView miniPlayerView { get; set; }

        private Database database;
        private Player player;
        private Player bufferPlayer;
        //private DispatcherTimer timer;
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

        public PlayerViewModel(Database databaseOut, MusicView musicView/*, FullPlayerView fullPlayerView, MiniPlayerView miniPlayerView*/)
        {
            database = databaseOut;

            this.musicView = musicView;
            //this.fullPlayerView = fullPlayerView;
            //this.miniPlayerView = miniPlayerView;

            //SetHandlersForSongSets(database);

            database.PropertyChanging += OnCurrentSongSetChanging;
            database.PropertyChanged += OnCurrentSongSetChanged; 

            if (database.CurrentSongSet != null)
            { 
                player = new Player(database.CurrentSongSet.SelectedSong);        //переписать для currentSongSet
                if (player.Song != null)
                {
                    player.Song.PropertyChanged += OnSongPropertyChanged;
                    player.Song.PropertyChanging += OnSongPropertyChanging;
                }
            }


            PlayCommand = new AnotherCommandImplementation(
                _ =>
                {
                    player.Play();
                    IsPlaying = player.IsPlaying;

                    miniPlayerView.button_Play.Visibility = Visibility.Collapsed;
                    miniPlayerView.button_Pause.Visibility = Visibility.Visible;


                    fullPlayerView.button_Play.Visibility = Visibility.Collapsed;
                    fullPlayerView.button_Pause.Visibility = Visibility.Visible;
                }
                );

            PauseCommand = new AnotherCommandImplementation(
                _ =>
                {
                    player.Pause();
                    IsPlaying = player.IsPlaying;
                    miniPlayerView.button_Play.Visibility = Visibility.Visible;
                    miniPlayerView.button_Pause.Visibility = Visibility.Collapsed;


                    fullPlayerView.button_Play.Visibility = Visibility.Visible;
                    fullPlayerView.button_Pause.Visibility = Visibility.Collapsed;
                }
                );

            PlayNextCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if ((fullPlayerView.button_PlayInOrder.Visibility == Visibility.Collapsed && 
                        fullPlayerView.button_PlayRandomly.Visibility == Visibility.Visible)
                        || 
                        (fullPlayerView.button_RepeatSong.Visibility == Visibility.Collapsed &&
                        fullPlayerView.button_PlayInOrder.Visibility == Visibility.Visible))
                        PlayNext();    //нажата кнопка играть по порядку
                    else
                        RandomNext();  //нажато инрать в случайном порядке
                }
                );

            PlayPreviousCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if (database.CurrentSongSet.Songs.Count > 0)
                    {
                        if (database.CurrentSongSet.SelectedIndex > 0)
                        {
                            database.CurrentSongSet.SelectedSong = database.CurrentSongSet.Songs[--database.CurrentSongSet.SelectedIndex];
                        }
                        else
                        {
                            database.CurrentSongSet.SelectedIndex = database.CurrentSongSet.Songs.Count - 1;
                            database.CurrentSongSet.SelectedSong = database.CurrentSongSet.Songs[database.CurrentSongSet.SelectedIndex];
                        }
                    }
                }
                );

            ToFavouritesCommand = new AnotherCommandImplementation(
                obj =>
                {
                    ToggleButton buttonFavourites = (ToggleButton)obj;

                    if (buttonFavourites.IsChecked == true)
                    {
                        database.Favourites.AddSong(database.CurrentSongSet.SelectedSong);
                    }
                    else
                    {
                        database.Favourites.RemoveSong(database.CurrentSongSet.SelectedSong);
                    }
                }
                );
        }

        //private void SetHandlersForSongSets(Database db)
        //{
        //    db.Songs.PropertyChanged += OnSelectedSongChanged;
        //    db.Favourites.PropertyChanged += OnSelectedSongChanged;

        //    foreach (Album al in database.Albums.AlbumsCollection)
        //        al.PropertyChanged += OnSelectedSongChanged;

        //    foreach (Artist a in db.Artists.ArtistsCollection)
        //        a.PropertyChanged += OnSelectedSongChanged;

        //    foreach (Folder f in db.Folders.FoldersCollection)
        //        f.PropertyChanged += OnSelectedSongChanged;

        //    foreach (Playlist p in database.Playlists.PlaylistsCollection)
        //        p.PropertyChanged += OnSelectedSongChanged;
        //}

        //private void RemoveHandlersForSongSets(Database db)
        //{

        //}

        public void PlayNext()
        {
            if (database.CurrentSongSet.Songs.Count > 0)
            {
                if (database.CurrentSongSet.SelectedIndex < database.CurrentSongSet.Songs.Count - 1)
                {
                    database.CurrentSongSet.SelectedSong = database.CurrentSongSet.Songs[++database.CurrentSongSet.SelectedIndex];
                }
                else
                {
                    database.CurrentSongSet.SelectedIndex = 0;
                    database.CurrentSongSet.SelectedSong = database.CurrentSongSet.Songs[database.CurrentSongSet.SelectedIndex];
                }
            }
        }

        public void RepeatSong()
        {
            if (database.CurrentSongSet.Songs.Count > 0)
                OnSelectedSongChanged(this, new PropertyChangedEventArgs("SelectedSong"));
        }

        public void RandomNext()
        {
            if (database.CurrentSongSet.Songs.Count > 0)
            {
                Random rand = new Random();
                int currentIndex = database.CurrentSongSet.SelectedIndex;
                int index = rand.Next(database.CurrentSongSet.Songs.Count);
                while (currentIndex == index)
                    index = rand.Next(database.CurrentSongSet.Songs.Count);

                database.CurrentSongSet.SelectedSong = database.CurrentSongSet.Songs[index];
            }
        }

        private void OnSelectedSongChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSong")
            {
                Song song = database.CurrentSongSet.SelectedSong;
                
                if (song != null)
                {
                    if (player == null)
                        player = new Player(song);

                    player.OpenSong(song);
                    player.Song.PropertyChanged += OnSongPropertyChanged;
                    player.Song.PropertyChanging += OnSongPropertyChanging;
                }
            }
        }

        private void OnSongPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            bufferPlayer = player.SavePlayerOptions();
            player.Close();
        }

        private void OnSongPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (bufferPlayer != null)
                player.RestorePlayerOptions(bufferPlayer);
        }

        //После того, как поменялся CurrentSongSet подписываемся обработчиком OnSelectedSongChanged на его событие PropertyChanged
        private void OnCurrentSongSetChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentSongSet" && (sender as Database).CurrentSongSet != null)
            {
                (sender as Database).CurrentSongSet.PropertyChanged += OnSelectedSongChanged;
                OnSelectedSongChanged((sender as Database).CurrentSongSet, new PropertyChangedEventArgs("SelectedSong"));
            }
        }

        // Перед тем, как поменяется CurrentSongSet отписываемся обработчиком OnSelectedSongChanged от его события PropertyChanged
        private void OnCurrentSongSetChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "CurrentSongSet" && (sender as Database).CurrentSongSet != null)
            {
                (sender as Database).CurrentSongSet.PropertyChanged -= OnSelectedSongChanged;
            }
        }

        public void NavigateToMusicSecondView()
        {
            MusicViewModel musicViewModel = musicView.DataContext as MusicViewModel;
            musicViewModel.NavigateToMusicSecondView();
        }

        public void PausePlaylist()
        {
            player.Pause();
            miniPlayerView.button_Play.Visibility = Visibility.Visible;
            miniPlayerView.button_Pause.Visibility = Visibility.Collapsed;


            fullPlayerView.button_Play.Visibility = Visibility.Visible;
            fullPlayerView.button_Pause.Visibility = Visibility.Collapsed;
        }

        public void PlayPlaylist()
        {
            database.CurrentSongSet.SelectedSong = database.CurrentSongSet.Songs[0];
            if (database.CurrentSongSet.SelectedSong != null)
            {
                player.OpenSong(database.CurrentSongSet.SelectedSong);

                if (!player.IsPlaying)
                {
                    player.Play();
                    isPlaying = true;
                    miniPlayerView.button_Play.Visibility = Visibility.Collapsed;
                    miniPlayerView.button_Pause.Visibility = Visibility.Visible;


                    fullPlayerView.button_Play.Visibility = Visibility.Collapsed;
                    fullPlayerView.button_Pause.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("There is an empty playlist");
            }
        }
    }
}
