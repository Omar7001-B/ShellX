using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public class Directory : DirectoryEntry
    {
        public List<DirectoryEntry> DirectoryTable { get; set; } = new List<DirectoryEntry>();
        public Directory Parent { get; set; }

        public Directory() : base() { }

        public Directory(string name, byte attribute, int cluster, int size, Directory parent) 
            : base(name, attribute, cluster, size) { Parent = parent; }

        public void WriteDirectory()
        {
            List<byte> directoryBytes = new List<byte>();

            foreach (DirectoryEntry entry in DirectoryTable)
                directoryBytes.AddRange(entry.ToByteArray());

            int totalBytes = directoryBytes.Count;
            int totalBlocks = (totalBytes / 1024) + 1;

            int firstCluster = FirstCluster != 0 ? FirstCluster : FatTable.getAvailableBlock();

            int currentCluster = firstCluster;
            int nextCluster;

            for (int i = 0; i < totalBlocks; i++)
            {
                int blockSize = Math.Min(1024, totalBytes - (i * 1024));
                byte[] blockData = directoryBytes.Skip(i * 1024).Take(blockSize).ToArray();
                VirtualDisk.writeBlock(blockData, currentCluster);

                nextCluster = (i < totalBlocks - 1) ? FatTable.getAvailableBlock() : -1;
                FatTable.setValue(nextCluster, currentCluster);

                currentCluster = nextCluster;
            }

            FatTable.writeFatTable();
        }

        public void ReadDirectory()
        {
            List<byte> directoryBytes = new List<byte>();

            int firstCluster = FirstCluster;
            int currentCluster = firstCluster;

            while (currentCluster != -1)
            {
                byte[] blockData = VirtualDisk.readBlock(currentCluster);
                directoryBytes.AddRange(blockData);

                currentCluster = FatTable.getValue(currentCluster);
            }

            int entryCount = directoryBytes.Count / 32;
            for (int i = 0; i < entryCount; i++)
            {
                byte[] entryData = directoryBytes.Skip(i * 32).Take(32).ToArray();
                DirectoryEntry entry = new DirectoryEntry();
                entry = entry.FromByteArray(entryData);
                DirectoryTable.Add(entry);
            }
        }

        public void DeleteDirectory()
        {
            if (new string(Filename).Equals("root", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Cannot delete root directory.");
                return;
            }

            int currentIndex = FirstCluster;
            int nextIndex;

            while (currentIndex != -1)
            {
                nextIndex = FatTable.getValue(currentIndex);
                FatTable.setValue(0, currentIndex);
                currentIndex = nextIndex;
            }

            if (Parent != null)
            {
                int index = Parent.Search(new string(Filename));
                if (index != -1)
                {
                    Parent.DirectoryTable.RemoveAt(index);
                    Parent.WriteDirectory();
                }
                FatTable.writeFatTable();
            }

            Console.WriteLine("Directory deleted.");
        }


        public int Search(string name)
        {
            for (int i = 0; i < DirectoryTable.Count; i++)
                if (DirectoryTable[i].Filename == name) return i;
            return -1;
        }
    }

}
