using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
------------------------------------------------------------------------------------------------------------------------
| FileName (11 bytes) | FileAttribute (1 byte) | FileEmpty (12 bytes) | FirstCluster (4 bytes) | FileSize (4 bytes)   |
-------------------------------------------------------------------------------------------------------------------------
|        Name [0,10]  |         Attr [11]      |   12 zeros [12,23]  | Cluster Number [24,27] | Size in Bytes [28,31] |
------------------------------------------------------------------------------------------------------------------------
 */

namespace Simple_Shell_And_File_System__FAT_.Classes
{
	public class DirectoryEntry
	{
		public string FileName { get; set; } = ""; // 11 bytes
        public byte FileAttribute { get; set; } // 1 byte
        public byte[] FileEmpty { get; set; } = new byte[12]; // 12 bytes
        public int FirstCluster { get; set; } // 4 bytes
        public int FileSize { get; set; } // 4 bytes

        public DirectoryEntry() { }

        public DirectoryEntry(string name, byte attribute, int cluster, int size)
        {
            FileName = FormateFileName(name, attribute);
            FileAttribute = attribute;
            FileSize = size;
            FirstCluster = cluster == 0 ? FatTable.getAvailableBlock() : cluster;
        }

        public DirectoryEntry(byte[] data)
        {
			DirectoryEntry entry = FromByteArray(data);
            this.FileName = entry.FileName;
            this.FileAttribute = entry.FileAttribute;
            this.FirstCluster = entry.FirstCluster;
            this.FileSize = entry.FileSize;
		}

        public byte[] ToByteArray()
        {
            byte[] data = new byte[32];
            Encoding.ASCII.GetBytes(FileName.PadRight(11)).CopyTo(data, 0);
            data[11] = FileAttribute;
            FileEmpty.CopyTo(data, 12);
            BitConverter.GetBytes(FirstCluster).CopyTo(data, 24);
            BitConverter.GetBytes(FileSize).CopyTo(data, 28);
            return data;
        }

        public DirectoryEntry FromByteArray(byte[] data)
        {
            return new DirectoryEntry
            {
                FileName = Encoding.ASCII.GetString(data, 0, 11).Trim(),
                FileAttribute = data[11],
                FirstCluster = BitConverter.ToInt32(data, 24),
                FileSize = BitConverter.ToInt32(data, 28)
            };
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


