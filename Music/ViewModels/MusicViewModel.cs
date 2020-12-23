using MaterialDesignThemes.Wpf;
using Music.Other;
using Music.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Music.ViewModels
{
    public class MusicViewModel
    {
        private Database database;

        private MusicView musicView;
        private MusicSecondView musicSecondView;
        private FullPlayerView fullPlayerView;
        private SongView songView;

        //Initialization of all secondary UserControls that are viaible from MusicView
        //  - full player
        //  - secondary MusicView + mini player
        //  - song view
        public MusicViewModel(Database database,
           MusicView musicView)
        {
            this.database = database;
            this.musicView = musicView;

            fullPlayerView = new FullPlayerView();
            MiniPlayerView miniPlayerView = new MiniPlayerView();

            PlayerViewModel playerViewModel = new PlayerViewModel(database, musicView/*, fullPlayerView, miniPlayerView*/);

            fullPlayerView.DataContext = playerViewModel;
            playerViewModel.fullPlayerView = fullPlayerView;

            musicSecondView = new MusicSecondView(miniPlayerView);
            musicSecondView.DataContext = new MusicSecondViewModel(database, musicView, musicSecondView);
            musicSecondView.MiniPlayer.DataContext = playerViewModel;

            playerViewModel.miniPlayerView = musicSecondView.MiniPlayer;

            songView = new SongView() { DataContext = new SongViewModel(database, musicView) };

        }

        public void NavigateToMusicSecondView()
        {
            musicView.frame_MusicWindow.Navigate(musicSecondView);
        }

        public void NavigateToFullPlayerView()
        {
            musicView.frame_MusicWindow.Navigate(fullPlayerView);
        }

        public void NavigateToSongView()
        {
            //ClosePlayer();
            musicView.frame_MusicWindow.Navigate(songView);
        }

        //public void ClosePlayer()
        //{
        //    PlayerViewModel playerViewModel = fullPlayerView.DataContext as PlayerViewModel;
        //    playerViewModel.Close();
        //}

        public void OpenPlayer()
        {
            PlayerViewModel playerViewModel = fullPlayerView.DataContext as PlayerViewModel;
        }

        public void PlayPlaylist()
        {
            PlayerViewModel playerViewModel = fullPlayerView.DataContext as PlayerViewModel;
            playerViewModel.PlayPlaylist();
        }

        public void PausePlaylist()
        {
            PlayerViewModel playerViewModel = fullPlayerView.DataContext as PlayerViewModel;
            playerViewModel.PausePlaylist();
        }
    }
}
