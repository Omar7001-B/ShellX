## Class Documentation: `VirtualDisk`

### Namespace
`ShellX.Disk`

### Description
`VirtualDisk` is a static class representing the virtual disk in the file system. It provides methods for initializing, reading, and writing disk blocks.

### [Source Code](./Disk/VirtualDisk.cs)

### Properties
- **DiskFileName**: The name of the disk file.

### Methods
- **Initialize()**: Initializes the virtual disk if it does not exist, including creating the disk file and initializing the FAT.
- **WriteBlock(byte[] data, int index)**: Writes data to a block at the specified index on the disk.
- **ReadBlock(int index)**: Reads data from a block at the specified index on the disk.

### Notes
- The `VirtualDisk` class provides an abstraction for disk operations, such as reading and writing blocks.
- It interacts with the `FatTable` class to manage disk block allocation and deallocation.
