using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Shell_And_File_System__FAT_.Classes;

namespace Simple_Shell_And_File_System__FAT_.Entry
{
    public class Directory : DirectoryEntry
    {

        public List<DirectoryEntry> DirectoryTable { get; set; } = new List<DirectoryEntry>();

        public Directory(string name, Directory parent) : base(name, 1, 0, 0, parent) { }
        public Directory(DirectoryEntry entry, Directory parent) : base(entry.FileName, entry.FileAttribute, entry.FirstCluster, entry.FileSize, parent) { }


        // ------------ Serialize (Write) Functions ------------
        public override List<byte> ConvertContentToBytes()
        {
            List<byte> contentBytes = new List<byte>();
            foreach (DirectoryEntry entry in DirectoryTable)
                contentBytes.AddRange(entry.MetaToByteArray());
            return contentBytes;
        }


        // ------------ Deserialize (Read) Functions ------------
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


        // ------------ Copy Function --------------
        public override DirectoryEntry CopyEntry(Directory newParent)
        {
            ReadEntryFromDisk();
            Directory newDir = new Directory(FileName, newParent);
            foreach (DirectoryEntry entry in DirectoryTable)
                newDir.AddChild(entry.CopyEntry(newDir));
            WriteEntryToDisk();
            return newDir;
        }


        // ------------ Delete Function ------------
        public override void DeleteEntryFromDisk()
        {
            ReadEntryFromDisk();
            while (DirectoryTable.Count > 0)
                DirectoryTable[0].DeleteEntryFromDisk();
            base.DeleteEntryFromDisk();
        }


        // ------------ Search Functions ------------
        public int Search(string name) => DirectoryTable.FindIndex(entry => entry.FileName == FormateFileName(name));
        public int Search(DirectoryEntry entry) => Search(entry.FileName);
        public bool HasChild(string name) => Search(name) != -1;
        public bool HasChild(DirectoryEntry entry) => HasChild(entry.FileName);


        // ------------ Add/Remove Functions ------------
        public void AddChild(DirectoryEntry entry, bool overrideExisting = true)
        {
            int index = Search(entry);
            entry.Parent = this;
            if (index == -1) DirectoryTable.Add(entry);
            else
            {
                if (overrideExisting)
                {
                    DirectoryTable[index].ClearFat();
                    entry.WriteEntryToDisk();
                    DirectoryTable[index] = entry;
                }
                else
                {
                    string name = entry.FileName;
                    int copyNumber = 1;
                    while (HasChild(name + $"({copyNumber})")) copyNumber++;
                    entry.FileName = name + $"({copyNumber})";
                    DirectoryTable.Add(entry);
                }
            }
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
