using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Lib;
using Music.Other;
using System.Drawing;   //for image

namespace Music.Models
{
    public class Song
    {
        private string title;
        private string artist;
        private string album;
        private string path;
        private bool isFavorite;
        private Image picture;
        private TimeSpan? duration;
        private string genre;
        private string year;
        private string lyrics;
        private string folder;

        public string Title
        {
            get => title;
            set
            {
                title = value;
            }
        }

        public string Artist
        {
            get => artist;
            set
            {
                artist = value;
            }
        }
        public string Album
        {
            get => album;
            set
            {
                album = value;
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
            }
        }

        public Image Picture
        {
            get => picture;
            set => picture = value;
        }

        public TimeSpan? Duration
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
            set => lyrics = value;
        }

        public string Folder
        {
            get => folder;
            set => folder = value;
        }

        //значение, которое вернет хэш-функция из id3lib
        public byte[] Audio { get; set; }

        public Song() { }       //xml

        public Song(string filePath)
        {
            Mp3File mp3 = new Mp3File(filePath);
            //this.database = database;

            title = mp3.TagHandler.Title;
            artist = mp3.TagHandler.Artist;
            album = mp3.TagHandler.Album;
            Audio = mp3.Audio.CalculateAudioSHA1();
            picture = mp3.TagHandler.Picture;
            duration = mp3.TagHandler.Length;
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
                picture = Image.FromFile(@"..\..\Resources\default.png");
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
    }
}
