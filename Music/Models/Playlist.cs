using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Playlist : SongSet
    {
        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
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
