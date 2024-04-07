using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShellX.Disk;
using ShellX.Entry;
using Directory = ShellX.Entry.Directory;

namespace ShellX.ShellSystem
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
            CurrentDirectory = new Directory("root", null);
            CurrentDirectory.FirstCluster = 5;
            CurrentDirectory.ReadEntryFromDisk();
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
            CurrentDirectory.AddChild(new Directory(folderName, CurrentDirectory));
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
                directory.DeleteEntryFromDisk();
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

		public void ListDirectoryTree(bool showFat = false)
        {
			ListDirectoryTree(CurrentDirectory, "");
		}
        public void ListDirectoryTree(Directory directory, string indent)
        {
            directory.ReadEntryFromDisk();
            Console.WriteLine($"{indent}{directory.FileName} {FatTable.GetFatValueAsString(directory.FirstCluster)}");
            for(int i = 0; i < directory.DirectoryTable.Count; i++)
            {
                string branch = (i == directory.DirectoryTable.Count - 1) ? "└──" : "├──";
                string spaces = indent.Replace("├", "│").Replace("└", " ").Replace("─", " ");

                DirectoryEntry entry = directory.DirectoryTable[i];
                if (entry is Directory subDirectory)
					ListDirectoryTree(subDirectory, $"{spaces}{branch} ");
				else
                {
					Console.WriteLine($"{spaces}{branch} {entry.FileName} {FatTable.GetFatValueAsString(entry.FirstCluster)}");
                }
			}
        }



        public void RenameDirectory(string currentName, string newName)
        {

            int index = CurrentDirectory.Search(currentName);
            if (index == -1)
            {
                Console.WriteLine($"Directory '{currentName}' not found.");
                return;
            }

            if(CurrentDirectory.Search(newName) != -1)
            {
                Console.WriteLine($"Directory '{newName}' already exists.");
                return;
            }


            Directory entry = (Directory)CurrentDirectory.DirectoryTable[index];
            entry.UpdateName(newName);

            Console.WriteLine($"Directory '{currentName}' renamed to '{newName}'.");
        }

        public void CopyEntry(string sourcePath, string destPath)
        {
            DirectoryEntry sourceEntry = GetByPath(sourcePath);
			if (sourceEntry == null) return;

			DirectoryEntry destEntry = GetByPath(destPath);
            if(destEntry is Directory directory)
            {
                sourceEntry.CopyEntry(directory);
				Console.WriteLine($"Directory '{sourceEntry.FileName}' copied to '{destEntry.FileName}'.");
            }
			else
            {
				Console.WriteLine($"'{destPath}' is not a directory.");
			}
        }

        public bool isChildOf(Directory parent, DirectoryEntry child)
        {
			if (child == null) return false;
			if (child == parent) return true;
			return isChildOf(parent, child.Parent);
		}

        public void CutEntry(string sourcePath, string destPath)
        {
            DirectoryEntry sourceEntry = GetByPath(sourcePath);
            if (sourceEntry == null) return;

            DirectoryEntry destEntry = GetByPath(destPath);

            if (isChildOf((Directory)sourceEntry, destEntry))
            {
				Console.WriteLine("Cannot move a directory into its child.");
            }

            if(sourcePath == destPath)
            {
                Console.WriteLine("Cannot move a directory into itself.");
				return;
            }

            if (destEntry is Directory directory)
            {
                var temp = sourceEntry.CopyEntry(directory);
                sourceEntry.DeleteEntryFromDisk();
				directory.AddChild(temp, false);
				Console.WriteLine($"Directory '{sourceEntry.FileName}' moved to '{destEntry.FileName}'.");
			}
			else
            {
				Console.WriteLine($"'{destPath}' is not a directory.");
			}

        }



        DirectoryEntry GetByPath(string path)
        {
			Directory current = CurrentDirectory;
            List<string> folders = path.Split('/').ToList();

			foreach (string folder in folders)
            {
                if (folder == ".") continue;
				else if (folder == "..")
                {
					if (current.Parent == null)
                    {
						Console.WriteLine("Access is denied.");
						return null;
					}
					current = current.Parent;
				}
				else if (folder == "root")
                {
					Directory temp = current;
					while (temp.Parent != null)
						temp = temp.Parent;
					current = temp;
				}
				else
                {
					int index = current.Search(folder);
					if (index == -1)
                    {
						Console.WriteLine($"Folder '{folder}' not found.");
						return null;
					}

					if (!(current.DirectoryTable[index] is Directory folderEntry))
                    {
                        if(folders.IndexOf(folder) == folders.Count - 1)
							return current.DirectoryTable[index];
						Console.WriteLine($"'{folder}' is not a folder.");
						return null;
					}

					current = folderEntry;
				}
                current.ReadEntryFromDisk();
			}

			return current;
		}

        public void ChangeDirectory(string directory)
        {
            DirectoryEntry entry = GetByPath(directory);
            if(entry is Directory folder)
            {
				CurrentDirectory = folder;
				Console.WriteLine($"Directory changed to '{folder.FileName}'.");
			}
            else
            {
                Console.WriteLine($"'{directory}' is not a directory.");
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
				FileEntry newFile = new FileEntry(fileName, CurrentDirectory);
                CurrentDirectory.AddChild(newFile);
				newFile.UpdateFile(content);
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
				FileEntry newFile = new FileEntry(fileName, CurrentDirectory);
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
				entry.ReadEntryFromDisk();
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
                    entry.DeleteEntryFromDisk();
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
