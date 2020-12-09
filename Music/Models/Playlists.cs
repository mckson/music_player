using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Playlists : INotifyPropertyChanged
    {
        private ObservableCollection<Playlist> playlists;
        private Playlist selectedPlaylist;
        private int selectedPlaylistIndex;

        public ObservableCollection<Playlist> PlaylistsCollection
        {
            get => playlists;
            set
            {
                playlists = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Playlists)));
            }
        }

        public Playlist SelectedPlaylist
        {
            get => selectedPlaylist;
            set
            {
                selectedPlaylist = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPlaylist)));
            }
        }

        public int SelectedPlaylistIndex
        {
            get => selectedPlaylistIndex;
            set
            {
                selectedPlaylistIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPlaylistIndex)));
            }
        }

        public Playlists()
            : this(null) { }

        public Playlists(ObservableCollection<Playlist> playlists)
        {
            if (playlists != null)
                this.playlists = playlists;
            else
                this.playlists = new ObservableCollection<Playlist>();
        }

        public bool AddPlaylist(Playlist playlist)
        {
            if (ContainsPlaylist(playlist))
                return false;
            else
            {
                playlists.Add(playlist);
                return true;
            }
        }

        public bool RemovePlaylist(Playlist playlist)
        {
            if (!playlists.Contains(playlist))
                return false;
            else
            {
                playlists.Remove(playlist);
                return true;
            }
        }

        private bool ContainsPlaylist(Playlist playlist)
        {
            foreach (Playlist pl in playlists)
            {
                if (pl.Title.ToUpper() == playlist.Title.ToUpper())
                {
                    throw new Exception("Playlist with such name already exists");
                    //return false;
                }
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
