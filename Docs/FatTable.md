## Class Documentation: `FatTable`

### Namespace
`ShellX.Disk`

### Description
`FatTable` is a static class representing the File Allocation Table (FAT) in the file system. It manages the allocation and deallocation of disk blocks, as well as reading and writing the FAT to disk.

### [Source Code](./Disk/FatTable.cs)

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
