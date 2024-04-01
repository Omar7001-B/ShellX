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

        public Directory(DirectoryEntry entry, Directory parent = null)
                : base(entry.FileName, entry.FileAttribute, entry.FirstCluster, entry.FileSize)
        { Parent = parent; }


        public void ClearFat() 
        {
            int currentIndex = FirstCluster;
            while(currentIndex != -1 && currentIndex != 0)
            {
				int nextIndex = FatTable.getValue(currentIndex);
				FatTable.setValue(currentIndex, 0);
				currentIndex = nextIndex;
			}
		}

        public void WriteDirectory()
        {
            List<byte> directoryBytes = new List<byte>();
			List<int> FatIndex  = new List<int>();

            foreach (DirectoryEntry entry in DirectoryTable)
                directoryBytes.AddRange(entry.ToByteArray());

            byte[] EmptyBlock = new byte[1024];
            Array.Fill(EmptyBlock, (byte)'#');

            int totalBytes = directoryBytes.Count;
            int totalBlocks = ((totalBytes + 1023) / 1024);


            if(this.FirstCluster == 0)
            {
				this.FirstCluster = FatTable.getAvailableBlock();
                FatTable.setValue(this.FirstCluster, -1);
                if (Parent != null)
                {
                    if(Parent.Search(FileName) != -1)
                        Parent.DirectoryTable[Parent.Search(FileName)] = this;
                    else 
                        Parent.DirectoryTable.Add(this);

                    Parent?.WriteDirectory();
                }
            }

            FatIndex.Add(this.FirstCluster); ClearFat();

            for(int i = 0; i < totalBlocks; i++)
            {
                int blockSize = Math.Min(1024, totalBytes - (i * 1024));
                byte[] blockData = directoryBytes.Skip(i * 1024).Take(blockSize).ToArray();

                if(i >= FatIndex.Count)
                    FatIndex.Add(FatTable.getAvailableBlock());

                FatTable.setValue(FatIndex[i], -1);
				if(i > 0) FatTable.setValue(FatIndex[i - 1], FatIndex[i]);

                VirtualDisk.writeBlock(EmptyBlock, FatIndex[i]);
                VirtualDisk.writeBlock(blockData, FatIndex[i]);
            }

            FatTable.writeFatTable();
        }

        public void ReadDirectory()
        {
            DirectoryTable.Clear();
            List<byte> directoryBytes = new List<byte>();

            int currentCluster = this.FirstCluster;
            
            if(currentCluster < 5  || FatTable.getValue(currentCluster) == 0)
				return;

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
                DirectoryTable.Add(GetActualType(new DirectoryEntry(entryData)));
            }

            // Remove empty entries
             DirectoryTable.RemoveAll(entry => entry.FileName == "###########");
        }

        public DirectoryEntry GetActualType(DirectoryEntry entry)
        {
			if (entry.FileAttribute == 1)
            {
				Directory directory = new Directory(entry, this);
				return directory;
			}
			return entry;
		}

        public void DeleteDirectory()
        {
            if (new string(FileName).Equals("root", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Cannot delete root directory.");
                return;
            }

            ReadDirectory(); //Remove
            while(DirectoryTable.Count > 0)
				if (DirectoryTable[0] is Directory directory)
					directory.DeleteDirectory();

            ClearFat();

            if (Parent != null)
            {
                int index = Parent.Search(new string(FileName));
                if (index != -1)
                {
                    Parent.DirectoryTable.RemoveAt(index);
                    Parent.WriteDirectory();
                }
            }

			FatTable.writeFatTable();
            //Console.WriteLine("Directory deleted.");
        }

        public int Search(string name)
        {
            for (int i = 0; i < DirectoryTable.Count; i++)
                if (DirectoryTable[i].FileName == FormateFileName(name)) return i;
            return -1;
        }

        public override void UpdateName(string newName) // Function Update Name in Directory
        {
            base.UpdateName(newName);
            Parent.WriteDirectory();
        }
    }

}
