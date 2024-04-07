using ShellX.Disk;
using ShellX.UnitTesting;
using Directory = ShellX.Entry.Directory;
using DirectoryEntry = ShellX.Entry.DirectoryEntry;
using FileEntry = ShellX.Entry.FileEntry;
using FatTable = ShellX.Disk.FatTable;
using Shell = ShellX.ShellSystem.Shell;

namespace ShellX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VirtualDisk.Initialize(); // Already in ShellSystem.Run()
            //FatTable.PrintFatTable(0, 21);
            Shell.Run();
        }
    }
}
