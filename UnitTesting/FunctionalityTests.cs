using Simple_Shell_And_File_System__FAT_.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Directory = Simple_Shell_And_File_System__FAT_.Classes.Directory;

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

        public static void TestingTheDirectoryEntry()
        {
            DirectoryEntry entry = new DirectoryEntry("mo.txt", 0, 2, 1024);

            byte[] entryData = entry.MetaToByteArray();

            Console.WriteLine("Directory Entry in bytes:");
            foreach (byte b in entryData)
            {
                Console.Write($"{b:x2} "); // Display bytes in hexadecimal format
            }
            Console.WriteLine();

            DirectoryEntry newEntry = new DirectoryEntry();

            Console.WriteLine("\nNew Directory Entry Properties:");
            Console.WriteLine($"FileName: {newEntry.FileName}");
            Console.WriteLine($"File Attribute: {newEntry.FileAttribute}");
            Console.WriteLine($"First Cluster: {newEntry.FirstCluster}");
            Console.WriteLine($"File Size: {newEntry.FileSize}");
		}
        public static void TestDirectory()
        {
            // Create a directory entry to serve as the parent directory
            Directory parentEntry = new Directory("ParentDir", 0, 0, 0, null);

			// Create a directory instance
			Directory directory = new Directory("TestDir", 0, 0, 0, parentEntry);

            // Add some directory entries to the directory table
            DirectoryEntry entry1 = new DirectoryEntry("File1.txt", 0, 0, 1024);
            DirectoryEntry entry2 = new DirectoryEntry("File2.txt", 0, 0, 2048);
            DirectoryEntry entry3 = new DirectoryEntry("SubDir", 0, 0, 0);

            directory.DirectoryTable.Add(entry1);
            directory.DirectoryTable.Add(entry2);
            directory.DirectoryTable.Add(entry3);

            // Write the directory to the virtual disk
            directory.WriteEntryToDisk();

            // Read the directory from the virtual disk
            Directory readDirectory = new Directory();
            readDirectory.ReadEntryFromDisk();

            // Search for a directory entry by name
            int index = readDirectory.Search("File1.txt");

            // Output the result of the search
            if (index != -1)
            {
                Console.WriteLine($"Found at index: {index}");
                Console.WriteLine($"FileName: {new string(readDirectory.DirectoryTable[index].FileName)}");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
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
