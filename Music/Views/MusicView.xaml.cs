using Music.Other;
using Music.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Music.Views
{
    /// <summary>
    /// Логика взаимодействия для MusicView.xaml
    /// </summary>
    public partial class MusicView : Window
    {
        private Database database;

        public MusicView()
        {
            InitializeComponent();
            database = new Database();
            MusicViewModel musicViewModel = new MusicViewModel(database, this);
            DataContext = musicViewModel;


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += timer_Tick;
            timer.Start();


            (DataContext as MusicViewModel).NavigateToMusicSecondView();
            //(DataContext as MusicViewModel).NavigateToSongView();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //database.Syncronize();
        }

        private void window_MusicWindow_Closed(object sender, System.EventArgs e)
        {
            database.SaveChanges();
        }
    }
}
