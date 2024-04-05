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

        public Directory() : base() { }

        public Directory(string name, byte attribute, int cluster, int size, Directory parent) 
            : base(name, attribute, cluster, size) { Parent = parent; }

        public Directory(DirectoryEntry entry, Directory parent = null)
                : base(entry.FileName, entry.FileAttribute, entry.FirstCluster, entry.FileSize)
        { Parent = parent; }


        public override List<byte> ConvertContentToBytes()
        {
            List<byte> contentBytes = new List<byte>();
            foreach (DirectoryEntry entry in DirectoryTable)
                contentBytes.AddRange(entry.MetaToByteArray());
            return contentBytes;
        }


        public override void ConvertBytesToContent(List<byte> data)
        {
			DirectoryTable.Clear();
			int entryCount = data.Count / 32;
			for (int i = 0; i < entryCount; i++)
            {
                byte[] entryData = data.Skip(i * 32).Take(32).ToArray();
				DirectoryTable.Add(GetActualType(new DirectoryEntry(entryData)));
			}

			// Remove empty entries
			DirectoryTable.RemoveAll(entry => entry.FileName == "###########");
		}

        public DirectoryEntry GetActualType(DirectoryEntry entry)
        {
            return entry.FileAttribute switch
            {
                1 => new Directory(entry, this),
                0 => new FileEntry(entry, this),
                _ => entry
            };
        }

		public override void DeleteEntryFromDisk()
		{
			ReadEntryFromDisk();
			while (DirectoryTable.Count > 0)
				DirectoryTable[0].DeleteEntryFromDisk();
			base.DeleteEntryFromDisk();
		}
		
        public int Search(string name)
        {
            for (int i = 0; i < DirectoryTable.Count; i++)
                if (DirectoryTable[i].FileName == FormateFileName(name)) return i;
            return -1;
        }

		public int Search(DirectoryEntry entry)
		{
			return Search(entry.FileName);
		}

        public void AddChild(DirectoryEntry entry)
        {
            ReadEntryFromDisk();
			int index = Search(entry);
			entry.Parent = this;
            if(index == -1) DirectoryTable.Add(entry);
			else DirectoryTable[index] = entry;
			WriteEntryToDisk();
		}

        public void RemoveChild(DirectoryEntry entry)
        {
			ReadEntryFromDisk();
			int index = Search(entry.FileName);
			if (index != -1) DirectoryTable.RemoveAt(index);
			WriteEntryToDisk();
		}
    }

}


// Old Functions
/*
public void WriteEntryToDisk()
{
	List<byte> directoryBytes = new List<byte>();
	List<int> FatIndex  = new List<int>();

	foreach (DirectoryEntry entry in DirectoryTable)
		directoryBytes.AddRange(entry.MetaToByteArray());

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

			Parent?.WriteEntryToDisk();
		}
	}

	FatIndex.Add(this.FirstCluster); ClearFat();

	for(int i = 0; i < totalBlocks; i++)
	{
		int blockSize = Math.Min(1024, totalBytes - (i * 1024));
		byte[] blockData = directoryBytes.Skip(i * 1024).Take(blockSize).ToArray();
		if(i >= FatIndex.Count) FatIndex.Add(FatTable.getAvailableBlock());
		FatTable.setValue(FatIndex[i], -1);
		if(i > 0) FatTable.setValue(FatIndex[i - 1], FatIndex[i]);
		VirtualDisk.writeBlock(blockData, FatIndex[i]);
	}

	FatTable.writeFatTable();
}
*/

/*
public void ReadEntryFromDisk()
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
*/
