using Music.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Music.ViewModels
{
    public class SettingsViewModel
    {
        private Database database;

        public Database Database { get => database; }

        public AnotherCommandImplementation DeleteDirectoryCommand { get; set; }
        public AnotherCommandImplementation AddDirectoryCommand { get; set; }

        public SettingsViewModel(Database database)
        {
            this.database = database;

            DeleteDirectoryCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if (Database.Directories.SelectedDirectory != null)
                    {
                        if (Database.Directories.SelectedDirectory.Path != @"C:\Users\maxim\Downloads")
                            Database.Directories.RemoveDirectory(Database.Directories.SelectedDirectory.Path);
                        else
                        {
                            MessageBox.Show("You cannot delete this main folder");
                        }
                    }
                }
                );

            AddDirectoryCommand = new AnotherCommandImplementation(
                _ =>
                {
                    FolderBrowserDialog dlg = new FolderBrowserDialog();
                    
                    dlg.ShowDialog();


                    var path = dlg.SelectedPath;
                    if (path != null && path != string.Empty)
                        Database.Directories.AddDirectory(path);
                }
                );
        }
    }
}
