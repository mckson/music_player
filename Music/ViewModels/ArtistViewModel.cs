using Music.Models;
using Music.Other;
using Music.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.ViewModels
{
    public class ArtistViewModel : INotifyPropertyChanged
    {
        private Artist artist;
        private Database database;
        private MusicSecondView musicSecondView;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get => artist.Title;
        }
        public string Description
        {
            get => artist.Description;
            set
            {
                artist.Description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public AnotherCommandImplementation GoBackCommand { get; set; }

        public Database Database { get => database; }

        public ArtistViewModel(Artist artist, Database database, MusicSecondView musicSecondView)
        {
            this.artist = artist;
            this.database = database;
            this.musicSecondView = musicSecondView;

            GoBackCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToPrevious();
                }
                );
        }

        private void NavigateToPrevious()
        {
            MusicSecondViewModel musicSecondViewModel = musicSecondView.DataContext as MusicSecondViewModel;
            musicSecondViewModel.NavigateToAllSongsView();
        }

        public Artist Artist
        {
            get => artist;
        }
    }
}
