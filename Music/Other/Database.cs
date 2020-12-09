using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Music.Models;

namespace Music.Other
{
    public class Database
    {
        //private const string songsPath = @"..\..\Database\Songs.xml";
        private const string playlistsPath = @"..\..\Database\Playlists.xml";
        private const string favouritesPath = @"..\..\Database\Favourites.xml";
        //private const string idPath = @"..\..\Database\Id.xml";

        private List<string> directoryPathes;
        private Playlists playlists;
        private Songs songs;
        private Favourites favourites;

        public Playlists Playlists { get => playlists; }
        public Songs Songs { get => songs; }
        public Favourites Favourites { get => favourites; }
        //public uint SongId;

        public Database()
        {
            directoryPathes = new List<string>();
            directoryPathes.Add(@"C:\Users\maxim\Downloads");

            playlists = DeserializePlaylists();
            //playlists = new Playlists();
            songs = new Songs();
            //songs = new Songs();
            favourites = DeserializeFavourites();
            //favourites = new Favourites();

            SearchLocalSongs(directoryPathes);
            FullfillPlaylistsWithSongs(playlists, songs);
            FullfillFavouritesWithSongs(favourites, songs);
        }

        public bool AddSongs(Song song)
        {
            return songs.AddSong(song);
        }

        //private void SerializeSongs()
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(Songs));
        //    using (FileStream fs = new FileStream(songsPath, FileMode.Create))
        //    {
        //        serializer.Serialize(fs, songs);
        //    }
        //}

        private void SerializePlaylists()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Playlists));
            using (FileStream fs = new FileStream(playlistsPath, FileMode.Create))
            {
                serializer.Serialize(fs, playlists);
            }
        }

        private void SerializeFavourites()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Favourites));
            using (FileStream fs = new FileStream(favouritesPath, FileMode.Create))
            {
                serializer.Serialize(fs, favourites);               
            }
        }

        //private void SerializeId()
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(uint));
        //    using (FileStream fs = new FileStream(idPath, FileMode.OpenOrCreate))
        //    {
        //        serializer.Serialize(fs, SongId);
        //    }
        //}

        //private uint DeserializeId()
        //{
        //    uint SongId = new Songs();
        //    XmlSerializer serializer = new XmlSerializer(typeof(Songs));
        //    using (FileStream fs = new FileStream(songsPath, FileMode.OpenOrCreate))
        //    {
        //        songs = (Songs)serializer.Deserialize(fs);
        //    }
        //    return songs;
        //}

        //private Songs DeserializeSongs()
        //{
        //    Songs songs = new Songs(); 
        //    XmlSerializer serializer = new XmlSerializer(typeof(Songs));
        //    using (FileStream fs = new FileStream(songsPath, FileMode.OpenOrCreate))
        //    {
        //        songs = (Songs)serializer.Deserialize(fs);
        //    }
        //    return songs;
        //}

        private Playlists DeserializePlaylists()
        {
            Playlists playlists = new Playlists();
            XmlSerializer serializer = new XmlSerializer(typeof(Playlists));
            using (FileStream fs = new FileStream(playlistsPath, FileMode.OpenOrCreate))
            {
                playlists = (Playlists)serializer.Deserialize(fs);
            }
            return playlists;
        }

        private Favourites DeserializeFavourites()
        {
            Favourites favourites = new Favourites();
            XmlSerializer serializer = new XmlSerializer(typeof(Favourites));
            using (FileStream fs = new FileStream(favouritesPath, FileMode.OpenOrCreate))
            {
                favourites = (Favourites)serializer.Deserialize(fs);
            }
            return favourites;
        }

        private void FullfillPlaylistsWithSongs(Playlists playlists, Songs songs)
        {
            //foreach (Playlist pl in playlists.PlaylistsCollection)
            //{
            //    foreach  (byte[] audio in pl.SongsAudio)
            //    {
            //        foreach (Song s in songs.SongsCollection)
            //        {
            //            if (s.CompareAudio(audio))
            //                pl.AddSong(s);
            //        }
            //    }
            //}

            for (int i = 0; i < playlists.PlaylistsCollection.Count; ++i)
            {
                Playlist playlist = playlists.PlaylistsCollection[i];
                for (int j = 0; j < playlist.SongsAudio.Count; ++j)
                {
                    byte[] audio = playlist.SongsAudio[j];
                    for (int k = 0; k < songs.SongsCollection.Count; ++k)
                    {
                        Song song = songs.SongsCollection[k];

                        if (song.CompareAudio(audio))
                            playlist.AddSong(song);
                    }
                }
            }
        }

        private void FullfillFavouritesWithSongs(Favourites favourites, Songs songs)
        {
            //foreach (Playlist pl in playlists.PlaylistsCollection)
            //{
            //    foreach  (byte[] audio in pl.SongsAudio)
            //    {
            //        foreach (Song s in songs.SongsCollection)
            //        {
            //            if (s.CompareAudio(audio))
            //                pl.AddSong(s);
            //        }
            //    }
            //}


            for (int i = 0; i < favourites.SongsAudio.Count; ++i)
            {
                byte[] audio = favourites.SongsAudio[i];
                for (int j = 0; j < songs.SongsCollection.Count; ++j)
                {
                    Song song = songs.SongsCollection[j];

                    if (song.CompareAudio(audio))
                        favourites.AddSong(song);
                }
            }
            
        }

        public void SaveChanges()
        {
            //SerializeSongs();
            SerializePlaylists();
            SerializeFavourites();
            //SerializeId();
        }

        //ДОБАВИТЬ УДАЛЕНИЕ ОТСУТСВУЮЩИХ ПЕСЕН ИЗ SONGS
        //ДОБАВЛЕНИЕ ТЕХ, ЧТО НЕТ В SONGS
        private void SearchLocalSongs(List<string> directoryPathes)
        {
            foreach(string directoryPath in directoryPathes)
            {
                foreach(string filePath in Directory.GetFiles(directoryPath, "*.mp3"))
                {
                    songs.AddSong(new Song(filePath));  //ИЗМЕНИТЬ СРАВНЕНИЕ ПЕСЕН!!!!!!
                }
            }
        }
    }
}
