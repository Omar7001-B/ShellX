using Simple_Shell_And_File_System__FAT_.Classes;

namespace Simple_Shell_And_File_System__FAT_
{
    internal class Program
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

            byte[] entryData = entry.ToByteArray();

            Console.WriteLine("Directory Entry in bytes:");
            foreach (byte b in entryData)
            {
                Console.Write($"{b:x2} "); // Display bytes in hexadecimal format
            }
            Console.WriteLine();

            DirectoryEntry newEntry = new DirectoryEntry().FromByteArray(entryData);

            Console.WriteLine("\nNew Directory Entry Properties:");
            Console.WriteLine($"Filename: {newEntry.Filename}");
            Console.WriteLine($"File Attribute: {newEntry.FileAttribute}");
            Console.WriteLine($"First Cluster: {newEntry.FirstCluster}");
            Console.WriteLine($"File Size: {newEntry.FileSize}");
		}
        static void Main(string[] args)
        {
            string s1 = "mo.txt \0\0\0\0";
            string s2 = "mo.txt";
            Console.WriteLine(s1.Length);
            Console.WriteLine(s2.Length);
            TestingTheDirectoryEntry();

        }
    }
}
