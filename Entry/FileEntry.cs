using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellX.Entry
{
    internal class FileEntry : DirectoryEntry
    {
        public string Content { get; private set; }
        public FileEntry(string name, Directory parent) : base(name, 0, 0, 0, parent) { Content = ""; }
        public FileEntry(DirectoryEntry entry, Directory parent) : base(entry.FileName, entry.FileAttribute, entry.FirstCluster, entry.FileSize, parent) { Content = ""; }
        public override void ConvertBytesToContent(List<byte> data)
        {
            Content = Encoding.ASCII.GetString(data.ToArray()).Trim('#');
        }

        public override List<byte> ConvertContentToBytes()
        {
            return Encoding.ASCII.GetBytes(Content).ToList();
        }

        public override FileEntry CopyEntry(Directory newParent)
        {
            ReadEntryFromDisk();
            FileEntry newFile = new FileEntry(FileName, newParent);
            newFile.UpdateFile(Content);
            return newFile;
        }

        public void AppendFile(string text)
        {
            ReadEntryFromDisk();
            Content += text;
            WriteEntryToDisk();
        }

        public void UpdateFile(string text)
        {
            ReadEntryFromDisk();
            Content = text;
            WriteEntryToDisk();
        }

    }
}


// Old code
/*
public byte[] ToByteArray()
{
	return Encoding.ASCII.GetBytes(Content);
}

public void FromByteArray(byte[] data)
{
	Content = Encoding.ASCII.GetString(data).Trim();
}

public void ClearFat()
{
	int currentIndex = FirstCluster;
	while (currentIndex != -1 && currentIndex != 0)
	{
		int nextIndex = FatTable.getValue(currentIndex);
		FatTable.setValue(currentIndex, 0);
		VirtualDisk.writeBlock(VirtualDisk.GetEmptyBlock('#'), currentIndex);
		currentIndex = nextIndex;
	}
}

public void WriteEntryToDisk()
{
	List<byte> fileBytes = ToByteArray().ToList();
	List<int> FatIndex = new List<int>();


	int totalBytes = fileBytes.Count;
	int totalBlocks = ((totalBytes + 1023) / 1024);

	if (this.FirstCluster == 0)
	{
		this.FirstCluster = FatTable.getAvailableBlock();
		FatTable.setValue(this.FirstCluster, -1);
		if (Parent != null)
		{
			if (Parent.Search(FileName) != -1)
				Parent.DirectoryTable[Parent.Search(FileName)] = this;
			else
				Parent.DirectoryTable.Add(this);

			Parent?.WriteEntryToDisk();
		}
	}

	FatIndex.Add(this.FirstCluster); ClearFat();

	for (int i = 0; i < totalBlocks; i++)
	{
		int blockSize = Math.Min(1024, totalBytes - (i * 1024));
		byte[] blockData = fileBytes.Skip(i * 1024).Take(blockSize).ToArray();
		if (i >= FatIndex.Count) FatIndex.Add(FatTable.getAvailableBlock());
		FatTable.setValue(FatIndex[i], -1);
		if (i > 0) FatTable.setValue(FatIndex[i - 1], FatIndex[i]);
		VirtualDisk.writeBlock(blockData, FatIndex[i]);
	}

	FatTable.writeFatTable();
}

public void ReadEntryFromDisk()
{
	Content = string.Empty;
	int currentIndex = FirstCluster;
	while (currentIndex != -1 && currentIndex != 0)
	{
		byte[] blockData = VirtualDisk.readBlock(currentIndex);
		Content += Encoding.ASCII.GetString(blockData);
		currentIndex = FatTable.getValue(currentIndex);
	}

	Content = Content.Trim('#');
}
*/
