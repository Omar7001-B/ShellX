using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple_Shell_And_File_System__FAT_.Classes;
using Directory = Simple_Shell_And_File_System__FAT_.Classes.Directory;
using DirectoryEntry = Simple_Shell_And_File_System__FAT_.Classes.DirectoryEntry;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
    public static class Executions // This is archieve for now. It will be used in the future.
    {

        public static void Cls(string[] args)
        {
            Console.Clear();
        }

        public static void Quit(string[] args)
        {
            Environment.Exit(0);
        }



    }
}
