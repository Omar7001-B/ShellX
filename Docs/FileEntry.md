## Class Documentation: `FileEntry`

### Namespace
`ShellX.Entry`

### Description
`FileEntry` extends `DirectoryEntry` and represents a file within the file system. It contains additional functionality to handle file content.

### [Source Code](./Entry/FileEntry.cs)

### Properties
- **Content**: The content of the file.

### Constructors
- **FileEntry(string name, Directory parent)**: Initializes a new file with the specified name and parent directory.
- **FileEntry(DirectoryEntry entry, Directory parent)**: Initializes a new file by copying an existing directory entry and assigning a parent directory.

### Methods
- **ConvertBytesToContent()**: Converts bytes to file content.
- **ConvertContentToBytes()**: Converts the file's content to bytes.
- **CopyEntry(Directory newParent)**: Creates a copy of the file with a new parent directory.
- **AppendFile(string text)**: Appends text to the file's content.
- **UpdateFile(string text)**: Updates the file's content with the specified text.

### Notes
- `FileEntry` represents a file in the file system and contains functionality to manipulate file content.
- Methods such as `AppendFile` and `UpdateFile` interact with the file's content.
