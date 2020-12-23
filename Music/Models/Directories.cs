using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Directories : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Directory> directories;
        private Directory selectedDirectory;
        private int selectedIndex;

        public ObservableCollection<Directory> DirectoriesCollection
        {
            get => directories;
            set
            {
                directories = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectoriesCollection)));
            }
        }

        public Directory SelectedDirectory
        {
            get => selectedDirectory;
            set
            {
                selectedDirectory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDirectory)));
            }
        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public Directories()
        {
            directories = new ObservableCollection<Directory>();
        }

        public void AddDirectory(string dirPath)
        {
            if (!ContainsDirectory(dirPath))
                directories.Add(new Directory(dirPath));
        }

        public void RemoveDirectory(string dirPath)
        {
            if (ContainsDirectory(dirPath))
                directories.Remove(FindDirectory(dirPath));
        }

        private bool ContainsDirectory(string dirPath)
        {
            foreach (Directory d in directories)
            {
                if (d.Path == dirPath)
                    return true;
            }
            return false;
        }

        private Directory FindDirectory(string dirPath)
        {
            foreach (Directory d in directories)
            {
                if (d.Path == dirPath)
                    return d;
            }
            return null;
        }
    }
}
