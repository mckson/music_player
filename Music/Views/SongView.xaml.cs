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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Music.Views
{
    /// <summary>
    /// Логика взаимодействия для SongView.xaml
    /// </summary>
    public partial class SongView
    {
        public SongView()
        {
            InitializeComponent();
        }

        private void button_Edit_Click(object sender, RoutedEventArgs e)
        {
            tb_Album.IsReadOnly = false;
            tb_Artist.IsReadOnly = false;
            tb_Title.IsReadOnly = false;
            tb_Lyrics.IsReadOnly = false;
            button_Edit.Visibility = Visibility.Collapsed;
            button_Back.Visibility = Visibility.Collapsed;
            button_AllowChanges.Visibility = Visibility.Visible;
            button_LoadPicture.Visibility = Visibility.Visible;
        }

        private void button_AllowChanges_Click(object sender, RoutedEventArgs e)
        {
            tb_Album.IsReadOnly = true;
            tb_Artist.IsReadOnly = true;
            tb_Title.IsReadOnly = true;
            tb_Lyrics.IsReadOnly = true;
            button_Edit.Visibility = Visibility.Visible;
            button_Back.Visibility = Visibility.Visible;
            button_AllowChanges.Visibility = Visibility.Collapsed;
            button_LoadPicture.Visibility = Visibility.Collapsed;
        }

        private void button_LoadPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.BMP, *.JPG, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg; *.tif; *.png; *.ico; *.emf; *.wmf";

            dlg.ShowDialog();

            var path = dlg.FileName;

            (DataContext as SongViewModel).Database.Songs.SelectedSong.Picture = new BitmapImage(new Uri(path));
        }
    }
}
