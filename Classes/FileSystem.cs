using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public class FileSystem
    {
        public Directory CurrentDirectory { get; set; }

        public FileSystem()
        {
            CurrentDirectory = new Directory("root", 1, 5, 1024, null);
            if(!FatTable.isRootIntialized())
            {
				FatTable.setValue(CurrentDirectory.FirstCluster, -1);
				FatTable.writeFatTable();
			}
            else
            {
                CurrentDirectory.ReadDirectory();
            }
        }

        public string GetCurrentDirectory()
        {
            return CurrentDirectory.Filename;
        }

        public void AddFolder(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                Console.WriteLine("Folder name cannot be empty.");
                return;
            }

            if (CurrentDirectory.Search(folderName) != -1)
            {
                Console.WriteLine($"Folder '{folderName}' already exists.");
                return;
            }

            // Create a new directory entry for the folder
            Directory newFolder = new Directory(folderName, 1, 0, 0, CurrentDirectory);

            // Add the new directory entry to the current directory's table
            CurrentDirectory.DirectoryTable.Add(newFolder);

            // Write the current directory to the virtual disk
            CurrentDirectory.WriteDirectory();

            Console.WriteLine($"Folder '{folderName}' created successfully.");
        }

        public void NavigateToFolder(string folderName)
        {
            int index = CurrentDirectory.Search(folderName);

            if (index == -1)
            {
                Console.WriteLine($"Folder '{folderName}' not found.");
                return;
            }

            if (!(CurrentDirectory.DirectoryTable[index] is Directory folder))
            {
                Console.WriteLine($"'{folderName}' is not a folder.");
                return;
            }

            CurrentDirectory = folder;
            Console.WriteLine($"Navigated to folder '{folderName}'.");
        }

        public void NavigateUp()
        {
            if (CurrentDirectory.Parent == null)
            {
                Console.WriteLine("Already at the root directory.");
                return;
            }

            CurrentDirectory = CurrentDirectory.Parent;
            Console.WriteLine("Navigated up to the parent directory.");
        }

        public void DeleteFolder(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                Console.WriteLine("Folder name cannot be empty.");
                return;
            }

            int index = CurrentDirectory.Search(folderName);

            if (index == -1)
            {
                Console.WriteLine($"Folder '{folderName}' not found.");
                return;
            }

            if (!(CurrentDirectory.DirectoryTable[index] is Directory folder))
            {
                Console.WriteLine($"'{folderName}' is not a folder.");
                return;
            }

            folder.DeleteDirectory();
        }

        public string ShowCurrentPath()
        {
            StringBuilder path = new StringBuilder();
            Directory current = CurrentDirectory;

            while (current != null)
            {
                path.Insert(0, current.Filename);
                if (current.Parent != null)
                    path.Insert(0, '/');
                current = current.Parent;
            }

            return path.ToString();
        }


    }

}
