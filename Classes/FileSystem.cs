using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    internal class FileSystem
    {
        private string currentDirectory;
        public FileSystem()
        {
            currentDirectory = "root:\\";
        }

        public string GetCurrenDirectory()
        {
            return currentDirectory;
        }
    }
}
