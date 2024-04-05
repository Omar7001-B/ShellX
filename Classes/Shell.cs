using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Shell_And_File_System__FAT_.Classes;
using Simple_Shell_And_File_System__FAT_.UnitTesting;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public class Command
    {
        public string Description  { get; set; }
        public Action<string[]> Action { get; set; }
    }

    public class Shell
    {
        public static FileSystem fileSystem;
        public static Dictionary<string, Command> commands = new Dictionary<string, Command>
        {
                // General
                {"help", new Command { Description  = "Provides Help information for commands.", Action = Help }},
                {"cls", new Command { Description  = "Clear the screen.", Action = Cls }},
                {"quit", new Command { Description  = "Quit the shell.", Action = Quit }},

                // Direcotry
                {"cd", new Command { Description  = "Changes the current directory.", Action = Cd }},
                {"dir", new Command { Description  = "List the contents of directory.", Action = Dir }},
                {"copy", new Command { Description  = "Copies one or more files to another location.", Action = Copy }},
                {"md", new Command { Description  = "Creates a directory.", Action = Md }},
                {"rd", new Command { Description  = "Removes a directory.", Action = Rd }},
                {"rename", new Command { Description  = "Renames a file.", Action = Rename }},

                // File
                {"echo", new Command { Description  = "Displays text or variables and can be used to write or append to files", Action = Echo }},
                {"type", new Command { Description  = "Displays the contents of a text file.", Action = Type }},
                {"del", new Command { Description  = "Deletes one or more files.", Action = Del }},
                {"import", new Command { Description  = "Import text file(s) from your computer.", Action = null }},
                {"export", new Command { Description  = "Export text file(s) to your computer.", Action = null }},

                // Debug
                {"showfat", new Command { Description  = "Shows The Fat File System.", Action = ShowFat }},
                {"mds", new Command { Description  = "Creates n directories.", Action = Mds }},
                {"rds", new Command { Description  = "Removes n directories.", Action = Rds }},

            };


        public Shell() { }

        public static void Run()
        {
            VirtualDisk.Initialize();
            fileSystem = new FileSystem();
            while (true)
            {
                Console.Write($"{fileSystem.ShowCurrentPath()}> ");
                string input = Console.ReadLine();
                var (command, arguments) = ParseInput(input);

                ExecuteCommand(command, arguments);
            }
        }

        private static (string Command, string[] Arguments) ParseInput(string input)
        {
            string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string command = (tokens.Length > 0) ? tokens[0].ToLower() : string.Empty;
            string[] arguments = (tokens.Length > 1) ? tokens.Skip(1).ToArray() : Array.Empty<string>();

            return (command, arguments);
        }

        private static void ExecuteCommand(string command, string[] arguments)
        {

            if (fileSystem == null)
            {
                Console.WriteLine("File system not initialized.");
                return;
            }

            if (commands.TryGetValue(command, out var commandInfo))
            {
                if (commandInfo.Action != null) commandInfo.Action(arguments);
                else Console.WriteLine($"Command '{command}' is not implemented yet.");
            }
            else if(!string.IsNullOrEmpty(command))
            {
                Console.WriteLine($"Command '{command}' not recognized.");
            }
        }
        public static void Help(string[] args)
        {
            if (args.Length == 1)
            {
                string command = args[0].ToLower();
                if (commands.TryGetValue(command, out var commandInfo))
                    Console.WriteLine($"{command,-10} - {commandInfo.Description}");
                else
                    Console.WriteLine($"Help not available for '{command}'.");
            }
            else
            {
                Console.WriteLine("Available commands:");
                foreach (var entry in commands)
                    Console.WriteLine($"{entry.Key,-10} - {entry.Value.Description}\n");
            }
        }


        public static void Cls(string[] args)
        {
            Console.Clear();
        }

        public static void Quit(string[] args)
        {
            Environment.Exit(0);
        }

        public static void Cd(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: cd <directory>");
                return;
            }

			fileSystem.ChangeDirectory(args[0]);
        }



        public static void Md(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: md <name>");
                return;
            }

            if (FileSystem.ValidateName(args[0]))
				fileSystem.AddFolder(args[0]);
        }

        public static void Dir(string[] args)
        {
            fileSystem.ListDirectoryContents();
        }
        public static void Rd(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: rd <name>");
                return;
            }

            if (FileSystem.ValidateName(args[0]))
				fileSystem.DeleteFolder(args[0]);
        }

        public static void Rename(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: rename <current_name> <new_name>");
                return;
            }

            if (FileSystem.ValidateName(args[0]))
                fileSystem.RenameDirectory(args[0], args[1]);
		}

        public static void Copy(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: copy <source_directory> <destination_directory>");
                return;
            }

            fileSystem.CopyDirectory(args[0], args[1]);
        }

        // Debug Part
        public static void ShowFat(string[] args)
        {
            if(args.Length == 0)
				FatTable.printFatTable(0, 10);

            else if(args.Length == 1)
                FatTable.printFatTable(0, int.Parse(args[0]));
		}

        public static void Mds(string[] args)
        {
            if(args.Length != 1)
            {
				Console.WriteLine("Usage: mds <number>");
				return;
			}

            FunctionalityTests.CreateDirecotries(int.Parse(args[0]));
		}

        public static void Rds(string[] args)
        {
			if(args.Length != 1)
            { 
                Console.WriteLine("Usage: rds <number>");
            }
            FunctionalityTests.DeleteDirecotries(int.Parse(args[0]));
        }


        public static void Echo(string[] args)
        {
			if (args.Length == 0)
            {
				Console.WriteLine("Usage: echo <text>");
				return;
			}

            if (args.Contains(">") || args.Contains(">>"))
            {
                string delimiter = args.Contains(">") ? ">" : ">>";

                int index = Array.IndexOf(args, delimiter);
                string[] text = args.Take(index).ToArray();
                string fileName = args[index + 1];


                if (delimiter == ">")
                    fileSystem.WriteFile(fileName, string.Join(" ", text));
                else
                    fileSystem.AppendFile(fileName, string.Join(" ", text));
            }
            else
            {
                Console.WriteLine(string.Join(" ", args));
            }


		}

        public static void Type(string[] args)
        {
			if (args.Length != 1)
            {
				Console.WriteLine("Usage: type <file>");
				return;
			}

            fileSystem.ReadFile(args[0]);
		}

        public static void Del(string[] args)
        {
			if (args.Length != 1)
            {
				Console.WriteLine("Usage: del <file>");
				return;
			}

            fileSystem.DeleteFile(args[0]);
		}

    }

}


/*
    The shell must support the following internal commands:
    cd - Change the current default directory to . If the argument is not present, report the current directory. If the directory does not exist an appropriate error should be reported.
    cls - Clear the screen.
    dir - List the contents of directory .
    quit - Quit the shell.
    copy - Copies one or more files to another location
    del - Deletes one or more files.
    help -Provides Help information for commands.
    md - Creates a directory.
    rd - Removes a directory.
    rename -  Renames a file.
    type - Displays the contents of a text file.
    import – import text file(s) from your computer
    export – export text file(s) to your computer


Notes: 
- toLower (Sensitve Case)
- Isolate the exceution 
- Formating
- Ask about Options
- Make help take multiple argueements
- Validate the arguemnts inside the functions
- -10
 */
