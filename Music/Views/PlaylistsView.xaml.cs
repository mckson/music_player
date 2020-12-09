using Music.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Music.Views
{
    /// <summary>
    /// Логика взаимодействия для PlaylistsView.xaml
    /// </summary>
    public partial class PlaylistsView : UserControl
    {
        public PlaylistsView()
        {
            InitializeComponent();
        }

        private void ListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainMusicView.frame_MusicWindow.Navigate(new SongList { DataContext = (DataContext as PlaylistsViewModel).Database.Playlists.SelectedPlaylist });
        }
    }
}
