using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public class FileSystem
    {
        public Directory CurrentDirectory { get; set; }

        public FileSystem()
        {
            Initialize();
        }

        private void Initialize()
        {
            // Initialize the root directory
            CurrentDirectory = new Directory("root", 1, 0, 0, null);
            CurrentDirectory.WriteDirectory();
        }

        public string GetCurrenDirectory()
        {
			return CurrentDirectory.Filename;
		}
    }

}
