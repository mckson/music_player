using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Folder : SongSet, INotifyPropertyChanged
    {
        private string title;

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

        public Folder()
            : base() { }

        public Folder(string title)
            : base()
        {
            this.title = title;
        }

        public Folder(string title, ObservableCollection<Song> songs, ObservableCollection<byte[]> songsAudio)
            : base(songs, songsAudio)
        {
            this.title = title;
        }
    }
}
