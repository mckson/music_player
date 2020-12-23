using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Albums : INotifyPropertyChanged
    {
        private ObservableCollection<Album> albums;
        private Album selectedAlbum;
        private int selectedAlbumIndex;

        public ObservableCollection<Album> AlbumsCollection
        {
            get => albums;
            set
            {
                albums = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlbumsCollection)));
            }
        }

        public Album SelectedAlbum
        {
            get => selectedAlbum;
            set
            {
                selectedAlbum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAlbum)));
            }
        }

        public int SelectedAlbumIndex
        {
            get => selectedAlbumIndex;
            set
            {
                selectedAlbumIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAlbumIndex)));
            }
        }

        public Albums()
            : this(null) { }

        public Albums(ObservableCollection<Album> albums)
        {
            if (albums != null)
                this.albums = albums;
            else
                this.albums = new ObservableCollection<Album>();
        }

        public bool AddAlbum(Album album)
        {
            if (ContainsAlbum(album))
                return false;
            else
            {
                albums.Add(album);
                return true;
            }
        }

        public bool RemoveAlbum(Album album)
        {
            if (!albums.Contains(album))
                return false;
            else
            {
                albums.Remove(album);
                return true;
            }
        }

        private bool ContainsAlbum(Album album)
        {
            foreach (Album al in albums)
            {
                if (al.Title.ToUpper() == album.Title.ToUpper())
                {
                    throw new Exception("Album with such name already exists");
                    //return false;
                }
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
