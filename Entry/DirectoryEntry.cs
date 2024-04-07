using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualDisk = ShellX.Disk.VirtualDisk;
using FatTable = ShellX.Disk.FatTable;

/*
------------------------------------------------------------------------------------------------------------------------
| FileName (11 bytes) | FileAttribute (1 byte) | FileEmpty (12 bytes) | FirstCluster (4 bytes) | FileSize (4 bytes)   |
-------------------------------------------------------------------------------------------------------------------------
|        Name [0,10]  |         Attr [11]      |   12 zeros [12,23]  | Cluster Number [24,27] | Size in Bytes [28,31] |
------------------------------------------------------------------------------------------------------------------------
 */

namespace ShellX.Entry
{
    public class DirectoryEntry
    {
        public string FileName { get; private set; } = ""; // 11 bytes
        public byte FileAttribute { get; private set; } // 1 byte
        public byte[] FileEmpty { get; private set; } = new byte[12]; // 12 bytes
        public int FirstCluster { get; private set; } // 4 bytes
        public int FileSize { get; private set; } // 4 bytes
        public Directory Parent { get; private set; }

        private protected List<byte> ContentBytes { get; set; } =  new List<byte>();

        public DirectoryEntry() { }

        // ---- Constructors ----
        public DirectoryEntry(string name, byte attribute, int cluster, int size, Directory parent)
        {
            FileName = FormateFileName(name, attribute);
            FileAttribute = attribute;
            FileSize = size;
            FirstCluster = cluster;
            Parent = parent;
            // parent?.AddChild(this); // Causes Circular Dependency
        }

        public DirectoryEntry(DirectoryEntry entry, Directory parent) : this(entry.FileName, entry.FileAttribute, entry.FirstCluster, entry.FileSize, parent) { }

        public DirectoryEntry(byte[] data)
        {
            DirectoryEntry entry = MetaFromByteArray(data);
            FileName = entry.FileName;
            FileAttribute = entry.FileAttribute;
            FirstCluster = entry.FirstCluster;
            FileSize = entry.FileSize;
        }


        // ---- Meta Functions ----
        public byte[] MetaToByteArray()
        {
            byte[] data = new byte[32];
            Encoding.ASCII.GetBytes(FileName.PadRight(11)).CopyTo(data, 0);
            data[11] = FileAttribute;
            FileEmpty.CopyTo(data, 12);
            BitConverter.GetBytes(FirstCluster).CopyTo(data, 24);
            BitConverter.GetBytes(FileSize).CopyTo(data, 28);
            return data;
        }

        public DirectoryEntry MetaFromByteArray(byte[] data)
        {
            return new DirectoryEntry
            {
                FileName = Encoding.ASCII.GetString(data, 0, 11).Trim(),
                FileAttribute = data[11],
                FirstCluster = BitConverter.ToInt32(data, 24),
                FileSize = BitConverter.ToInt32(data, 28)
            };
        }

        // ------ Read Functions --------    
        public void ReadEntryFromDisk()
        {
            ContentBytes.Clear();
            if (FirstCluster == 0 || FatTable.GetValue(FirstCluster) == 0) return;
            int currentCluster = FirstCluster;

            while (currentCluster != -1)
            {
                byte[] blockData = VirtualDisk.ReadBlock(currentCluster);
                ContentBytes.AddRange(blockData);
                currentCluster = FatTable.GetValue(currentCluster);
            }
            ConvertBytesToContent(); // Virtual Function
        }


        public virtual void ConvertBytesToContent()
        {
            Console.WriteLine("Bytes To Content from Parent");
        }


        // ------ Write Functions --------
        public void WriteEntryToDisk()
        {
            ConvertContentToBytes(); // Virtual Function
            ClearFat(); AllocateFirstCluster();
            List<int> fatIndex = new List<int>() { FirstCluster };
            int totalBytes = FileSize = ContentBytes.Count;
            int totalBlocks = (totalBytes + 1023) / 1024;
            for (int i = 0; i < totalBlocks; i++)
            {
                int blockSize = Math.Min(1024, totalBytes - i * 1024);
                byte[] blockData = ContentBytes.Skip(i * 1024).Take(blockSize).ToArray();
                if (i >= fatIndex.Count) fatIndex.Add(FatTable.GetAvailableBlock());
                FatTable.SetValue(fatIndex[i], -1);
                if (i > 0) FatTable.SetValue(fatIndex[i - 1], fatIndex[i]);
                VirtualDisk.WriteBlock(blockData, fatIndex[i]);
            }

        }
        public virtual void ConvertContentToBytes()
        {
            Console.WriteLine("Content To Bytes from Parent");
        }

        // ------ Copy Functions --------
        public virtual DirectoryEntry CopyEntry(Directory newParent)
        {
            return new DirectoryEntry(FileName, FileAttribute, 0, FileSize, newParent);
        }

        // ------ Move Functions --------
        public virtual void MoveEntry(Directory newParent)
        {
            int index = Parent.Search(this);
            if (index != -1)
            {
				Parent.DirectoryTable.RemoveAt(index);
				Parent.WriteEntryToDisk();
            }

			Parent = newParent;
            newParent.DirectoryTable.Add(this);
			newParent.WriteEntryToDisk();
		}

        // ------ Delete Functions --------
        public virtual void DeleteEntryFromDisk()
        {
            ClearFat();
            int index = Parent.Search(this);
            if (index == -1) return;
            Parent.DirectoryTable.RemoveAt(index);
            Parent.WriteEntryToDisk();
        }

        // ------ General Functions --------
        public void ClearFat()
        {
            int currentIndex = FirstCluster;
            while (currentIndex != -1 && currentIndex != 0)
            {
                int nextIndex = FatTable.GetValue(currentIndex);
                FatTable.SetValue(currentIndex, 0);
                VirtualDisk.WriteBlock(VirtualDisk.GetEmptyBlock('#'), currentIndex);
                currentIndex = nextIndex;
            }
        }

        private void AllocateFirstCluster()
        {
            if (ContentBytes.Count > 0)
				 SetFirstCluster((FirstCluster == 0 ? FatTable.GetAvailableBlock() : FirstCluster));
            else
                SetFirstCluster(0);

        }
        public void SetFirstCluster(int cluster)
        {
            FirstCluster = cluster;
            if(FirstCluster != 0) FatTable.SetValue(FirstCluster, -1);

            if(Parent == null) return;

			int index = Parent.Search(this);
			if (index == -1)
            {
                 // Console.WriteLine($"Entry {FileName} is not found in Parent {Parent.FileName}."); // Debugging
                return;
            }
			Parent.DirectoryTable[index].FirstCluster = FirstCluster;
			Parent?.WriteEntryToDisk();
        }

        public virtual void SetName(string name)
        {
            int index = Parent.Search(this);
            if (index == -1) return;
            Parent.DirectoryTable[index].FileName = name; 
            this.FileName = name;
			Parent?.WriteEntryToDisk();
        }

        public static string FormateFileName(string name, int attribute = 1)
        {
            name = name.Trim();
            if (name.Contains("."))
            {
                string[] parts = name.Split('.'); parts[1] = parts[1].Substring(0, Math.Min(3, parts[1].Length));
                return $"{parts[0].Substring(0, Math.Min(11 - parts[1].Length - 1, parts[0].Length))}.{parts[1]}";
            }
            return name.Substring(0, Math.Min(11, name.Length));
        }
    }
}


