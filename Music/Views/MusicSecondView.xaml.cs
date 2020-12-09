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
    /// Логика взаимодействия для MusicSecondView.xaml
    /// </summary>
    public partial class MusicSecondView : UserControl
    {
        private MusicView mainMusicView;
        private FullPlayerView fullPlayer;
        public MusicSecondView(MusicView mv, PlayerViewModel playerViewModel)
        {
            mainMusicView = mv; //для доступа к frame
            fullPlayer = new FullPlayerView(this, mainMusicView) { DataContext = playerViewModel };

            InitializeComponent();
            MiniPlayer.DataContext = playerViewModel;
        }

        private void lb_Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            frame_Content.Navigate((DataContext as MusicViewModel).SelectedItem.Content);
        }

        private void grid_MiniPlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainMusicView.frame_MusicWindow.Navigate(fullPlayer);
        }
    }
}
