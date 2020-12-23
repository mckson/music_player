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
        public MusicSecondView(MiniPlayerView miniPlayerView)
        {
            MiniPlayer = miniPlayerView;
            InitializeComponent();
        }

        private void lb_Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            frame_Content.Navigate((DataContext as MusicSecondViewModel).SelectedItem.Content);
        }

        private void grid_MiniPlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MusicSecondViewModel).NavigateToFullPlayerView();
        }
    }
}
