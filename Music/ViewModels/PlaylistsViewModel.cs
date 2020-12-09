using Music.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.ViewModels
{
    public class PlaylistsViewModel
    {
        private Database database;
        public Database Database
        {
            get => database;
            set => database = value;
        }
        public PlaylistsViewModel(Database database)
        {
            this.database = database;
        }
    }
}
