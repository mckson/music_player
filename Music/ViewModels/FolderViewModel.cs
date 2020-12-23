using Music.Models;
using Music.Other;
using Music.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.ViewModels
{
    public class FolderViewModel
    {
        private Folder folder;
        private Database database;
        private MusicSecondView musicSecondView;

        public string Title
        {
            get => folder.Title;
            set => folder.Title = value;
        }

        public AnotherCommandImplementation GoBackCommand { get; set; }

        public Database Database { get => database; }

        public FolderViewModel(Folder folder, Database database, MusicSecondView musicSecondView)
        {
            this.folder = folder;
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

        public Folder Folder
        {
            get => folder;
        }
    }
}
