using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
using Music.Models;
using Music.Other;

namespace Music.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Database database;
        //private ObservableCollection<IGrouping<string, Song>> albums = new ObservableCollection<IGrouping<string, Song>>();
        //private ObservableCollection<IGrouping<string, Song>> artists = new ObservableCollection<IGrouping<string, Song>>();
        //public ObservableCollection<IGrouping<string, Song>> Albums
        //{
        //    get => albums;
        //}
        //public ObservableCollection<IGrouping<string, Song>> Artists
        //{
        //    get => artists;
        //}
        //public IGrouping<string, Song> SelectedAlbum { get; set; }
        //public IGrouping<string, Song> SelectedArtist { get; set; }

        public MainWindow()
        {
            //DataContext = this;
            InitializeComponent();
            //database = new Database();

            //var albumsG = from song in database.Songs.SongsCollection
            //             group song by song.Album;

            //var artistsG = from song in database.Songs.SongsCollection
            //              group song by song.Artist;

            //foreach (IGrouping<string, Song> s in albumsG)
            //{
            //    albums.Add(s);
            //}
            //foreach (IGrouping<string, Song> s in artistsG)
            //{
            //    artists.Add(s);
            //}

            //database.SaveChanges();
        }

        //private void lbAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    foreach(Song s in SelectedAlbum) {
        //        ImageConverter imgCon = new ImageConverter();
        //        img.Source = PictureDecoders.LoadImage((byte[])imgCon.ConvertTo(s.Picture, typeof(byte[])));
        //        MessageBox.Show(s.Title + s.Lyrics);
        //    }
        //}


        //private void lbArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    foreach (Song s in SelectedArtist)
        //    {
        //        ImageConverter imgCon = new ImageConverter();
        //        img.Source  =  PictureDecoders.LoadImage((byte[])imgCon.ConvertTo(s.Picture, typeof(byte[])));
        //        MessageBox.Show(s.Title + s.Lyrics);
        //    }
        //}
    }
}
