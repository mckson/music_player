using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Artist : SongSet, INotifyPropertyChanged
    {
        private string title;
        private string description;

        public int Count { get => songs.Count; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public Artist()
            : base() { }

        public Artist(string description)
            : base()
        {
            this.description = description;
        }

        public Artist(string description, ObservableCollection<Song> songs, ObservableCollection<byte[]> songsAudio)
            : base(songs, songsAudio)
        {
            this.description = description;
        }
    }
}
