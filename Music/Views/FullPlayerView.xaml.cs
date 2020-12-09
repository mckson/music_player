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
        private MusicSecondView musicSecondView;    //for main frame navigation
        private MusicView mainMusicView;            //for main frame navigation
        private bool userIsDraggingSlider = false;

        public FullPlayerView(MusicSecondView musicSecondView, MusicView mainMusicView)
        {
            this.musicSecondView = musicSecondView;
            this.mainMusicView = mainMusicView;

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
                textbox_Duration.Text = String.Format("-{0}", TimeSpan.FromSeconds((DataContext as PlayerViewModel).Player.Duration.TimeSpan.TotalSeconds - (DataContext as PlayerViewModel).Player.Position.TotalSeconds).ToString(@"mm\:ss"));/* (TimeSpan.FromSeconds((DataContext as PlayerViewModel).Player.Duration.TimeSpan.TotalSeconds) - (DataContext as PlayerViewModel).Player.Position.TotalSeconds).ToString(@"mm\:ss");*/
            }
        }

        private void button_ToMiniPlayer_Click(object sender, RoutedEventArgs e)
        {
            mainMusicView.frame_MusicWindow.Navigate(musicSecondView);
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
            if (slider_Position.Value == (DataContext as PlayerViewModel).Player.Duration.TimeSpan.TotalSeconds)
            {
                //вызвать комманду next
                //PlayerViewModel pwm = (DataContext as PlayerViewModel);
                //Database database = pwm.Database;
                //void Next() {
                //    (DataContext as PlayerViewModel).PlayNextCommand.Execute(;
                //};
                // += Next();
            }
        }
    }
}
