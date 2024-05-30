## FileSystem Documentation

### Overview
The `FileSystem` class represents the file system and provides functionalities for managing directories and files. It interacts with the virtual disk and FAT table to perform disk operations.

### Usage
1. Initialize the file system.
2. Perform directory and file operations using provided methods.
3. Access metadata and manage files through command-line interface using the `Shell` class.

### Directory Operations
- **AddFolder**: Creates a directory.
- **DeleteFolder**: Removes a directory.
- **ListDirectoryContents**: Lists the contents of a directory.
- **ListDirectoryTree**: Lists the tree structure of a directory.
- **ChangeDirectory**: Changes the current directory.
- **RenameDirectory**: Renames a directory.

### File Operations
- **WriteFile**: Writes content to a file.
- **AppendFile**: Appends content to a file.
- **ReadFile**: Reads the contents of a file.
- **DeleteFile**: Deletes a file.
- **ExportFile**: Exports a file to the local file system.
- **ImportFile**: Imports a file from the local file system.

### Metadata and Validation
- **ShowMetaData**: Displays metadata of a directory or file.
- **ValidateName**: Validates the name of a directory or file.

### Helper Functions
- Various helper functions for directory and file operations.

### Notes
- Ensure proper initialization of the virtual disk and FAT table before using file system operations.
- Use provided validation functions to ensure correct input and prevent errors.
