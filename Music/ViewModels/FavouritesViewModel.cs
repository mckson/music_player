using Music.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.ViewModels
{
    public class FavouritesViewModel
    {
        private Database database;
        public Database Database
        {
            get => database;
            set => database = value;
        }
        public FavouritesViewModel(Database database)
        {
            this.database = database;
            database.CurrentSongSet = database.Songs;
        }
    }
}
