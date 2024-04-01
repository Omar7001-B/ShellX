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

        // Make fucntion to check if there is root block at fat[5]

        public static bool isRootIntialized()
        {
			return fat[5] != 0;
		}

        public static void printFatTable(int start = 0, int end = 1024)
        {
            Dictionary<int, List<int>> blocks = new Dictionary<int, List<int>>();
            List<int> visited = new List<int>();

            for (int i = start; i < end; i++)
            {
				if (visited.Contains(i))
					continue;

				List<int> children = new List<int>();
				int current = i;

                if(i == 0)
                {
                    children.Add(fat[i]);
					blocks.Add(i, children);
					continue;
				}

				while (current != -1 && current != 0)
                {
                    children.Add(fat[current]);
					visited.Add(current);
					current = fat[current];
				}

				blocks.Add(i, children);
			}



            foreach (var block in blocks)
            {
				Console.Write($"fat[{block.Key}] -> ");
                // print them and print -> between them but not after the last one
                for (int i = 0; i < block.Value.Count; i++)
                {
                    Console.Write(block.Value[i]);
                    if (i != block.Value.Count - 1)
                        Console.Write(" -> ");
                }
				Console.WriteLine();
			}
        }

        public static int getAvailableBlock()
        {
            return Array.FindIndex(fat, block => block == 0);
        }

        public static int getValue(int index)
        {
            if(index == fat[index])
                Console.WriteLine($"Infinite loop detected at fat[{index}] = {fat[index]}");

            return fat[index];
        }

        public static void setValue(int index, int value)
        {
            if(index == 0 || index == 1 || index == 2 || index == 3 || index == 4)
				Console.WriteLine($"Cannot write to reserved block fat[{index}]={fat[index]}");

            if(index == value)
				Console.WriteLine($"Cannot write to itself, this would lead to inf fat[{index}]");

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
