using Music.Other;
using Music.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
            database = new Database();
            MusicViewModel musicViewModel = new MusicViewModel(database, this, musicSecondView);    //для musicSecond отдельная vm???


            MusicSecondView musicSecondView = new MusicSecondView(this, new PlayerViewModel(database)) { DataContext = musicViewModel };

            InitializeComponent();

            frame_MusicWindow.Navigate(musicSecondView);
        }

        private void window_MusicWindow_Closed(object sender, System.EventArgs e)
        {
            database.SaveChanges();
        }
    }
}
