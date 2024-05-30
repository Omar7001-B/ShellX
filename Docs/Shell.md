## Shell Documentation

### Overview
The `Shell` class provides a command-line interface for interacting with the file system. It allows users to execute various commands for navigating directories, managing files, and accessing debug functionalities.

### Usage
1. Initialize the virtual disk and file system.
2. Run the shell.
3. Enter commands to perform actions such as changing directories, listing directory contents, creating/deleting directories, managing files, and accessing debug functionalities.

### Available Commands
- **help**: Provides help information for commands.
- **cls**: Clears the screen.
- **quit**: Quits the shell.
- **cd**: Changes the current directory.
- **dir**: Lists the contents of a directory.
- **copy**: Copies one or more entries to another location.
- **cut**: Cuts one or more entries to another location.
- **md**: Creates a directory.
- **rd**: Removes a directory.
- **rename**: Renames a file.
- **echo**: Displays text or variables, writes or appends to files.
- **type**: Displays the contents of a text file.
- **del**: Deletes one or more files.
- **import**: Imports text file(s) from your computer.
- **export**: Exports text file(s) to your computer.
- **meta**: Shows meta data of a file.
- **tree**: Lists the tree of a directory.
- **fat**: Shows the FAT file system.
- **mds**: Creates n directories.
- **rds**: Removes n directories.

### Debugging
- Use debug commands (`meta`, `tree`, `fat`, `mds`, `rds`) for debugging and testing purposes.

### Notes
- Ensure proper initialization of the virtual disk and file system before running the shell.
- Commands are case-insensitive.
