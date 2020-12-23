using Music.Other;
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
using System.Windows.Threading;

namespace Music.Views
{
    /// <summary>
    /// Логика взаимодействия для FullPlayerView.xaml
    /// </summary>
    public partial class FullPlayerView : UserControl
    {
        private bool userIsDraggingSlider = false;

        public FullPlayerView()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();  

            InitializeComponent();
        }


        private void timer_Tick(object sender, EventArgs e)
        {

            if (((DataContext as PlayerViewModel).Player != null) && (DataContext as PlayerViewModel).Player.Duration.HasTimeSpan)
            {
                slider_Position.Minimum = 0;
                slider_Position.Maximum = (DataContext as PlayerViewModel).Player.Duration.TimeSpan.TotalSeconds;
                slider_Position.Value = (DataContext as PlayerViewModel).Player.Position.TotalSeconds;
                try
                {
                    textbox_Duration.Text = String.Format("-{0}", TimeSpan.FromSeconds((DataContext as PlayerViewModel).Player.Duration.TimeSpan.TotalSeconds - (DataContext as PlayerViewModel).Player.Position.TotalSeconds).ToString(@"mm\:ss"));
                }
                catch
                {

                }
            }
        }

        private void button_ToMiniPlayer_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as PlayerViewModel).NavigateToMusicSecondView();
        }

        private void slider_Position_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void slider_Position_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            (DataContext as PlayerViewModel).Player.Position = TimeSpan.FromSeconds(slider_Position.Value);
        }

        private void slider_Position_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textbox_Position.Text = TimeSpan.FromSeconds(slider_Position.Value).ToString(@"mm\:ss");
            if ((DataContext as PlayerViewModel).Player.Duration.HasTimeSpan)
            {
                if (slider_Position.Value == (DataContext as PlayerViewModel).Player.Duration.TimeSpan.TotalSeconds)
                {
                    
                    if (button_PlayInOrder.Visibility == Visibility.Collapsed && button_PlayRandomly.Visibility == Visibility.Visible)
                        (DataContext as PlayerViewModel).PlayNext();    //нажата кнопка играть по порядку
                    else if (button_RepeatSong.Visibility == Visibility.Collapsed && button_PlayInOrder.Visibility == Visibility.Visible)
                        (DataContext as PlayerViewModel).RepeatSong();  //нажато повторить песню
                    else
                        (DataContext as PlayerViewModel).RandomNext();  //нажато инрать в случайном порядке
                    slider_Position.Value = slider_Position.Minimum;
                }
            }
        }

        private void button_SwitchIndicator_Click(object sender, RoutedEventArgs e)
        {
            if (grid_SongLyrics.Visibility == Visibility.Collapsed && grid_SongPicture.Visibility == Visibility.Visible)
            {
                grid_SongLyrics.Visibility = Visibility.Visible;
                grid_SongPicture.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid_SongLyrics.Visibility = Visibility.Collapsed;
                grid_SongPicture.Visibility = Visibility.Visible;
            }
            grid_CurrentSongSet.Visibility = Visibility.Collapsed;
        }

        private void button_RepeatSong_Click(object sender, RoutedEventArgs e)
        {
            button_RepeatSong.Visibility = Visibility.Collapsed;
            button_PlayInOrder.Visibility = Visibility.Visible;
        }

        private void button_PlayInOrder_Click(object sender, RoutedEventArgs e)
        {
            button_PlayInOrder.Visibility = Visibility.Collapsed;
            button_PlayRandomly.Visibility = Visibility.Visible;
        }

        private void button_PlayRandomly_Click(object sender, RoutedEventArgs e)
        {
            button_PlayRandomly.Visibility = Visibility.Collapsed;
            button_RepeatSong.Visibility = Visibility.Visible;
        }

        private void button_CurrentSongSet_Click(object sender, RoutedEventArgs e)
        {
            grid_CurrentSongSet.Visibility = Visibility.Visible;
            grid_SongLyrics.Visibility = Visibility.Collapsed;
            grid_SongPicture.Visibility = Visibility.Collapsed;

            button_CurrentSongSet.Visibility = Visibility.Collapsed;
            button_CurrentSongSetHide.Visibility = Visibility.Visible;
        }

        private void button_CurrentSongSetHide_Click(object sender, RoutedEventArgs e)
        {
            grid_CurrentSongSet.Visibility = Visibility.Collapsed;
            grid_SongPicture.Visibility = Visibility.Visible;

            button_CurrentSongSet.Visibility = Visibility.Visible;
            button_CurrentSongSetHide.Visibility = Visibility.Collapsed;
        }
    }
}
