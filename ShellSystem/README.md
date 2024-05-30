# Shell and FileSystem Documentation

## Shell System [Source Code](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs)
The `Shell` system provides a command-line interface (CLI) for interacting with the file system. It encapsulates various functionalities for managing directories, files, and executing commands.

##### Members:
- **[fileSystem](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L23)**: A static member representing the file system instance.
- **[commands](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L24)**: A dictionary containing command names and their corresponding actions.

##### Methods:
- **[Run()](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L73)**: Initializes the virtual disk and file system, then starts the shell loop to accept user input and execute commands.
- **[ParseInput(string input)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L73)**: Parses the user input to extract the command and arguments.
- **[ExecuteCommand(string command, string[] arguments)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L102)**: Executes the specified command with its arguments.
- **[Help(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L102)**: Displays help information for commands.
- **[HandleCmd(string[] args, string usage, Action<string[]> action)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L131)**: Handles commands with one argument.
- **[HandleCmd(string[] args, string usage, Action<string[], string[]> action)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L98)**: Handles commands with two arguments.
- **[Cls(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L145)**: Clears the console screen.
- **[Quit(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L118)**: Exits the shell.
- **[Cd(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L155)**: Changes the current directory.
- **[Md(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L168)**: Creates a new directory.
- **[Dir(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L180)**: Lists the contents of the current directory.
- **[Tree(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L166)**: Lists the directory tree.
- **[Rd(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L189)**: Removes a directory.
- **[Rename(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L200)**: Renames a directory.
- **[Copy(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L211)**: Copies one or more entries to another location.
- **[Cut(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L222)**: Cuts one or more entries to another location.
- **[ShowFat(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L234)**: Displays the FAT file system.
- **[ShowMeta(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L243)**: Shows metadata of a file.
- **[Mds(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L254)**: Creates multiple directories.
- **[Rds(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L265)**: Removes multiple directories.
- **[Echo(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L275)**: Displays text or variables, writes or appends to files.
- **[Type(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L305)**: Displays the contents of a text file.
- **[Del(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L316)**: Deletes one or more files.
- **[Import(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L327)**: Imports text file(s) from the local file system.
- **[Export(string[] args)](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/Shell.cs#L338)**: Exports text file(s) to the local file system.

## FileSystem [Source Code](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs)
The `FileSystem` class represents the file system and provides functionalities for managing directories and files. It interacts with the virtual disk and FAT table to perform disk operations.

### Directory Operations
- **[AddFolder](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L54)**: Creates a directory.
- **[DeleteFolder](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L61)**: Removes a directory.
- **[ListDirectoryContents](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L90)**: Lists the contents of a directory.
- **[ListDirectoryTree](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L120)**: Lists the tree structure of a directory.
- **[ChangeDirectory](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L264)**: Changes the current directory.
- **[RenameDirectory](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L141)**: Renames a directory.

### File Operations
- **[WriteFile](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L278)**: Writes content to a file.
- **[AppendFile](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L296)**: Appends content to a file.
- **[ReadFile](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L314)**: Reads the contents of a file.
- **[DeleteFile](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L329)**: Deletes a file.
- **[ExportFile](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L351)**: Exports a file to the local file system.
- **[ImportFile](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L368)**: Imports a file from the local file system.

### Metadata and Validation
- **[ShowMetaData](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L382)**: Displays metadata of a directory or file.
- **[ValidateName](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L406)**: Validates the name of a directory or file.

### Helper Functions
- **[IsSubdirectory](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L174)**: Checks if a directory entry is a subdirectory of another directory.
- **[GetByPath](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/FileSystem.cs#L215)**: Retrieves a directory entry by its path.

### Notes
- Ensure proper initialization of the virtual disk and FAT table before using file system operations.
- Use provided validation functions to ensure correct input and prevent errors.
