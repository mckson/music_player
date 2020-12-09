using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Other
{
    public class MenuItem : INotifyPropertyChanged
    {
        private string name;
        private object content;
        private PackIcon packIcon;

        public MenuItem(string name, object content, PackIcon packIcon)
        {
            this.name = name;
            this.content = content;
            this.packIcon = packIcon;
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public object Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public PackIcon PackIcon
        {
            get => packIcon;
            set
            {
                packIcon = value;
                OnPropertyChanged(nameof(PackIcon));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (prop != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
