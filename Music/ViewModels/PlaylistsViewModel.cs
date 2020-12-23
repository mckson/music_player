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

namespace Music.ViewModels
{
   //КАК CURRENT SONG SET ИДЕТ ПЕРВЫЙ ПЛЕЙЛИСТ, ИСПРАВИТЬ НА SONGS
    public class PlaylistsViewModel : INotifyPropertyChanged
    {
        private Database database;
        private MusicSecondView musicSecondView;
        private string searchKeyword;
        private ObservableCollection<Song> songsCollection;


        public event PropertyChangedEventHandler PropertyChanged;

        public AnotherCommandImplementation OpenPlaylistCommand { get; set; }
        public AnotherCommandImplementation PlayPlaylistCommand { get; set; }
        public AnotherCommandImplementation PausePlaylistCommand { get; set; }

        

        public Database Database
        {
            get => database;
        }

        public ObservableCollection<Song> SongsCollection
        {
            get => songsCollection;
            set
            {
                songsCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SongsCollection)));
            }
        }

        public string SearchKeyword
        {
            get => searchKeyword;
            set
            {
                searchKeyword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchKeyword)));
                FilterSongs(searchKeyword);
            }
        }

        public PlaylistsViewModel(Database database, MusicSecondView musicSecondView)
        {
            this.database = database;
            this.musicSecondView = musicSecondView;

            database.Playlists.PropertyChanged += OnSelectedPlaylistChanged;

            songsCollection = this.database.Songs.Songs;

            OpenPlaylistCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToPlaylistView(Database.Playlists.SelectedPlaylist);
                }
                );

            PlayPlaylistCommand = new AnotherCommandImplementation(
                _ =>
                {
                    MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
                    if (database.CurrentSongSet != null)
                    {
                        musicSecondViewModel.PlayPlaylist();
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[1].Content as PlaylistsView).button_PausePlaylist.Visibility = Visibility.Visible;
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[1].Content as PlaylistsView).button_PlayPlaylist.Visibility = Visibility.Collapsed;

                    }
                    else
                        MessageBox.Show("There is no selected playlist");
                }
                );

            PausePlaylistCommand = new AnotherCommandImplementation(
                _ =>
                {
                    MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
                    if (database.CurrentSongSet != null)
                    {
                        musicSecondViewModel.PausePlaylist();
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[1].Content as PlaylistsView).button_PausePlaylist.Visibility = Visibility.Collapsed;
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[1].Content as PlaylistsView).button_PlayPlaylist.Visibility = Visibility.Visible;

                    }
                    else
                        MessageBox.Show("There is no selected playlist");
                }
                );


            database.CurrentSongSet = database.Songs;
        }

        public void NavigateToPlaylistView(Playlist playlist)
        {
            MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
            database.CurrentSongSet = playlist;
            if (database.CurrentSongSet != null)
                musicSecondViewModel.NavigateToPlaylistView(playlist);
            else
                MessageBox.Show("There is no selected playlist");
        }

        private void FilterSongs(string keyword)
        {
            var filteredSongs = 
                string.IsNullOrWhiteSpace(keyword) ? 
                database.Songs.Songs : 
                database.Songs.Songs.Where(s => s.Title.ToLower().Contains(keyword.ToLower()) || s.Album.ToLower().Contains(keyword.ToLower()) || s.Artist.ToLower().Contains(keyword.ToLower()));
            
            SongsCollection = new ObservableCollection<Song>(filteredSongs);
        }

        private bool isInitialization = true;

        private void OnSelectedPlaylistChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPlaylist")
            {
                if (isInitialization)
                    isInitialization = false;
                else if (database.Playlists.SelectedPlaylist != null && database.CurrentSongSet != database.Playlists.SelectedPlaylist)
                    database.CurrentSongSet = database.Playlists.SelectedPlaylist;
            }
        }
    }
}
