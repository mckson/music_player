using Music.Other;
using Music.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.ViewModels
{
    public class SongViewModel
    {
        private Database database;
        private MusicView musicView;

        public Database Database { get => database; }

        public AnotherCommandImplementation GoBackCommand { get; set; }

        public SongViewModel(Database database, MusicView musicView)
        {
            this.musicView = musicView;
            this.database = database;

            GoBackCommand = new AnotherCommandImplementation(
                _ =>
                {
                    NavigateToPrevious();
                }
            );
        }

        private void NavigateToPrevious()
        {
            if (musicView.frame_MusicWindow.CanGoBack)
                musicView.frame_MusicWindow.GoBack();
        }
    }
}
