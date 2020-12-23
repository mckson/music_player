using Music.Models;
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
using MaterialDesignThemes;

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

        private void button_Back_Click(object sender, RoutedEventArgs e)
        {
            tb_Title.Text = String.Empty;
            tb_Description.Text = String.Empty;
            foreach (Song song in lb_Songs.ItemsSource)
            {
                if (song.IsChecked)
                    song.IsChecked = false;
            }
        }

        private void button_Create_Click(object sender, RoutedEventArgs e)
        {
            Playlist playlist = new Playlist();
            playlist.Title = tb_Title.Text;
            playlist.Description = tb_Description.Text;
            tb_Title.Text = String.Empty;
            tb_Description.Text = String.Empty;
            foreach (Song song in lb_Songs.ItemsSource)
            {
                if (song.IsChecked)
                {
                    playlist.AddSong(song);
                    song.IsChecked = false;
                }
            }
            try
            {
                (DataContext as PlaylistsViewModel).Database.Playlists.AddPlaylist(playlist);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (Song song in lb_Songs.ItemsSource)
            {
                if (song.IsChecked)
                    song.IsChecked = false;
            }
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as PlaylistsViewModel).Database.CurrentSongSet = listBox_Playlists.SelectedItem as Playlist;
        }

        //private void listBox_Playlists_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    (DataContext as PlaylistsViewModel).Database.CurrentSongSet = listBox_Playlists.SelectedItem as Playlist;
        //}

        //private void listBox_Playlists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    (DataContext as PlaylistsViewModel).Database.CurrentSongSet = e.AddedItems[0] as Playlist;
        //}
    }
}
