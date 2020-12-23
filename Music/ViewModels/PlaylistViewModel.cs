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
    //НЕ ОТОБРАЖАЕТСЯ SELECTED SONG В ПЛЕЕРЕ
    public class PlaylistViewModel : INotifyPropertyChanged
    {
        private Playlist playlist;
        private Database database;
        private MusicSecondView musicSecondView;

        public Database Database { get => database; }

        public AnotherCommandImplementation GoBackCommand { get; set; }
        public AnotherCommandImplementation DeleteCommand { get; set; }

        public string Title
        {
            get => playlist.Title;
            set
            {
                playlist.Title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }
        public string Description
        {
            get => playlist.Description;
            set
            {
                playlist.Description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public PlaylistViewModel(Playlist playlist, Database database, MusicSecondView musicSecondView)
        {
            this.playlist = playlist;
            this.database = database;
            this.musicSecondView = musicSecondView;

            GoBackCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToPrevious();
                }
                );

            DeleteCommand = new AnotherCommandImplementation(
                _ =>
                {
                    database.Playlists.RemovePlaylist(database.Playlists.SelectedPlaylist);
                    NavigateToPrevious();
                }
                );
        }

        private void NavigateToPrevious()
        {
            MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
            musicSecondViewModel.NavigateToPlaylistsView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
