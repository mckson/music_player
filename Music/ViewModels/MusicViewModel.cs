using MaterialDesignThemes.Wpf;
using Music.Other;
using Music.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Music.ViewModels
{
    public class MusicViewModel : INotifyPropertyChanged
    {
        public MusicViewModel(Database database, MusicView musicView, MusicSecondView musicSecondView)
        {
            //Передавть информацию об окнах не через View коснтрукторы, а через MV кострукторы
            this.database = database;
            this.musicView = musicView;
            this.musicSecondView = musicSecondView;
            menuItems = GenerateMenuItems();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<MenuItem> menuItems;
        private MenuItem selectedItem;
        private int selectedIndex;

        private MusicView musicView;
        private MusicSecondView musicSecondView;

        private Database database;
        public Database Database
        {
            get => database;
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get => menuItems;
            set
            {
                menuItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItems)));
            }
        }

        public MenuItem SelectedItem
        {
            get => selectedItem;
            set
            {
                if (value == null || value.Equals(selectedItem)) return;

                selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
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

        private ObservableCollection<MenuItem> GenerateMenuItems()
        {
            return new ObservableCollection<MenuItem>
            {
                new MenuItem("All songs", new AllSongsView { DataContext = new AllSongsViewModel(database) }, new PackIcon{ Kind = PackIconKind.MusicNoteOutline }),
                new MenuItem("Playlists", new PlaylistsView { DataContext = new PlaylistsViewModel(database) }, new PackIcon{ Kind = PackIconKind.PlaylistMusicOutline }),
                new MenuItem("Favourites", new FavouritesView{ DataContext = new FavouritesViewModel(database) }, new PackIcon{ Kind = PackIconKind.HeartOutline }),
                new MenuItem("Settings", null, new PackIcon{ Kind = PackIconKind.Settings })
            };
        }
    }
}
