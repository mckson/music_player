using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Lib;
using Music.Other;
using System.Drawing;   //for image
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;
using System.Drawing.Imaging;

namespace Music.Models
{
    public class Song : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string title;
        private string artist;
        private string album;
        private string path;
        private bool isFavorite;
        private Image picture;
        private double duration;
        private string genre;
        private string year;
        private string lyrics;
        private string folder;
        private Mp3File mp3;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public string Title
        {
            get => title;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Title)));
                title = value;
                mp3.TagHandler.Title = title;
                mp3.Update();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        public string Artist
        {
            get => artist;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Artist)));
                artist = value;
                mp3.TagHandler.Artist = artist;
                mp3.Update();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Artist)));
            }
        }
        public string Album
        {
            get => album;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Album)));
                album = value;
                mp3.TagHandler.Album = album;
                mp3.Update();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Album)));
            }
        }
        public string Path
        {
            get => path;
            set => path = value;
        }

        public bool IsFavorite
        {
            get => isFavorite;
            set
            {
                isFavorite = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isFavorite)));
            }
        }

        public BitmapImage Picture
        {
            get
            {
                //BitmapSource bt = PictureDecoders.LoadImage(ImageToByteArray(picture));
                //return BitmapToBitmapImage((Bitmap)picture);
                return null;
            }
            set
            {
                //НЕ КОНВЕРТИТ
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Picture)));
                //picture = (Bitmap)(new ImageConverter()).ConvertFrom(value);
                picture = BitmapImageToBitmap(value);
                mp3.TagHandler.Picture = picture;
                mp3.Update();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Picture)));
            }
        }

        public double Duration
        {
            get => duration;
        }

        public string Genre
        {
            get => genre;
            set => genre = value;
        }

        public string Year
        {
            get => year;
            set => year = value;
        }

        public string Lyrics
        {
            get => lyrics;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Lyrics)));
                lyrics = value;
                mp3.TagHandler.Lyrics = lyrics;
                mp3.Update();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lyrics)));
            }
        }

        public string Folder
        {
            get => folder;
            set => folder = value;
        }

        private bool isChecked = false;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }

        //значение, которое вернет хэш-функция из id3lib
        public byte[] Audio { get; set; }

        public Song() { }       //xml

        public Song(string filePath)
        {
            mp3 = new Mp3File(filePath);
            //this.database = database;

            title = mp3.TagHandler.Title;
            artist = mp3.TagHandler.Artist;
            album = mp3.TagHandler.Album;
            Audio = mp3.Audio.CalculateAudioSHA1();
            picture = mp3.TagHandler.Picture;
            duration = mp3.Audio.Duration;
            genre = mp3.TagHandler.Genre;
            year = mp3.TagHandler.Year;
            lyrics = mp3.TagHandler.Lyrics;
            folder = new DirectoryInfo(filePath).Parent.Name;

            if (album == null || album == String.Empty)
            {
                album = new DirectoryInfo(filePath).Parent.Name;
            }

            if (picture == null)
            {
                picture = Image.FromFile(@"..\..\Resources\Pictures\default.png");
            }

            //artist = GetArtist(mp3.TagHandler.Artist);
            //artist.AddSong(this);
            //album = GetAlbum(mp3.TagHandler.Album);
            path = filePath;
            isFavorite = false;
        }

        public bool CompareAudio(Song other)
        {
            byte[] sha1 = Audio;
            byte[] sha2 = other.Audio;
            for (int i = 0; i < sha1.Length && i < sha2.Length; i++)
            {
                if (sha1[i] != sha2[i])
                    return false;
            }
            return true;
        }

        public static bool ComapareSongs(Song song1, Song song2)
        {
            if (song1.CompareAudio(song2) &&
                song1.Title == song2.Title &&
                song1.Path == song2.Path)
                return true;
            else return false;
        }

        public bool CompareAudio(byte[] other)
        {
            byte[] sha1 = Audio;
            byte[] sha2 = other;
            for (int i = 0; i < sha1.Length && i < sha2.Length; i++)
            {
                if (sha1[i] != sha2[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Поиск исполнителя в Artists по его названию или создание нового, если его нет
        /// </summary>
        /// <param name="artistName">Название исполнителя</param>
        /// <returns></returns>
        //private Artist GetArtist(string artistName)
        //{
        //    foreach (Artist a in database.Artists.ArtistsCollection)
        //    {
        //        if (a.Title.Equals(artistName))
        //        { 
        //            return a;
        //        }
        //    }

        //    Artist artist = new Artist(artistName);
        //    database.AddArtist(artist);
        //    return artist;
        //}

        /// <summary>
        /// Поиск альбома в Albums по его названию или создание нового, если его нет
        /// </summary>
        /// <param name="albumName">Название альбома</param>
        /// <returns></returns>
        //private Album GetAlbum(string albumName)
        //{
        //    //foreach (Artist artist in Artists.Artists)
        //    //{
        //    //    if (artist.Title.)
        //    //}
        //    return null;
        //}

        private static BitmapImage GetImage(Image bm)
        {
            using (var ms = new MemoryStream())
            {
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                var image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
