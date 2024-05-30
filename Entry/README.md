## Class Documentation: `DirectoryEntry` [Source Code](./DirectoryEntry.cs)
`DirectoryEntry` represents an entry in a directory within the file system. This class handles the metadata associated with a file or directory, including its name, attributes, size, and location on the disk.

### Properties
- **FileName**: The name of the file or directory (11 bytes).
- **FileAttribute**: The attribute of the file or directory (1 byte).
- **FileEmpty**: A placeholder of 12 empty bytes.
- **FirstCluster**: The starting cluster number on the disk (4 bytes).
- **FileSize**: The size of the file in bytes (4 bytes).
- **Parent**: The parent directory containing this entry.
- **ContentBytes**: The content of the file represented in bytes.

### Constructors
- **DirectoryEntry()**: Default constructor.
- **DirectoryEntry(string name, byte attribute, int cluster, int size, Directory parent)**: Initializes a new `DirectoryEntry` with the specified name, attribute, cluster, size, and parent directory.
- **DirectoryEntry(DirectoryEntry entry, Directory parent)**: Initializes a new `DirectoryEntry` by copying an existing entry and assigning a new parent directory.
- **DirectoryEntry(byte[] data)**: Initializes a new `DirectoryEntry` from a byte array containing metadata.

### Methods
- **MetaToByteArray()**: Converts the metadata of the entry to a byte array.
- **MetaFromByteArray(byte[] data)**: Creates a `DirectoryEntry` from a byte array of metadata.
- **ReadEntryFromDisk()**: Reads the entry content from the disk.
- **ConvertBytesToContent()**: Virtual method to convert bytes to content (to be overridden by derived classes).
- **WriteEntryToDisk()**: Writes the entry content to the disk.
- **ConvertContentToBytes()**: Virtual method to convert content to bytes (to be overridden by derived classes).
- **CopyEntry(Directory newParent)**: Creates a copy of the entry with a new parent directory.
- **MoveEntry(Directory newParent)**: Moves the entry to a new parent directory.
- **DeleteEntryFromDisk()**: Deletes the entry from the disk.
- **ClearFat()**: Clears the FAT (File Allocation Table) entries associated with this entry.
- **AllocateFirstCluster()**: Allocates the first cluster for the entry if needed.
- **SetFirstCluster(int cluster)**: Sets the first cluster for the entry and updates the parent directory.
- **SetName(string name)**: Sets the name of the entry and updates the parent directory.
- **FormateFileName(string name, int attribute = 1)**: Formats the file name to fit the 11-byte constraint.

### Notes
- **FileName** is padded or truncated to fit 11 bytes.
- **FileAttribute** determines if the entry is a file or a directory.
- **Parent** references the parent directory but care must be taken to avoid circular dependencies.
- Methods such as `ReadEntryFromDisk` and `WriteEntryToDisk` interact with the `VirtualDisk` and `FatTable` classes to read/write data.


## Class Documentation: `Directory` [Source Code](./Directory.cs)
`Directory` extends `DirectoryEntry` and represents a directory within the file system. It contains a list of directory entries representing its contents.

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

## Class Documentation: `FileEntry` [Source Code](./FileEntry.cs)
`FileEntry` extends `DirectoryEntry` and represents a file within the file system. It contains additional functionality to handle file content.

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
