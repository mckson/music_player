using Music.Models;
using Music.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Music.Views
{
    /// <summary>
    /// Логика взаимодействия для PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView
    {
        public PlaylistView()
        {
            InitializeComponent();
        }

        private void button_Edit_Click(object sender, RoutedEventArgs e)
        {
            textBlock_Description.Visibility = Visibility.Collapsed;
            textBlock_Title.Visibility = Visibility.Collapsed;
            textBox_Description.Visibility = Visibility.Visible;
            textBox_Title.Visibility = Visibility.Visible;

            button_Back.Visibility = Visibility.Collapsed;
            button_Edit.Visibility = Visibility.Collapsed;
            button_AllowChanges.Visibility = Visibility.Visible;
            button_DeletePlaylist.Visibility = Visibility.Visible;
            button_LoadPicture.Visibility = Visibility.Visible;

            lb_Playlist.Visibility = Visibility.Collapsed;
            lb_Songs.Visibility = Visibility.Visible;

            foreach (Song song in lb_Songs.ItemsSource)
            {
                if ((DataContext as PlaylistViewModel).Database.Playlists.SelectedPlaylist.ContainsAudio(song))
                {
                    //(DataContext as PlaylistViewModel).playlist.AddSong(song);
                    song.IsChecked = true;
                }
            }
        }

        private void button_AllowChanges_Click(object sender, RoutedEventArgs e)
        {
            textBlock_Description.Visibility = Visibility.Visible;
            textBlock_Title.Visibility = Visibility.Visible;
            textBox_Description.Visibility = Visibility.Collapsed;
            textBox_Title.Visibility = Visibility.Collapsed;

            button_Back.Visibility = Visibility.Visible;
            button_Edit.Visibility = Visibility.Visible;
            button_DeletePlaylist.Visibility = Visibility.Collapsed;
            button_AllowChanges.Visibility = Visibility.Collapsed;
            button_LoadPicture.Visibility = Visibility.Collapsed;

            lb_Playlist.Visibility = Visibility.Visible;
            lb_Songs.Visibility = Visibility.Collapsed;

            Playlist playlist = (DataContext as PlaylistViewModel).Database.Playlists.SelectedPlaylist;

            //очищаем список песен перед обновлением
            
            while (playlist.Songs.Count != 0)
            {
                playlist.RemoveSong(playlist.Songs[0]);
            }

            foreach (Song song in lb_Songs.ItemsSource)
            {
                if (song.IsChecked)
                {
                    playlist.AddSong(song);
                }
            }

            //сбрасываем isChecked для следующих операций
            foreach (Song song in lb_Songs.ItemsSource)
            {
                if (song.IsChecked)
                    song.IsChecked = false;
            }
        }

        private void button_DeletePlaylist_Click(object sender, RoutedEventArgs e)
        {
            textBlock_Description.Visibility = Visibility.Visible;
            textBlock_Title.Visibility = Visibility.Visible;
            textBox_Description.Visibility = Visibility.Collapsed;
            textBox_Title.Visibility = Visibility.Collapsed;

            button_Back.Visibility = Visibility.Visible;
            button_Edit.Visibility = Visibility.Visible;
            button_DeletePlaylist.Visibility = Visibility.Collapsed;
            button_AllowChanges.Visibility = Visibility.Collapsed;
            button_LoadPicture.Visibility = Visibility.Collapsed;

            lb_Playlist.Visibility = Visibility.Visible;
            lb_Songs.Visibility = Visibility.Collapsed;
        }

        private void button_LoadPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.BMP, *.JPG, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg; *.tif; *.png; *.ico; *.emf; *.wmf";

            dlg.ShowDialog();

            var path = dlg.FileName;
            
            if (path != null && path != string.Empty)
                (DataContext as PlaylistViewModel).Database.Playlists.SelectedPlaylist.ImagePath = path;
        }
    }
}
