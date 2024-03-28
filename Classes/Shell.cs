using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public class Command
    {
        public string Description  { get; set; }
        public Action<string[]> Action { get; set; }
    }

    public class Shell
    {
        public static FileSystem fileSystem = new FileSystem();
        public static Dictionary<string, Command> commands = new Dictionary<string, Command>
        {
                {"help", new Command { Description  = "Provides Help information for commands.", Action = Help }},
                {"cls", new Command { Description  = "Clear the screen.", Action = Cls }},
                {"quit", new Command { Description  = "Quit the shell.", Action = Quit }},

                {"cd", new Command { Description  = "Change the current default directory to . If the argument is not present, report the current directory. If the directory does not exist, an appropriate error should be reported.", Action = null }},
                {"dir", new Command { Description  = "List the contents of directory .", Action = ListDirectoryContents }},
                {"copy", new Command { Description  = "Copies one or more files to another location", Action = null }},
                {"del", new Command { Description  = "Deletes one or more files.", Action = null }},
                {"md", new Command { Description  = "Creates a directory.", Action = MakeDirectory }},
                {"rd", new Command { Description  = "Removes a directory.", Action = RemoveDirectory }},
                {"rename", new Command { Description  = "Renames a file.", Action = null }},
                {"type", new Command { Description  = "Displays the contents of a text file.", Action = null }},
                {"import", new Command { Description  = "Import text file(s) from your computer", Action = null }},
                {"export", new Command { Description  = "Export text file(s) to your computer", Action = null }},
            };


        public Shell() { }

        public static void Run()
        {
            while (true)
            {
                Console.Write($"{fileSystem.GetCurrenDirectory()}> ");
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
                {
                    Console.WriteLine($"{command,-10} - {commandInfo.Description}");
                }
                else
                {
                    Console.WriteLine($"Help not available for '{command}'.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");
                foreach (var entry in commands)
                {
                    Console.WriteLine($"{entry.Key,-10} - {entry.Value.Description}\n");
                }
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


        public static void MakeDirectory(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: md <name>");
                return;
            }

            string name = args[0];

            if (fileSystem.CurrentDirectory.Search(name) != -1)
            {
                Console.WriteLine($"Directory '{name}' already exists.");
                return;
            }

            Directory newDir = new Directory(name, 1, 0, 0, fileSystem.CurrentDirectory);

            fileSystem.CurrentDirectory.DirectoryTable.Add(newDir);

            fileSystem.CurrentDirectory.WriteDirectory();

            Console.WriteLine($"Directory '{name}' created successfully.");
        }
        public static void ListDirectoryContents(string[] args)
        {
            if (fileSystem == null)
            {
                Console.WriteLine("File system not initialized.");
                return;
            }

            Console.WriteLine("Directory Contents:");
            foreach (var entry in fileSystem.CurrentDirectory.DirectoryTable)
            {
                Console.WriteLine(entry.Filename);
            }
        }
        public static void RemoveDirectory(string[] args)
        {
            if (fileSystem == null)
            {
                Console.WriteLine("File system not initialized.");
                return;
            }

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: rd <name>");
                return;
            }

            string name = args[0];

            int index = fileSystem.CurrentDirectory.Search(name);
            if (index == -1)
            {
                Console.WriteLine($"Directory '{name}' not found.");
                return;
            }

            DirectoryEntry entry = fileSystem.CurrentDirectory.DirectoryTable[index];
            if (entry is Directory directory)
            {
                fileSystem.CurrentDirectory.DirectoryTable.RemoveAt(index);
                fileSystem.CurrentDirectory.WriteDirectory();
                directory.DeleteDirectory();
                Console.WriteLine($"Directory '{name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"'{name}' is not a directory.");
            }
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
