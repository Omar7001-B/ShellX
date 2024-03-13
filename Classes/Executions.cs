using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Shell_And_File_System__FAT_.Classes
{
	public static class Executions
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
