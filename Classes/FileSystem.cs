using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public class FileSystem
    {

        private Directory _currentDirectory;

		public Directory CurrentDirectory
        {
            get => _currentDirectory;
            set { _currentDirectory = value;  _currentDirectory?.ReadEntryFromDisk(); }
        }

        public string ExportPath { get; set; }
        public string ImportPath { get; set; }

        public FileSystem()
        {
            CurrentDirectory = new Directory("root", 1, 5, 1024, null);
            if (!FatTable.isRootIntialized())
            {
                FatTable.setValue(CurrentDirectory.FirstCluster, -1);
                FatTable.writeFatTable();
            }

            ExportPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Exports");
            ImportPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Imports");
            if(!System.IO.Directory.Exists(ExportPath)) System.IO.Directory.CreateDirectory(ExportPath);
            if(!System.IO.Directory.Exists(ImportPath)) System.IO.Directory.CreateDirectory(ImportPath);
        }

        public string GetCurrentDirectory()
        {
            return CurrentDirectory.FileName;
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
            CurrentDirectory.WriteEntryToDisk();
            Console.WriteLine($"Folder '{folderName}' created successfully.");
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
                CurrentDirectory.WriteEntryToDisk();
                directory.DeleteDirectory();
                Console.WriteLine($"Directory '{name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"'{name}' is not a directory.");
            }
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

        public void RenameDirectory(string currentName, string newName)
        {

            //string currentName = args[0];
            //string newName = args[1];

            int index = CurrentDirectory.Search(currentName);

            if (index == -1)
            {
                Console.WriteLine($"Directory '{currentName}' not found.");
                return;
            }

            Directory entry = (Directory)CurrentDirectory.DirectoryTable[index];
            entry.UpdateName(newName);

            Console.WriteLine($"Directory '{currentName}' renamed to '{newName}'.");
        }

        public void CopyDirectory(string sourceName, string destinationName)
        {
            int sourceIndex = CurrentDirectory.Search(sourceName);
            int destinationIndex = CurrentDirectory.Search(destinationName);

            if (sourceIndex == -1)
            {
                Console.WriteLine($"Source directory '{sourceName}' not found.");
                return;
            }

            if (destinationIndex == -1)
            {
                Console.WriteLine($"Destination directory '{destinationName}' not found.");
                return;
            }

            Directory sourceEntry = (Directory)CurrentDirectory.DirectoryTable[sourceIndex];
            Directory destinationEntry = (Directory)CurrentDirectory.DirectoryTable[destinationIndex];

            Directory newEntry = new Directory(sourceEntry.FileName + "_copy", sourceEntry.FileAttribute, 0, sourceEntry.FileSize, destinationEntry);
            destinationEntry.DirectoryTable.Add(newEntry);
            destinationEntry.WriteEntryToDisk();


            sourceEntry.ReadEntryFromDisk();
            newEntry.DirectoryTable = sourceEntry.DirectoryTable;
            newEntry.WriteEntryToDisk();

            Console.WriteLine($"Directory '{sourceName}' copied to '{destinationName}'.");
        }

        public void ChangeDirectory(string directory)
        {
            if (directory == "..")
            {
                if (CurrentDirectory.Parent == null)
                {
                    Console.WriteLine("Already at the root directory.");
                    return;
                }

                CurrentDirectory = CurrentDirectory.Parent;
                Console.WriteLine("Navigated up to the parent directory.");
            }
            else if (directory == "root")
            {
                Directory temp = CurrentDirectory;
                while (temp.Parent != null)
                    temp = temp.Parent;
                CurrentDirectory = temp;
                Console.WriteLine("Navigated to the root directory.");
            }
            else
            {
                int index = CurrentDirectory.Search(directory);

                if (index == -1)
                {
                    Console.WriteLine($"Folder '{directory}' not found.");
                    return;
                }

                if (!(CurrentDirectory.DirectoryTable[index] is Directory folder))
                {
                    Console.WriteLine($"'{directory}' is not a folder.");
                    return;
                }

                CurrentDirectory = folder;
                Console.WriteLine($"Navigated to folder '{directory}'.");
            }
		}

        public void WriteFile(string fileName, string content)
        {
            int index = CurrentDirectory.Search(fileName);
            if (index != -1)
            {
                FileEntry entry = (FileEntry)CurrentDirectory.DirectoryTable[index];
                entry.UpdateFile(content);
                Console.WriteLine($"File '{fileName}' updated successfully.");
            }
            else
            {
				FileEntry newFile = new FileEntry(fileName, 0, 0, 0, CurrentDirectory);
				newFile.UpdateFile(content);
                CurrentDirectory.AddChild(newFile);
				Console.WriteLine($"File '{fileName}' created successfully.");
			}
        }

        public void AppendFile(string fileName, string content)
        {
			int index = CurrentDirectory.Search(fileName);
			if (index != -1)
            {
				FileEntry entry = (FileEntry)CurrentDirectory.DirectoryTable[index];
				entry.AppendFile(content);
				Console.WriteLine($"File '{fileName}' updated successfully.");
			}
			else
            {
				FileEntry newFile = new FileEntry(fileName, 0, 0, 0, CurrentDirectory);
                CurrentDirectory.AddChild(newFile);
				Console.WriteLine($"File '{fileName}' created successfully.");
			}
		}

        public void ReadFile(string fileName)
        {
            int index = CurrentDirectory.Search(fileName);
			if (index != -1)
            {
				FileEntry entry = (FileEntry)CurrentDirectory.DirectoryTable[index];
				entry.ReadFile();
				Console.WriteLine(entry.Content);
			}
			else
            {
				Console.WriteLine($"File '{fileName}' not found.");
			}
        }

        public void DeleteFile(string fileName)
        {
			int index = CurrentDirectory.Search(fileName);
            if (index != -1)
            {
                if (CurrentDirectory.DirectoryTable[index] is FileEntry entry)
                {
                    entry = (FileEntry)CurrentDirectory.DirectoryTable[index];
                    entry.DeleteFile();
                    Console.WriteLine($"File '{fileName}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"'{fileName}' is not a file.");
                }
			}
			else
            {
				Console.WriteLine($"File '{fileName}' not found.");
			}
		}

        public void ExportFile(string fileName)
        {
			int index = CurrentDirectory.Search(fileName);
			if (index != -1)
            {
				FileEntry entry = (FileEntry)CurrentDirectory.DirectoryTable[index];
                string content = entry.Content;
                string filePath = System.IO.Path.Combine(ExportPath, fileName);
                System.IO.File.WriteAllText(filePath, content);
				Console.WriteLine($"File '{fileName}' exported successfully.");
			}
			else
            {
				Console.WriteLine($"File '{fileName}' not found.");
			}
		}

        public void ImportFile(string fileName)
        {
			string filePath = System.IO.Path.Combine(ImportPath, fileName);
			if (!System.IO.File.Exists(filePath))
            {
				Console.WriteLine($"File '{fileName}' not found.");
				return;
			}

			string content = System.IO.File.ReadAllText(filePath);
			WriteFile(fileName, content);
			Console.WriteLine($"File '{fileName}' imported successfully.");
		}


        // Helper Functions
        public static bool ValidateName(string name)
        {
			if (string.IsNullOrWhiteSpace(name))
            {
				Console.WriteLine("Name cannot be empty.");
				return false;
			}

            if(!ValidNameCharacters(name))
            {
				return false;
			}

			if (name.Length > 11)
            {
                Console.WriteLine($"Name cannot be longer than 11 characters.");
				return false;
			}

			return true;
		}

        public static bool ValidNameCharacters(string name)
        {
            string illegalChars = "#%&{}\\<>*?/ $!'\":@+`|=";
            foreach (char c in name)
				if (illegalChars.Contains(c))
                {
					Console.WriteLine($"Illegal character '{c}' in name.");
					return false;
				}
			return true;
		}
    }

}
