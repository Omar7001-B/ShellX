using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
	public static class VirtualDisk
	{

		public const string DiskFileName = "Disk.txt";

        public static byte[] EmptyBlock = Encoding.ASCII.GetBytes(new string('#', 1024));

        public static byte[] GetEmptyBlock(char c) { return Encoding.ASCII.GetBytes(new string(c, 1024)); }
        public static void Initialize()
        {
            if (!File.Exists(DiskFileName))
            {
                using (FileStream fs = new FileStream(DiskFileName, FileMode.CreateNew))
                {
                    byte[] superBlock = new byte[1024];
                    byte[] fatTableBlock = new byte[4 * 1024];
                    byte[] storageBlock = new byte[1019 * 1024];

                    Array.Fill(superBlock, (byte)'0');
                    Array.Fill(fatTableBlock, (byte)'*');
                    Array.Fill(storageBlock, (byte)'#');

                    fs.Write(superBlock, 0, superBlock.Length);
                    fs.Write(fatTableBlock, 0, fatTableBlock.Length);
                    fs.Write(storageBlock, 0, storageBlock.Length);
                }

                FatTable.Initialize(); // int fat[1024]
            }
            else
            {
                FatTable.readFatTable();
            }
        }

        public static void writeBlock(byte[] data, int index)
        {
            using (FileStream fs = new FileStream(DiskFileName, FileMode.Open))
            {
                fs.Seek(index * 1024, SeekOrigin.Begin);
                fs.Write(data, 0, data.Length);
            }
        }

        public static byte[] readBlock(int index)
        {
            byte[] buffer = new byte[1024];
            using (FileStream fs = new FileStream(DiskFileName, FileMode.Open))
            {
                fs.Seek(index * 1024, SeekOrigin.Begin);
                fs.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }

	}
}
