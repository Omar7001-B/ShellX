using Simple_Shell_And_File_System__FAT_.Classes;
using Simple_Shell_And_File_System__FAT_.UnitTesting;
using Directory = Simple_Shell_And_File_System__FAT_.Classes.Directory;

namespace Simple_Shell_And_File_System__FAT_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VirtualDisk.Initialize();
            Shell.Run();
        }
    }
}
