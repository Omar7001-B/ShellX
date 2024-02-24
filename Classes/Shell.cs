using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public class Command
    {
        public string Help { get; set; }
        public Action<string[]> Action { get; set; }
    }

    internal class Shell
    {
        public FileSystem fileSystem;
        public Dictionary<string, Command> commands;

        public Shell()
        {
            Initialize();
        }
        private void Initialize()
        {
            fileSystem = new FileSystem();
            commands = new Dictionary<string, Command>
            {
                {"help", new Command { Help = "Provides Help information for commands.", Action = Help }},
                {"cd", new Command { Help = "Change the current default directory to . If the argument is not present, report the current directory. If the directory does not exist, an appropriate error should be reported.", Action = null }},
                {"cls", new Command { Help = "Clear the screen.", Action = null }},
                {"dir", new Command { Help = "List the contents of directory .", Action = null }},
                {"quit", new Command { Help = "Quit the shell.", Action = null }},
                {"copy", new Command { Help = "Copies one or more files to another location", Action = null }},
                {"del", new Command { Help = "Deletes one or more files.", Action = null }},
                {"md", new Command { Help = "Creates a directory.", Action = null }},
                {"rd", new Command { Help = "Removes a directory.", Action = null }},
                {"rename", new Command { Help = "Renames a file.", Action = null }},
                {"type", new Command { Help = "Displays the contents of a text file.", Action = null }},
                {"import", new Command { Help = "Import text file(s) from your computer", Action = null }},
                {"export", new Command { Help = "Export text file(s) to your computer", Action = null }},
            };
        }
    

       public void Run()
        {
            while (true)
            {
                Console.Write($"{fileSystem.GetCurrenDirectory()}> ");
                string input = Console.ReadLine();
                var (command, arguments) = ParseInput(input);

                ExecuteCommand(command, arguments);
            }
        }

        private (string Command, string[] Arguments) ParseInput(string input)
        {
            string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string command = (tokens.Length > 0) ? tokens[0].ToLower() : string.Empty;
            string[] arguments = (tokens.Length > 1) ? tokens.Skip(1).ToArray() : Array.Empty<string>();

            return (command, arguments);
        }



       private void ExecuteCommand(string command, string[] arguments)
       {
          if (commands.TryGetValue(command, out var commandInfo))
          {
              commandInfo.Action(arguments);
          }
          else
          {
              Console.WriteLine($"Command '{command}' not recognized.");
          }
       }


        private void Help(string[] args)
        {
            if (args.Length == 1)
            {
                string command = args[0].ToLower();
                if (commands.TryGetValue(command, out var commandInfo))
                {
                    Console.WriteLine($"{command} - {commandInfo.Help}");
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
                    Console.WriteLine($"{entry.Key} - {entry.Value.Help}");
                }
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
 */
