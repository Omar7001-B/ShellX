## Class Documentation: `VirtualDisk` [Source Code](./Disk/VirtualDisk.cs)
`VirtualDisk` is a static class representing the virtual disk in the file system. It provides methods for initializing, reading, and writing disk blocks.

### Properties
- **DiskFileName**: The name of the disk file.

### Methods
- **Initialize()**: Initializes the virtual disk if it does not exist, including creating the disk file and initializing the FAT.
- **WriteBlock(byte[] data, int index)**: Writes data to a block at the specified index on the disk.
- **ReadBlock(int index)**: Reads data from a block at the specified index on the disk.

### Notes
- The `VirtualDisk` class provides an abstraction for disk operations, such as reading and writing blocks.
- It interacts with the `FatTable` class to manage disk block allocation and deallocation.

## Class Documentation: `FatTable` [Source Code](./Disk/FatTable.cs)
`FatTable` is a static class representing the File Allocation Table (FAT) in the file system. It manages the allocation and deallocation of disk blocks, as well as reading and writing the FAT to disk.

### Methods
- **Initialize()**: Initializes the FAT with default values and writes it to disk.
- **WriteFatTable()**: Writes the FAT to the disk.
- **ReadFatTable()**: Reads the FAT from the disk.
- **PrintFatTable(int start = 0, int end = 1024)**: Prints the FAT entries from start to end.
- **GetFullFatValue(int firstCluster)**: Retrieves the full chain of FAT values starting from the specified cluster.
- **GetFatValueAsString(int firstCluster)**: Retrieves the FAT values as a string for the specified cluster.
- **GetAvailableBlock()**: Finds the index of the next available block in the FAT.
- **GetValue(int index)**: Retrieves the value at the specified index in the FAT.
- **SetValue(int index, int value)**: Sets the value at the specified index in the FAT and writes it to disk.
- **GetNumberOfFreeBlocks()**: Counts the number of free blocks in the FAT.
- **GetFreeSpace()**: Calculates the total free space in the FAT.

### Notes
- The FAT manages the allocation of disk blocks and tracks the usage of disk space.
- Methods such as `Initialize`, `ReadFatTable`, and `WriteFatTable` interact with the disk to read and write the FAT.
