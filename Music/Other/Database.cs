using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Music.Models;

namespace Music.Other
{
    public class Database : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private const string playlistsPath = @"..\..\Database\Playlists.xml";
        private const string favouritesPath = @"..\..\Database\Favourites.xml";
        private const string albumsPath = @"..\..\Database\Albums.xml";
        private const string artistsPath = @"..\..\Database\Artists.xml";
        private const string foldersPath = @"..\..\Database\Folders.xml";
        private const string direcoriesPath = @"..\..\Database\Directories.xml";

        private Directories directories;
        private Playlists playlists;
        private Songs songs;
        private Favourites favourites;
        private Albums albums;
        private Artists artists;
        private Folders folders;

        private SongSet currentSongSet;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public SongSet CurrentSongSet
        {
            get => currentSongSet;
            set
            {
                if (currentSongSet == value) return;
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(CurrentSongSet)));
                currentSongSet = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSongSet)));
            }
        }
        public Playlists Playlists { get => playlists; }
        public Songs Songs { get => songs; }
        public Favourites Favourites { get => favourites; }
        public Albums Albums { get => albums; }
        public Artists Artists { get => artists; }
        public Folders Folders { get => folders; }

        public Directories Directories { get => directories; }

        public Database()
        {
            directories = DeserializeDirectories();
            directories.DirectoriesCollection.CollectionChanged += OnDirectoriesCollectionChanged;

            playlists = DeserializePlaylists();
            songs = new Songs();
            favourites = DeserializeFavourites();

            SearchLocalSongs(directories.DirectoriesCollection);
            FullfillPlaylistsWithSongs(playlists, songs);
            FullfillFavouritesWithSongs(favourites, songs);
            
            albums = DeserializeAlbums();
            SyncronizeAlbums();
            artists = DeserializeArtists();
            SyncronizeArtists();
            folders = DeserializeFolders();
            SyncronizeFolders();

            if (playlists.SelectedPlaylist == null)
            {
                if (playlists.PlaylistsCollection.Count > 0)
                    playlists.SelectedPlaylist = playlists.PlaylistsCollection[0];
            }

            if (albums.SelectedAlbum == null)
            {
                if (albums.AlbumsCollection.Count > 0)
                    albums.SelectedAlbum = albums.AlbumsCollection[0];
            }

            if (artists.SelectedArtist == null)
            {
                if (artists.ArtistsCollection.Count > 0)
                    artists.SelectedArtist = artists.ArtistsCollection[0];
            }

            if (folders.SelectedFolder == null)
            {
                if (folders.FoldersCollection.Count > 0)
                    folders.SelectedFolder = folders.FoldersCollection[0];
            }

            if (favourites.SelectedSong == null)
            {
                if (favourites.Songs.Count > 0)
                    favourites.SelectedSong = favourites.Songs[0];
            }

            CurrentSongSet = songs;
        }

        private void OnDirectoriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //while (songs.Songs.Count != 0)
            //    songs.RemoveSong(songs.Songs[0]);

            //SearchLocalSongs(directories.DirectoriesCollection);

            //FullfillFavouritesWithSongs(favourites, songs);
            //FullfillPlaylistsWithSongs(playlists, songs);
            //SyncronizeAlbums();
            //SyncronizeArtists();
            //SyncronizeFolders();
            Syncronize();
        }

        public /*async*/ void Syncronize()
        {
            while (songs.Songs.Count != 0)
                songs.RemoveSong(songs.Songs[0]);

            /*await*/ /*Task.Run(() => */SearchLocalSongs(directories.DirectoriesCollection);

            FullfillFavouritesWithSongs(favourites, songs);
            FullfillPlaylistsWithSongs(playlists, songs);
            SyncronizeAlbums();
            SyncronizeArtists();
            SyncronizeFolders();
        }

        public bool AddSongs(Song song)
        {
            return songs.AddSong(song);
        }

        private void SerializeAlbums()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Albums));
            using (FileStream fs = new FileStream(albumsPath, FileMode.Create))
            {
                serializer.Serialize(fs, albums);
            }
        }

        private void SerializeArtists()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Artists));
            using (FileStream fs = new FileStream(artistsPath, FileMode.Create))
            {
                serializer.Serialize(fs, artists);
            }
        }

        private void SerializeFolders()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Folders));
            using (FileStream fs = new FileStream(foldersPath, FileMode.Create))
            {
                serializer.Serialize(fs, folders);
            }
        }

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

        private void SerializeDirectories()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Directories));
            using (FileStream fs = new FileStream(direcoriesPath, FileMode.Create))
            {
                serializer.Serialize(fs, directories);
            }
        }

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

        private Albums DeserializeAlbums()
        {
            Albums albums = new Albums();
            XmlSerializer serializer = new XmlSerializer(typeof(Albums));
            using (FileStream fs = new FileStream(albumsPath, FileMode.OpenOrCreate))
            {
                albums = (Albums)serializer.Deserialize(fs);
            }
            return albums;
        }

        private Artists DeserializeArtists()
        {
            Artists artists = new Artists();
            XmlSerializer serializer = new XmlSerializer(typeof(Artists));
            using (FileStream fs = new FileStream(artistsPath, FileMode.OpenOrCreate))
            {
                artists = (Artists)serializer.Deserialize(fs);
            }
            return artists;
        }

        private Folders DeserializeFolders()
        {
            Folders folders = new Folders();
            XmlSerializer serializer = new XmlSerializer(typeof(Folders));
            using (FileStream fs = new FileStream(foldersPath, FileMode.OpenOrCreate))
            {
                folders = (Folders)serializer.Deserialize(fs);
            }
            return folders;
        }

        private Directories DeserializeDirectories()
        {
            Directories directories = new Directories();
            XmlSerializer serializer = new XmlSerializer(typeof(Directories));
            using (FileStream fs = new FileStream(direcoriesPath, FileMode.OpenOrCreate))
            {
                directories = (Directories)serializer.Deserialize(fs);
            }
            return directories;
        }

        private void FullfillPlaylistsWithSongs(Playlists playlists, Songs songs)
        {
            for (int i = 0; i < playlists.PlaylistsCollection.Count; ++i)
            {
                Playlist playlist = playlists.PlaylistsCollection[i];
                playlist.Songs.Clear();
                for (int j = 0; j < playlist.SongsAudio.Count; ++j)
                {
                    byte[] audio = playlist.SongsAudio[j];
                    for (int k = 0; k < songs.Songs.Count; ++k)
                    {
                        Song song = songs.Songs[k];

                        if (song.CompareAudio(audio))
                            playlist.AddSong(song);
                    }
                }
            }
        }

        private void FullfillFavouritesWithSongs(Favourites favourites, Songs songs)
        {
            favourites.Songs.Clear();
            for (int i = 0; i < favourites.SongsAudio.Count; ++i)
            {
                byte[] audio = favourites.SongsAudio[i];
                for (int j = 0; j < songs.Songs.Count; ++j)
                {
                    Song song = songs.Songs[j];

                    if (song.CompareAudio(audio))
                        favourites.AddSong(song);
                }
            }
        }

        public void SaveChanges()
        {
            SerializePlaylists();
            SerializeFavourites();
            SerializeAlbums();
            SerializeArtists();
            SerializeFolders();
            SerializeDirectories();
        }

        private void SearchLocalSongs(ObservableCollection<Models.Directory> directories)
        {
            foreach (Models.Directory d in directories)
            {
                foreach (string filePath in System.IO.Directory.GetFiles(d.Path, "*.mp3"))
                {
                    //songs.AddSong(new Song(filePath));  

                    bool isSongExists = false;
                    Song termSong = new Song(filePath);
                    //обновление существующих песен и добавление новых
                    for (int i = songs.Songs.Count() - 1; i >= 0; --i)
                    {
                        if (termSong.CompareAudio(songs.Songs[i]))
                        {
                            isSongExists = true;
                        }
                    }

                    if (!isSongExists)
                    {
                        songs.AddSong(termSong);
                    }
                }
            }

            
                //удаление песни, которой больше нет
                for (int i = songs.Songs.Count - 1; i >= 0; --i)
                {
                    bool isSongExists = false;
                    foreach (Models.Directory d in directories)
                    {
                        foreach (string filePath in System.IO.Directory.GetFiles(d.Path, "*.mp3"))
                        {
                            Song termSong = new Song(filePath);

                            //если песня есть в обновленной версии отмечаем это
                            if (Song.ComapareSongs(termSong, songs.Songs[i]))
                                isSongExists = true;
                        }
                    }

                    //если же ее нет, удаляем из списка песен
                    if (!isSongExists)
                    {
                        songs.RemoveSong(songs.Songs[i]);
                    }
                }
            
        }

                

        private Albums GenerateAlbums()
        {
            var albumsGrouping = from song in songs.Songs
                                 group song by song.Album;

            Albums albums = new Albums();
            foreach (IGrouping<string, Song> s in albumsGrouping)
            {
                albums.AddAlbum(new Album() { Title = s.Key, Songs = new ObservableCollection<Song>(s) });
            }

            return albums;   
        }

        private Artists GenerateArtists()
        {
            var artistsGrouping = from song in songs.Songs
                                  group song by song.Artist;

            Artists artists = new Artists();
            foreach(IGrouping<string, Song> s in artistsGrouping)
            {
                artists.AddArtist(new Artist() { Title = s.Key, Songs = new ObservableCollection<Song>(s) });
            }

            return artists;
        }

        private Folders GenerateFolders()
        {
            var foldersGrouping = from song in songs.Songs
                                  group song by song.Folder;

            Folders folders = new Folders();
            foreach (IGrouping<string, Song> s in foldersGrouping)
                folders.AddFolder(new Folder() { Title = s.Key, Songs = new ObservableCollection<Song>(s) });

            return folders;
        }

        public void SyncronizeAlbums()
        {
            var albumsGrouping = from song in songs.Songs
                                 group song by song.Album;

            //albums.AlbumsCollection.Clear();

            //foreach (IGrouping<string, Song> s in albumsGrouping)
            //{
            //    albums.AddAlbum(new Album() { Title = s.Key, Songs = new ObservableCollection<Song>(s) });
            //}

            
            
            //for (int i = 0; i < albums.AlbumsCollection.Count && i < albumsGrouping.Count(); ++i)
            //{
            //    albums.AddAlbum(new Album() { Title = albumsGrouping.ElementAt(i).Key, Description = albums.AlbumsCollection[i].Description, Songs = new ObservableCollection<Song>(albumsGrouping.ElementAt(i)) });
            //    albums.RemoveAlbum(albums.AlbumsCollection[0]);
            //}


            //обновление существующих альбомов и добавление новых
            for (int i = albumsGrouping.Count() - 1; i >= 0; --i)
            {
                bool isAlbumExists = false;
                Album termAlbum = new Album() { Title = albumsGrouping.ElementAt(i).Key, Songs = new ObservableCollection<Song>(albumsGrouping.ElementAt(i)) };
                for (int j = albums.AlbumsCollection.Count - 1; j >= 0; --j)
                {
                    if (termAlbum.Title == albums.AlbumsCollection[j].Title)
                    {
                        isAlbumExists = true;
                        albums.AlbumsCollection[j].Songs = termAlbum.Songs; //если альбом есть, то обновляем список его песен
                    }
                }

                if (!isAlbumExists)
                {
                    albums.AddAlbum(termAlbum);
                }
            }

            //удаление альбома, которого больше нет
            for (int j = albums.AlbumsCollection.Count - 1; j >= 0; --j)
            {
                bool isAlbumExists = false;
                for (int i = albumsGrouping.Count() - 1; i >= 0; --i)
                {
                    Album termAlbum = new Album() { Title = albumsGrouping.ElementAt(i).Key, Songs = new ObservableCollection<Song>(albumsGrouping.ElementAt(i)) };

                    //если альбом есть в обновленной версии отмечаем это
                    if (albums.AlbumsCollection[j].Title == termAlbum.Title)
                        isAlbumExists = true;
                }

                //если же его нет, удаляем из списка альбомов
                if (!isAlbumExists)
                {
                    albums.AlbumsCollection[j].Songs.Clear();
                }
            }
        }

        public void SyncronizeArtists()
        {
            var artistsGrouping = from song in songs.Songs
                                  group song by song.Artist;

            //обновление существующих исполнителей и добавление новых
            for (int i = artistsGrouping.Count() - 1; i >= 0; --i)
            {
                bool isArtistExists = false;
                Artist termArtist = new Artist() { Title = artistsGrouping.ElementAt(i).Key, Songs = new ObservableCollection<Song>(artistsGrouping.ElementAt(i)) };
                for (int j = artists.ArtistsCollection.Count - 1; j >= 0; --j)
                {
                    if (termArtist.Title == artists.ArtistsCollection[j].Title)
                    {
                        isArtistExists = true;
                        artists.ArtistsCollection[j].Songs = termArtist.Songs; //если исполнитель есть, то обновляем список его песен
                    }
                }

                if (!isArtistExists)
                {
                    artists.AddArtist(termArtist);
                }
            }

            //удаление исполнителя, которого больше нет
            for (int j = artists.ArtistsCollection.Count - 1; j >= 0; --j)
            {
                bool isArtistExists = false;
                for (int i = artistsGrouping.Count() - 1; i >= 0; --i)
                {
                    Artist termArtist = new Artist() { Title = artistsGrouping.ElementAt(i).Key, Songs = new ObservableCollection<Song>(artistsGrouping.ElementAt(i)) };

                    //если альбом есть в обновленной версии отмечаем это
                    if (artists.ArtistsCollection[j].Title == termArtist.Title)
                        isArtistExists = true;
                }

                //если же его нет, удаляем из списка альбомов
                if (!isArtistExists)
                {
                    artists.ArtistsCollection[j].Songs.Clear();
                }
            }
        }

        public void SyncronizeFolders()
        {
            var foldersGrouping = from song in songs.Songs
                                  group song by song.Folder;

            //обновление существующих папок и добавление новых
            for (int i = foldersGrouping.Count() - 1; i >= 0; --i)
            {
                bool isFolderExists = false;
                Folder termFolder = new Folder() { Title = foldersGrouping.ElementAt(i).Key, Songs = new ObservableCollection<Song>(foldersGrouping.ElementAt(i)) };
                for (int j = folders.FoldersCollection.Count - 1; j >= 0; --j)
                {
                    if (termFolder.Title == folders.FoldersCollection[j].Title)
                    {
                        isFolderExists = true;
                        folders.FoldersCollection[j].Songs = termFolder.Songs; //если исполнитель есть, то обновляем список его песен
                    }
                }

                if (!isFolderExists)
                {
                    folders.AddFolder(termFolder);
                }
            }

            //удаление папки, которой больше нет
            for (int j = folders.FoldersCollection.Count - 1; j >= 0; --j)
            {
                bool isFolderExists = false;
                for (int i = foldersGrouping.Count() - 1; i >= 0; --i)
                {
                    Folder termFolder = new Folder() { Title = foldersGrouping.ElementAt(i).Key, Songs = new ObservableCollection<Song>(foldersGrouping.ElementAt(i)) };

                    //если папка есть в обновленной версии отмечаем это
                    if (folders.FoldersCollection[j].Title == termFolder.Title)
                        isFolderExists = true;
                }

                //если же ее нет, удаляем из списка альбомов
                if (!isFolderExists)
                {
                    folders.FoldersCollection[j].Songs.Clear();
                }
            }
        }
    }
}
