namespace ShellX.Disk
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
			WriteFatTable();
        }

        public static void WriteFatTable()
        {
            byte[] fatBytes = new byte[NumberOfBlocks * 4];
			Buffer.BlockCopy(fat, 0, fatBytes, 0, fatBytes.Length);
			using (FileStream fs = new FileStream(VirtualDisk.DiskFileName, FileMode.Open))
            {
                fs.Seek(1024, SeekOrigin.Begin);
                fs.Write(fatBytes, 0, fatBytes.Length);
            }
        }

        public static void ReadFatTable()
        {
            byte[] fatBytes = new byte[NumberOfBlocks * 4];

            using (FileStream fs = new FileStream(VirtualDisk.DiskFileName, FileMode.Open))
            {
                fs.Seek(1024, SeekOrigin.Begin);
                fs.Read(fatBytes, 0, fatBytes.Length);
            }

            Buffer.BlockCopy(fatBytes, 0, fat, 0, fatBytes.Length);
        }

        public static void PrintFatTable(int start = 0, int end = 1024)
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

        public static List<int> GetFullFatValue(int firstCluster)
        {
            List<int> result = new List<int>();
            int current = firstCluster;
            while (current != -1 && current != 0)
            {
				result.Add(current);
				current = fat[current];
			}
            return result;
        }

        public static string GetFatValueAsString(int firstCluster)
        {
            List<int> fatDx = FatTable.GetFullFatValue(firstCluster);
            string fatValue = (fatDx.Count > 0) ? "[" + string.Join(" -> ", fatDx) + "]" : "";
            return fatValue;
		}

        public static int GetAvailableBlock() { return Array.FindIndex(fat, block => block == 0); }
        public static int GetValue(int index) { return fat[index]; }
        public static void SetValue(int index, int value) { fat[index] = value; WriteFatTable(); }
        public static int GetNumberOfFreeBlocks() { return fat.Count(block => block == 0); }
        public static int GetFreeSpace() { return GetNumberOfFreeBlocks() * 1024; }
	}
}
