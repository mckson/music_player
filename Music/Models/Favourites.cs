using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Favourites : SongSet
    {
        public Favourites()
            : base() { }        //xml

        public Favourites(ObservableCollection<Song> songs, ObservableCollection<byte[]> songsAudio)
            : base(songs, songsAudio) { }

        public override bool AddSong(Song song)
        {
            if (ContainsAudio(song))
            {
                throw new Exception("Audio already in favourites");
                //return false;
            }

            songs.Add(song);
            song.IsFavorite = true;

            //если добавляется новый трек- добавить его Audio
            //если добавляется вначале, при сравнии Audio из Songs и содержимого SongsAudio, еще одно byte[] добавлять не надо (уже есть) 
            if (!ContainsAudioSHA(song.Audio))
                SongsAudio.Add(song.Audio);

            return true;
        }

        public override bool RemoveSong(Song song)
        {
            if (!songs.Contains(song))
                return false;

            song.IsFavorite = false;
            songs.Remove(song);
            //byte[] audio = FindSongAudio(song.Audio);
            //if (audio != null)


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
    }
}
