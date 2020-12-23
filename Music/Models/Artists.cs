using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Artists : INotifyPropertyChanged
    {
        private ObservableCollection<Artist> artists;
        private Artist selectedArtist;
        private int selectedArtistIndex;

        public ObservableCollection<Artist> ArtistsCollection
        {
            get => artists;
            set
            {
                artists = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArtistsCollection)));
            }
        }

        public Artist SelectedArtist
        {
            get => selectedArtist;
            set
            {
                selectedArtist = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedArtist)));
            }
        }

        public int SelectedArtistIndex
        {
            get => selectedArtistIndex;
            set
            {
                selectedArtistIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedArtistIndex)));
            }
        }

        public Artists()
            : this(null) { }

        public Artists(ObservableCollection<Artist> artists)
        {
            if (artists != null)
                this.artists = artists;
            else
                this.artists = new ObservableCollection<Artist>();
        }

        public bool AddArtist(Artist artist)
        {
            if (ContainsArtist(artist))
                return false;
            else
            {
                artists.Add(artist);
                return true;
            }
        }

        public bool RemoveArtist(Artist artist)
        {
            if (!artists.Contains(artist))
                return false;
            else
            {
                artists.Remove(artist);
                return true;
            }
        }

        private bool ContainsArtist(Artist artist)
        {
            foreach (Artist ar in artists)
            {
                if (ar.Title.ToUpper() == artist.Title.ToUpper())
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
