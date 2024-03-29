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
            int totalBlocks = ((totalBytes + 1023) / 1024);


            if(this.FirstCluster == 0)
				this.FirstCluster = FatTable.getAvailableBlock();

            int previousCluster = 0;

            byte[] EmptyBlock = new byte[1024];
            Array.Fill(EmptyBlock, (byte)'#');

            for (int i = 0; i < totalBlocks; i++)
            {
                int blockSize = Math.Min(1024, totalBytes - (i * 1024));
                byte[] blockData = directoryBytes.Skip(i * 1024).Take(blockSize).ToArray();

                int nextCluster = FatTable.getAvailableBlock();
                VirtualDisk.writeBlock(EmptyBlock, nextCluster);
                VirtualDisk.writeBlock(blockData, nextCluster);


                FatTable.setValue(nextCluster, -1);
                if(previousCluster != 0)
					FatTable.setValue(previousCluster, nextCluster);
                previousCluster = nextCluster;
            }

            FatTable.writeFatTable();
        }

        public void ReadDirectory()
        {
            List<byte> directoryBytes = new List<byte>();

            int currentCluster = this.FirstCluster;

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
                DirectoryTable.Add(new DirectoryEntry(entryData));
            }

            // Remove empty entries
            DirectoryTable.RemoveAll(entry => entry.Filename == "###########");
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
                FatTable.setValue(currentIndex, 0);
                currentIndex = nextIndex;
            }

            if (Parent != null)
            {
                int index = Parent.Search(new string(Filename));
                if (index != -1)
                {
                    Parent.DirectoryTable.RemoveAt(index);
                    Parent.WriteDirectory();
					FatTable.writeFatTable();
                }
            }

            Console.WriteLine("Directory deleted.");
        }


        public int Search(string name)
        {
            for (int i = 0; i < DirectoryTable.Count; i++)
                if (DirectoryTable[i].Filename == name) return i;
            return -1;
        }


        public void PrintDirectoryContents() // For testing purposes
        {
            // Print Number of files in the directory
			Console.WriteLine($"Directory: {new string(Filename)}");
            Console.WriteLine($"Number of Files: {DirectoryTable.Count}");
			foreach (DirectoryEntry entry in DirectoryTable)
            {
				Console.WriteLine($"Filename: {new string(entry.Filename)}");
				Console.WriteLine($"File Attribute: {entry.FileAttribute}");
				Console.WriteLine($"First Cluster: {entry.FirstCluster}");
				Console.WriteLine($"File Size: {entry.FileSize}");
				Console.WriteLine();
			}
		}
    }

}
