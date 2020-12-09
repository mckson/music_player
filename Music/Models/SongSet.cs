using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Music.Models
{
    public  abstract class SongSet : INotifyPropertyChanged
    {
        protected internal ObservableCollection<Song> songs;
        protected internal Song selectedSong;                  //for ListBox, DataGrid binding and other bindings with selection
        protected internal int selectedIndex;                  //the same as 

        [XmlIgnore]
        public Song SelectedSong
        {
            get => selectedSong;
            set
            {
                selectedSong = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSong)));
            }
        }

        [XmlIgnore]
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        [XmlIgnore]
        public ObservableCollection<Song> Songs
        {
            get => songs;
            set
            {
                songs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Songs)));
            }
        }

        //набор байтов возвращаемый встроенной функцией
        public ObservableCollection<byte[]> SongsAudio
        {
            get;
            set;
        }

        public SongSet()
            : this(null, null) { }

        public SongSet(ObservableCollection<Song> songs, ObservableCollection<byte[]> songsAudio)
        {
            if (songs == null)
                this.songs = new ObservableCollection<Song>();
            else
                this.songs = songs;

            if (songsAudio == null)
                SongsAudio = new ObservableCollection<byte[]>();
            else
                SongsAudio = songsAudio;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool AddSong(Song song)
        {
            if (ContainsAudio(song))
            {
                throw new Exception("Playlist already contains this song");
                //return false;
            }

            songs.Add(song);

            //если добавляется новый трек- добавить его Audio
            //если добавляется вначале, при сравнии Audio из Songs и содержимого SongsAudio, еще одно byte[] добавлять не надо (уже есть) 
            if (!ContainsAudioSHA(song.Audio))
                SongsAudio.Add(song.Audio);

            return true;
        }

        public virtual bool RemoveSong(Song song)
        {
            if (!songs.Contains(song))
                return false;

            songs.Remove(song);

            //поиск Audio по индексу
            int index = -1;
            for (int i = 0; i < SongsAudio.Count; ++i)
            {
                if (song.CompareAudio(SongsAudio[i]))
                {
                    index = i;
                    break;
                }

            }

            if (index != -1)
                SongsAudio.RemoveAt(index);

            return true;
        }

        protected internal bool ContainsAudio(Song song)
        {
            foreach (Song s in songs)
            {
                if (s.CompareAudio(song))
                    return true;
            }
            return false;
        }

        //проверка на содержание в SongsAudio занчения songAudio
        protected internal bool ContainsAudioSHA(byte[] songAudio)
        {
            foreach (byte[] audio in SongsAudio)
            {
                byte[] sha1 = songAudio;
                byte[] sha2 = audio;
                bool isEqual = true;
                for (int i = 0; i < sha1.Length && i < sha2.Length; i++)
                {
                    if (sha1[i] != sha2[i])
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (isEqual)
                    return true;
            }
            return false;
        }

        //protected internal byte[] FindSongAudio(byte[] songAudio)
        //{
        //    byte[] sha1 = songAudio;
        //    foreach (byte[] audio in SongsAudio)
        //    {
        //        byte[] sha2 = audio;
        //        bool isEqual = true;
        //        for (int i = 0; i < sha1.Length && i < sha2.Length; i++)
        //        {
        //            if (sha1[i] != sha2[i])
        //            {
        //                isEqual = false;
        //                break;
        //            }
        //        }
        //        if (isEqual)
        //            return sha1;
        //    }
        //    return null;
        //}
    }
}
