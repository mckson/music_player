using Music.Models;
using Music.Other;
using Music.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Music.ViewModels
{
    public class AlbumViewModel
    {
        private Album album;
        private Database database;
        private MusicSecondView musicSecondView;

        public string Title
        {
            get => album.Title;
            set => album.Title = value;
        }
        public string Description
        {
            get => album.Description;
            set => album.Description = value;
        }

        public AnotherCommandImplementation GoBackCommand { get; set; }
        public AnotherCommandImplementation DeleteAlbumCommand { get; set; }

        public Database Database { get => database; }

        public AlbumViewModel(Album album, Database database, MusicSecondView musicSecondView)
        {
            this.album = album;
            this.database = database;
            this.musicSecondView = musicSecondView;

            GoBackCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToPrevious();
                }
                );

            DeleteAlbumCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if (database.Albums.SelectedAlbum.Songs.Count == 0)
                    {
                        database.Albums.RemoveAlbum(database.Albums.SelectedAlbum);
                        NavigateToPrevious();
                    }
                    else
                        MessageBox.Show("You can not delete nonempty album");
                }
                );
        }

        private void NavigateToPrevious()
        {
            MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
            musicSecondViewModel.NavigateToAllSongsView();
        }

        public Album Album
        {
            get => album;
        }
    }
}
