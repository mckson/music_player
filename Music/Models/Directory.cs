using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Directory : INotifyPropertyChanged
    {
        private string path; 

        public string Path
        {
            get => path;
            set
            {
                path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));
            }
        }

        public Directory()
        {

        }

        public Directory(string path)
        {
            this.path = path;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
