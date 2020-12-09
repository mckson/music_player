using Music.Models;
using Music.Other;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.ViewModels
{
    public class AllSongsViewModel
    {
        private Database database;
        private ObservableCollection<IGrouping<string, Song>> albums;
        private ObservableCollection<IGrouping<string, Song>> artists;
        private ObservableCollection<IGrouping<string, Song>> folders;

        public ObservableCollection<IGrouping<string, Song>> Albums { get => albums; }
        public ObservableCollection<IGrouping<string, Song>> Artists { get => artists; }
        public ObservableCollection<IGrouping<string, Song>> Folders { get => folders; }


        public Database Database { get => database; }

        public AllSongsViewModel(Database database)
        {
            this.database = database;
            albums = new ObservableCollection<IGrouping<string, Song>>();
            artists = new ObservableCollection<IGrouping<string, Song>>();
            folders = new ObservableCollection<IGrouping<string, Song>>();

            var albumsGrouping = from song in database.Songs.SongsCollection
                                 group song by song.Album;

            var artistsGrouping = from song in database.Songs.SongsCollection
                                  group song by song.Artist;

            var foldersGrouping = from song in database.Songs.SongsCollection
                                  group song by song.Folder;

            foreach (IGrouping<string, Song> s in albumsGrouping)
                albums.Add(s);
            foreach (IGrouping<string, Song> s in artistsGrouping)
                artists.Add(s);
            foreach (IGrouping<string, Song> s in foldersGrouping)
                folders.Add(s);
        }
    }
}
