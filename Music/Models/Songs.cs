using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Songs : INotifyPropertyChanged
    {
        private ObservableCollection<Song> songs;
        private Song selectedSong;                  //for ListBox, DataGrid binding and other bindings with selection
        private int selectedIndex;                  //the same as 

        public Song SelectedSong
        {
            get => selectedSong;
            set
            {
                selectedSong = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSong)));
            }
        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public ObservableCollection<Song> SongsCollection
        {
            get => songs;
            set
            {
                songs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Songs)));
            }
        }

        public Songs()
            : this(null) { }

        public Songs(ObservableCollection<Song> songs)
        {
            if (songs == null)
                this.songs = new ObservableCollection<Song>();
            else
                this.songs = songs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool AddSong(Song song)
        {
            if (ContainsAudio(song))
                return false;

            songs.Add(song);
            return true;
        }

        public bool RemoveSong(Song song)
        {
            if (!songs.Contains(song))
                return false;

            songs.Remove(song);
            return true;
        }

        private bool ContainsAudio(Song song)
        {
            foreach (Song s in songs)
            {
                if (s.CompareAudio(song))
                    return true;
            }
            return false;
        }
    }
}
