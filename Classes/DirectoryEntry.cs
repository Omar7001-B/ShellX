using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
------------------------------------------------------------------------------------------------------------------------
| Filename (11 bytes) | FileAttribute (1 byte) | FileEmpty (12 bytes) | FirstCluster (4 bytes) | FileSize (4 bytes)   |
-------------------------------------------------------------------------------------------------------------------------
|        Name [0,10]  |         Attr [11]      |   12 zeros [12,23]  | Cluster Number [24,27] | Size in Bytes [28,31] |
------------------------------------------------------------------------------------------------------------------------
 */

namespace Simple_Shell_And_File_System__FAT_.Classes
{
	public class DirectoryEntry
	{
		public string Filename { get; set; } = ""; // 11 bytes
        public byte FileAttribute { get; set; } // 1 byte
        public byte[] FileEmpty { get; set; } = new byte[12]; // 12 bytes
        public int FirstCluster { get; set; } // 4 bytes
        public int FileSize { get; set; } // 4 bytes

        public DirectoryEntry() { }

        public DirectoryEntry(string name, byte attribute, int cluster, int size)
        {
            if (attribute == 0 && name.Contains("."))
            {
                string[] parts = name.Split('.'); parts[1] = parts[1].Substring(0, Math.Min(3, parts[1].Length));
				Filename = $"{parts[0].Substring(0, Math.Min(11 - parts[1].Length, parts[0].Length))}.{parts[1]}";
            }
            else
                Filename = name.Substring(0, Math.Min(11, name.Length));
            FileAttribute = attribute;
            FileSize = size;
            FirstCluster = cluster == 0 ? FatTable.getAvailableBlock() : cluster;
        }


        public DirectoryEntry(byte[] data)
        {
			DirectoryEntry entry = FromByteArray(data);
            this.Filename = entry.Filename;
            this.FileAttribute = entry.FileAttribute;
            this.FirstCluster = entry.FirstCluster;
            this.FileSize = entry.FileSize;
		}



        public byte[] ToByteArray()
        {
            byte[] data = new byte[32];
            Encoding.ASCII.GetBytes(Filename.PadRight(11)).CopyTo(data, 0);
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
                Filename = Encoding.ASCII.GetString(data, 0, 11).Trim(),
                FileAttribute = data[11],
                FirstCluster = BitConverter.ToInt32(data, 24),
                FileSize = BitConverter.ToInt32(data, 28)
            };
        }
	}
}


