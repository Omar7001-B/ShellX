## Class Documentation: `Directory`

### Namespace
`ShellX.Entry`

### Description
`Directory` extends `DirectoryEntry` and represents a directory within the file system. It contains a list of directory entries representing its contents.

### [Source Code](./Entry/Directory.cs)

### Properties
- **DirectoryTable**: A list of directory entries representing the contents of the directory.

### Constructors
- **Directory(string name, Directory parent)**: Initializes a new directory with the specified name and parent directory.
- **Directory(DirectoryEntry entry, Directory parent)**: Initializes a new directory by copying an existing directory entry and assigning a parent directory.

### Methods
- **ConvertContentToBytes()**: Converts the directory's content to bytes.
- **ConvertBytesToContent()**: Converts bytes to directory content.
- **GetActualType(DirectoryEntry entry)**: Determines the actual type of the directory entry (file or directory).
- **CopyEntry(Directory newParent)**: Creates a copy of the directory with a new parent directory.
- **DeleteEntryFromDisk()**: Deletes the directory and its contents from the disk.
- **Search(string name)**: Searches for an entry by name in the directory.
- **Search(DirectoryEntry entry)**: Searches for an entry in the directory.
- **HasChild(string name)**: Checks if the directory has a child entry with the specified name.
- **HasChild(DirectoryEntry entry)**: Checks if the directory has a child entry.

### Notes
- The directory maintains a list of directory entries representing its contents in the `DirectoryTable` property.
- Methods such as `CopyEntry` and `DeleteEntryFromDisk` interact with the file system to perform operations on the directory and its contents.
