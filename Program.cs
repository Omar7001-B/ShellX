using Simple_Shell_And_File_System__FAT_.Classes;
using Simple_Shell_And_File_System__FAT_.UnitTesting;
using Directory = Simple_Shell_And_File_System__FAT_.Classes.Directory;

namespace Simple_Shell_And_File_System__FAT_
{
    internal class Program
    {
        static void Main(string[] args)
        {
           // FunctionalityTests.TestDirectory();
           VirtualDisk.Initialize();
           //FunctionalityTests.TestingDirectoryAgain();

            Directory parentEntry = new Directory("ParentDir", 0, 5, 0, null);
            parentEntry.PrintDirectoryContents();
            parentEntry.DirectoryTable.Add(new Directory("TestDir", 0, 0, 0, parentEntry));

            foreach (var entry in parentEntry.DirectoryTable)
            {
				Console.WriteLine(entry.Filename);
                Console.WriteLine((entry is Directory ? "Directory" : "Not Directory"));
			}

            FatTable.printFatTable(0, 20);
           
            Shell.Run();
        }
    }
}
