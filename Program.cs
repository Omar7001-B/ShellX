using Simple_Shell_And_File_System__FAT_.Classes;
using Simple_Shell_And_File_System__FAT_.Disk;
using Simple_Shell_And_File_System__FAT_.UnitTesting;
using Directory = Simple_Shell_And_File_System__FAT_.Entry.Directory;
using DirectoryEntry = Simple_Shell_And_File_System__FAT_.Entry.DirectoryEntry;
using FileEntry = Simple_Shell_And_File_System__FAT_.Entry.FileEntry;
using FatTable = Simple_Shell_And_File_System__FAT_.Disk.FatTable;
using Shell = Simple_Shell_And_File_System__FAT_.ShellSystem.Shell;

namespace Simple_Shell_And_File_System__FAT_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VirtualDisk.Initialize(); // Already in ShellSystem.Run()
            //FatTable.printFatTable(0, 21);
            Shell.Run();
        }
    }
}
