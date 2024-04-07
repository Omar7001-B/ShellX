using Simple_Shell_And_File_System__FAT_.Disk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Directory = Simple_Shell_And_File_System__FAT_.Entry.Directory;
using DirectoryEntry = Simple_Shell_And_File_System__FAT_.Entry.DirectoryEntry;
using FileEntry = Simple_Shell_And_File_System__FAT_.Entry.FileEntry;
using FatTable = Simple_Shell_And_File_System__FAT_.Disk.FatTable;
using VirtualDisk = Simple_Shell_And_File_System__FAT_.Disk.VirtualDisk;
using Shell = Simple_Shell_And_File_System__FAT_.ShellSystem.Shell;


namespace Simple_Shell_And_File_System__FAT_.UnitTesting
{
    public class FunctionalityTests
	{
        public static void TestingTheVirtualDisk()
        {
            VirtualDisk.Initialize();
            return;
            Console.WriteLine("Initializing Virtual Disk...");
            VirtualDisk.Initialize();
            Console.WriteLine("Virtual Disk initialized.");

            Console.WriteLine("\nPrinting FAT Table:");
            FatTable.printFatTable();

            Console.WriteLine("\nTesting Reading/Writing Blocks:");
            byte[] testData = new byte[1024];
            Array.Fill(testData, (byte)'X');

            Console.WriteLine("Writing test data to block 5...");
            VirtualDisk.writeBlock(testData, 7);

            Console.WriteLine("Reading data from block 5:");
            byte[] readData = VirtualDisk.readBlock(7);
            Console.WriteLine(System.Text.Encoding.ASCII.GetString(readData));

            Console.WriteLine("\nTesting complete.");
        }

        public static void CreateDirecotries(int n)
        {
            for(int i = 0; i < n; i++)
            {
                string[] folder = new string[]{ $"Folder{i}" };
				Shell.Md(folder);
			}
        }

        public static void DeleteDirecotries(int n)
        {
            for(int i = 0;i < n; i++)
            {
                string[] folder = new string[]{ $"Folder{i}" };
                Shell.Rd(folder);
            }
        }
	}
}
