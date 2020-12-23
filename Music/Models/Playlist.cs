using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Playlist : SongSet, INotifyPropertyChanged
    {
        private string title;
        private string description;
        
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

        public Playlist()
            : base() { }        //xml

        public Playlist(string title)
            : base() 
        {
            this.title = title;       
        }

        public Playlist(string title, ObservableCollection<Song> songs, ObservableCollection<byte[]> songsAudio)
            : base(songs, songsAudio)
        {
            this.title = title;
        }
    }
}
