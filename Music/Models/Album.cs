using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Album : SongSet, INotifyPropertyChanged
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

        public Album()
            : base()
        { }

        public Album(string description)
            : base()
        {
            this.description = description;
        }

        public Album(string description, ObservableCollection<Song> songs, ObservableCollection<byte[]> songsAudio)
            : base(songs, songsAudio)
        {
            this.description = description;
        }
    }
}
