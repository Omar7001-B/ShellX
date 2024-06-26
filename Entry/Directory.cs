﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellX.Entry
{
    public class Directory : DirectoryEntry
    {

        public List<DirectoryEntry> DirectoryTable { get; set; } = new List<DirectoryEntry>();

        public Directory(string name, Directory parent) : base(name, 1, 0, 0, parent) { }
        public Directory(DirectoryEntry entry, Directory parent) : base(entry.FileName, entry.FileAttribute, entry.FirstCluster, entry.FileSize, parent) { }


        // ------------ Serialize (Write) Functions ------------
        public override void ConvertContentToBytes()
        {
            ContentBytes.Clear();
            foreach (DirectoryEntry entry in DirectoryTable)
                ContentBytes.AddRange(entry.MetaToByteArray());
        }


        // ------------ Deserialize (Read) Functions ------------
        public override void ConvertBytesToContent()
        {
            DirectoryTable.Clear();
            int entryCount = ContentBytes.Count / 32;
            for (int i = 0; i < entryCount; i++)
            {
                byte[] entryData = ContentBytes.Skip(i * 32).Take(32).ToArray();
                if (entryData[0] == '#') break;
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
        public void AddChild(DirectoryEntry entry)
        {
            ReadEntryFromDisk();
            int index = Search(entry);
            if (index == -1) DirectoryTable.Add(entry);
            else
            {
				DirectoryTable[index] = entry;
				DirectoryTable.Add(entry);
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
		this.FirstCluster = FatTable.GetAvailableBlock();
		FatTable.SetValue(this.FirstCluster, -1);
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
		if(i >= FatIndex.Count) FatIndex.Add(FatTable.GetAvailableBlock());
		FatTable.SetValue(FatIndex[i], -1);
		if(i > 0) FatTable.SetValue(FatIndex[i - 1], FatIndex[i]);
		VirtualDisk.WriteBlock(blockData, FatIndex[i]);
	}

	FatTable.WriteFatTable();
}
*/

/*
public void ReadEntryFromDisk()
{
	DirectoryTable.Clear();
	List<byte> directoryBytes = new List<byte>();

	int currentCluster = this.FirstCluster;
	
	if(currentCluster < 5  || FatTable.GetValue(currentCluster) == 0)
		return;

	while (currentCluster != -1)
	{
		byte[] blockData = VirtualDisk.ReadBlock(currentCluster);
		directoryBytes.AddRange(blockData);
		currentCluster = FatTable.GetValue(currentCluster);
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
