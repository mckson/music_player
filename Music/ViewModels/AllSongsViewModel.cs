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
    public class AllSongsViewModel : INotifyPropertyChanged
    {
        private Database database;

        private MusicView musicView;
        private MusicSecondView musicSecondView;
        public Database Database { get => database; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AnotherCommandImplementation GoToSongViewCommand { get; set; }
        public AnotherCommandImplementation SyncronizeInfoCommand { get; set; }
        public AnotherCommandImplementation GoToAlbumsCommand { get; set; }
        public AnotherCommandImplementation GoToArtistsCommand { get; set; }
        public AnotherCommandImplementation GoToFoldersCommand { get; set; }
        public AnotherCommandImplementation OpenPlaylistCommand { get; set; }
        public AnotherCommandImplementation PlayPlaylistCommand { get; set; }
        public AnotherCommandImplementation PausePlaylistCommand { get; set; }

        public AllSongsViewModel(Database database, MusicView musicView, MusicSecondView musicSecondView)
        {
            this.database = database;
            this.musicView = musicView;
            this.musicSecondView = musicSecondView;

            database.Albums.PropertyChanged += OnSelectedSongSetChanged;
            database.Artists.PropertyChanged += OnSelectedSongSetChanged;
            database.Folders.PropertyChanged += OnSelectedSongSetChanged;

            GoToSongViewCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToSongView();
                }
                );

            SyncronizeInfoCommand = new AnotherCommandImplementation(
                _ =>
                {
                    database.SyncronizeAlbums();
                    database.SyncronizeArtists();
                    database.SyncronizeFolders();
                }
                );

            GoToAlbumsCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToAlbumView(database.Albums.SelectedAlbum);
                }
                );

            GoToArtistsCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToArtistView(database.Artists.SelectedArtist);
                }
                );

            GoToFoldersCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToFolderView(database.Folders.SelectedFolder);
                }
                );

            OpenPlaylistCommand = new AnotherCommandImplementation(
                _ =>
                {
                    AllSongsView allSongsView = (musicSecondView.DataContext as MusicSecondViewModel).MenuItems[0].Content as AllSongsView;
                    if (allSongsView.gr_Albums.Visibility == System.Windows.Visibility.Visible)
                        NavigateToAlbumView(database.Albums.SelectedAlbum);
                    else if (allSongsView.gr_Artists.Visibility == System.Windows.Visibility.Visible)
                        NavigateToArtistView(database.Artists.SelectedArtist);
                    else if (allSongsView.gr_Folders.Visibility == System.Windows.Visibility.Visible)
                        NavigateToFolderView(database.Folders.SelectedFolder);
                }
                );

            PlayPlaylistCommand = new AnotherCommandImplementation(
                _ =>
                {
                    MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
                    if (database.CurrentSongSet != null)
                    {
                        musicSecondViewModel.PlayPlaylist();
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[0].Content as AllSongsView).button_PausePlaylist.Visibility = Visibility.Visible;
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[0].Content as AllSongsView).button_PlayPlaylist.Visibility = Visibility.Collapsed;

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
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[0].Content as AllSongsView).button_PausePlaylist.Visibility = Visibility.Collapsed;
                        ((musicSecondView.DataContext as MusicSecondViewModel).MenuItems[0].Content as AllSongsView).button_PlayPlaylist.Visibility = Visibility.Visible;

                    }
                    else
                        MessageBox.Show("There is no selected playlist");
                }
                );
        }

        private bool isAlbumsInitialization = true; 
        private bool isArtistsInitialization = true; 
        private bool isFoldersInitialization = true;

        private void OnSelectedSongSetChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAlbum")
            {
                if (isAlbumsInitialization)
                    isAlbumsInitialization = false;
                else if (database.Albums.SelectedAlbum != null && database.CurrentSongSet != database.Albums.SelectedAlbum)
                    database.CurrentSongSet = database.Albums.SelectedAlbum;
            }
            else if (e.PropertyName == "SelectedArtist")
            {
                if (isArtistsInitialization)
                    isArtistsInitialization = false;
                else if(database.Artists.SelectedArtist != null && database.CurrentSongSet != database.Artists.SelectedArtist)
                    database.CurrentSongSet = database.Artists.SelectedArtist;
            }
            else if (e.PropertyName == "SelectedFolder")
            {
                if (isFoldersInitialization)
                    isFoldersInitialization = false;
                else if (database.Folders.SelectedFolder != null && database.CurrentSongSet != database.Folders.SelectedFolder)
                    database.CurrentSongSet = database.Folders.SelectedFolder;
            }
        }

        private void NavigateToSongView()
        {
            MusicViewModel musicViewModel = musicView.DataContext as MusicViewModel;
            musicViewModel.NavigateToSongView();
        }

        private void NavigateToAlbumView(Album album)
        {
            MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
            musicSecondViewModel.NavigateToAlbumView(album);
        }

        private void NavigateToArtistView(Artist artist)
        {
            MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
            musicSecondViewModel.NavigateToArtistView(artist);
        }

        private void NavigateToFolderView(Folder folder)
        {
            MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
            musicSecondViewModel.NavigateToFolderView(folder);
        }
    }
}
