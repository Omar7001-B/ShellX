using Simple_Shell_And_File_System__FAT_.Classes;

namespace Simple_Shell_And_File_System__FAT_
{
    internal class Program
    {

       public static void TestingTheVirtualDisk()
        {
            // Test VirtualDisk Initialization
            Console.WriteLine("Initializing Virtual Disk...");
            VirtualDisk.Initialize();
            Console.WriteLine("Virtual Disk initialized.");

            // Test FAT Table
            Console.WriteLine("\nPrinting FAT Table:");
            FatTable.printFatTable();

            // Test Reading/Writing Blocks
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
        static void Main(string[] args)
        {
            TestingTheVirtualDisk();
            return;
            Shell shell = new Shell();
            shell.Run();
        }
    }
}
