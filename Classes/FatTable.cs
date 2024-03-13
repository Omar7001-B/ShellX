using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
	public static class FatTable
	{
	    private const int NumberOfBlocks = 1024;
        private static int[] fat = new int[NumberOfBlocks];

        public static void Initialize()
        {
            fat[0] = -1; // Super block

            fat[1] = 2; // Fat Table
            fat[2] = 3; 
            fat[3] = 4;
            fat[4] = -1;

            Array.Fill(fat, 0, 5, 1019); // Data Blocks
        }

        public static void writeFatTable()
        {
            byte[] fatBytes = new byte[NumberOfBlocks * 4];
			Buffer.BlockCopy(fat, 0, fatBytes, 0, fatBytes.Length);
			using (FileStream fs = new FileStream(VirtualDisk.DiskFileName, FileMode.Open))
            {
                fs.Seek(1024, SeekOrigin.Begin);
                fs.Write(fatBytes, 0, fatBytes.Length);
            }
        }

        public static void readFatTable()
        {
            byte[] fatBytes = new byte[NumberOfBlocks * 4];

            using (FileStream fs = new FileStream(VirtualDisk.DiskFileName, FileMode.Open))
            {
                fs.Seek(1024, SeekOrigin.Begin);
                fs.Read(fatBytes, 0, fatBytes.Length);
            }

            Buffer.BlockCopy(fatBytes, 0, fat, 0, fatBytes.Length);
        }

        public static void printFatTable()
        {
            Console.WriteLine("FAT Table:");
            for (int i = 0; i < NumberOfBlocks; i++)
                Console.WriteLine($"FAT[{i}] = {fat[i]}");
        }

        public static int getAvailableBlock()
        {
            return Array.FindIndex(fat, block => block == 0);
        }

        public static int getValue(int index)
        {
            return fat[index];
        }

        public static void setValue(int value, int index)
        {
            fat[index] = value;
        }

        public static int getNumberOfFreeBlocks()
        {
            return fat.Count(block => block == 0);
        }

        public static int getFreeSpace()
        {
            return getNumberOfFreeBlocks() * 1024;
        }
	}
}
