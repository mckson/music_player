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
    /// Логика взаимодействия для ArtistView.xaml
    /// </summary>
    public partial class ArtistView
    {
        public ArtistView()
        {
            InitializeComponent();
        }

        private void button_LoadPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.BMP, *.JPG, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg; *.tif; *.png; *.ico; *.emf; *.wmf";

            dlg.ShowDialog();

            var path = dlg.FileName;

            (DataContext as ArtistViewModel).Database.Artists.SelectedArtist.ImagePath = path;
        }

        private void button_AllowChanges_Click(object sender, RoutedEventArgs e)
        {
            textBlock_Description.Visibility = Visibility.Visible;
            textBox_Description.Visibility = Visibility.Collapsed;

            button_Back.Visibility = Visibility.Visible;
            button_Edit.Visibility = Visibility.Visible;
            button_AllowChanges.Visibility = Visibility.Collapsed;
            button_LoadPicture.Visibility = Visibility.Collapsed;
        }

        private void button_Edit_Click(object sender, RoutedEventArgs e)
        {
            textBlock_Description.Visibility = Visibility.Collapsed;
            textBox_Description.Visibility = Visibility.Visible;

            button_Back.Visibility = Visibility.Collapsed;
            button_Edit.Visibility = Visibility.Collapsed;
            button_AllowChanges.Visibility = Visibility.Visible;
            button_LoadPicture.Visibility = Visibility.Visible;
        }
    }
}
