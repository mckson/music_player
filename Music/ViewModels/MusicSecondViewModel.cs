using MaterialDesignThemes.Wpf;
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

namespace Music.ViewModels
{
    public class MusicSecondViewModel: INotifyPropertyChanged
    {
        private Database database;

        private MusicView musicView;
        private MusicSecondView musicSecondView;
        private AlbumView albumView;
        private ArtistView artistView;
        private FolderView folderView;
        private PlaylistView playlistView;

        public MusicSecondViewModel(Database database, MusicView musicView, MusicSecondView musicSecondView)
        {
            //Передавть информацию об окнах не через View коснтрукторы, а через MV кострукторы
            this.database = database;
            this.musicView = musicView;
            this.musicSecondView = musicSecondView;

            albumView = new AlbumView();
            artistView = new ArtistView();
            folderView = new FolderView();
            playlistView = new PlaylistView();

            menuItems = GenerateMenuItems();


            database.CurrentSongSet = database.Songs;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<MenuItem> menuItems;
        private MenuItem selectedItem;
        private int selectedIndex;


        public Database Database
        {
            get => database;
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get => menuItems;
            set
            {
                menuItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItems)));
            }
        }

        public MenuItem SelectedItem
        {
            get => selectedItem;
            set
            {
                if (value == null || value.Equals(selectedItem)) return;

                selectedItem = value;
                if (value.Content is AllSongsView)
                    database.CurrentSongSet = database.Songs;
                else if (value.Content is FavouritesView)
                    database.CurrentSongSet = database.Favourites;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        private ObservableCollection<MenuItem> GenerateMenuItems()
        {
            return new ObservableCollection<MenuItem>
            {
                new MenuItem("All songs", new AllSongsView { DataContext = new AllSongsViewModel(database, musicView, musicSecondView) }, new PackIcon{ Kind = PackIconKind.MusicNoteOutline }),
                new MenuItem("Playlists", new PlaylistsView { DataContext = new PlaylistsViewModel(database, musicSecondView) }, new PackIcon{ Kind = PackIconKind.PlaylistMusicOutline }),
                new MenuItem("Favourites", new FavouritesView{ DataContext = new FavouritesViewModel(database) }, new PackIcon{ Kind = PackIconKind.HeartOutline }),
                new MenuItem("Settings", new SettingsView{ DataContext = new SettingsViewModel(database) }, new PackIcon{ Kind = PackIconKind.Settings })
            };
        }

        public void NavigateToFullPlayerView()
        {
            MusicViewModel musicViewModel = musicView.DataContext as MusicViewModel;
            musicViewModel.NavigateToFullPlayerView();
        }

        public void NavigateToAlbumView(Album album)
        {
            database.CurrentSongSet = album;
            albumView.DataContext = new AlbumViewModel(album, database, musicSecondView);
            musicSecondView.frame_Content.Navigate(albumView);
        }

        public void NavigateToArtistView(Artist artist)
        {
            database.CurrentSongSet = artist;
            artistView.DataContext = new ArtistViewModel(artist, database, musicSecondView);
            musicSecondView.frame_Content.Navigate(artistView);
        }

        public void NavigateToFolderView(Folder folder)
        {
            database.CurrentSongSet = folder;
            folderView.DataContext = new FolderViewModel(folder, database, musicSecondView);
            musicSecondView.frame_Content.Navigate(folderView);
        }

        public void NavigateToPlaylistView(Playlist playlist)
        {
            playlistView.DataContext = new PlaylistViewModel(playlist, database, musicSecondView);
            musicSecondView.frame_Content.Navigate(playlistView);
        }

        public void NavigateToPlaylistsView()
        {
            musicSecondView.frame_Content.Navigate(menuItems[1].Content);
        }

        public void NavigateToAllSongsView()
        {
            musicSecondView.frame_Content.Navigate(menuItems[0].Content);
        }

        public void NavigateToSongView()
        {
            MusicViewModel musicViewModel = musicView.DataContext as MusicViewModel;
            musicViewModel.NavigateToSongView();
        }

        public void PlayPlaylist()
        {
            MusicViewModel musicViewModel = musicView.DataContext as MusicViewModel;
            musicViewModel.PlayPlaylist();
        }

        public void PausePlaylist()
        {
            MusicViewModel musicViewModel = musicView.DataContext as MusicViewModel;
            musicViewModel.PausePlaylist();
        }
    }
}
