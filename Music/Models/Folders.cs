using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Models
{
    public class Folders : INotifyPropertyChanged
    {
        private ObservableCollection<Folder> folders;
        private Folder selectedFolder;
        private int selectedFolderIndex;

        public ObservableCollection<Folder> FoldersCollection
        {
            get => folders;
            set
            {
                folders = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoldersCollection)));
            }
        }

        public Folder SelectedFolder
        {
            get => selectedFolder;
            set
            {
                selectedFolder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFolder)));
            }
        }

        public int SelectedFolderIndex
        {
            get => selectedFolderIndex;
            set
            {
                selectedFolderIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFolderIndex)));
            }
        }

        public Folders()
            : this(null) { }

        public Folders(ObservableCollection<Folder> folders)
        {
            if (folders != null)
                this.folders = folders;
            else
                this.folders = new ObservableCollection<Folder>();
        }

        public bool AddFolder(Folder folder)
        {
            if (ContainsFolder(folder))
                return false;
            else
            {
                folders.Add(folder);
                return true;
            }
        }

        public bool RemoveFolder(Folder folder)
        {
            if (!folders.Contains(folder))
                return false;
            else
            {
                folders.Remove(folder);
                return true;
            }
        }

        private bool ContainsFolder(Folder folder)
        {
            foreach (Folder fl in folders)
            {
                if (fl.Title.ToUpper() == folder.Title.ToUpper())
                {
                    throw new Exception("Folder with such name already exists");
                    //return false;
                }
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
