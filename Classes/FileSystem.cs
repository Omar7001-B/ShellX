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
            if (!FatTable.isRootIntialized())
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
            return CurrentDirectory.FileName;
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
                Console.WriteLine($"Folder '{DirectoryEntry.FormateFileName(folderName)}' already exists.");
                return;
            }

            Directory newFolder = new Directory(folderName, 1, 0, 0, CurrentDirectory);
            CurrentDirectory.DirectoryTable.Add(newFolder);
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
            CurrentDirectory.ReadDirectory();
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
            CurrentDirectory.ReadDirectory();
            Console.WriteLine("Navigated up to the parent directory.");
        }

        public void NavigateToRoot()
        {
            Directory temp = CurrentDirectory;
            while (temp.Parent != null)
                temp = temp.Parent;
            CurrentDirectory = temp;
            CurrentDirectory.ReadDirectory();
            Console.WriteLine("Navigated to the root directory.");
        }

        public void DeleteFolder(string folderName)
        {
            string name = folderName;

            if(name == ".." || name == "root")
            {
				Console.WriteLine("Access is denied.");
				return;
			}

            int index = CurrentDirectory.Search(name);
            if (index == -1)
            {
                Console.WriteLine($"Directory '{name}' not found.");
                return;
            }

            DirectoryEntry entry = CurrentDirectory.DirectoryTable[index];
            if (entry is Directory directory)
            {
                CurrentDirectory.DirectoryTable.RemoveAt(index);
                CurrentDirectory.WriteDirectory();
                directory.DeleteDirectory();
                Console.WriteLine($"Directory '{name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"'{name}' is not a directory.");
            }
        }

        public string ShowCurrentPath()
        {
            StringBuilder path = new StringBuilder();
            Directory current = CurrentDirectory;

            while (current != null)
            {
                path.Insert(0, current.FileName);
                if (current.Parent != null)
                    path.Insert(0, '/');
                current = current.Parent;
            }

            return path.ToString();
        }

        public void ListDirectoryContents()
        {
            int numFiles = 0;
            int numDirs = 0;
            int usedSpace = 0;

            Console.WriteLine($"\n Directory of {ShowCurrentPath()}:\n");

            if(CurrentDirectory.Parent != null)
				Console.WriteLine($"<DIR>             ..");

            foreach (var entry in CurrentDirectory.DirectoryTable)
            {
                string type = (entry.FileAttribute == 1) ? "<DIR>" : "";
                string fileSize = (entry.FileAttribute == 1) ? "" : $"{entry.FileSize}B";
                Console.WriteLine($"{type,-6} {fileSize,-10} {entry.FileName}");
                numFiles += (entry.FileAttribute == 0) ? 1 : 0;
                numDirs += (entry.FileAttribute == 1) ? 1 : 0;
                usedSpace += entry.FileSize;
            }

            int freeSpace = 1024 * 1024 - usedSpace;
            Console.WriteLine($"   {numFiles} File(s)   {numDirs} Dir(s)   {usedSpace} bytes used");
            Console.WriteLine($"   {freeSpace} bytes free");
        }
    

    }

}
