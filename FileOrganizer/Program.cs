using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            LoopThrough();
        }

        static void LoopThrough()
        {
            List<string> filesList = Directory.GetFiles(@"C:\Users\Herczeg Zoltán\Desktop\Test").ToList();
            List<string> folderList = Directory.GetDirectories(@"C:\Users\Herczeg Zoltán\Desktop\Test").ToList();


            foreach (var item in filesList)
            {
                var extension = Path.GetExtension(item);
                if (true)
                {

                }
                Console.WriteLine("ext:" + extension + " " + item);
            }

            foreach (var item in folderList)
            {
                string getFolderName = new DirectoryInfo(Path.GetDirectoryName(item)).Name;

                Console.WriteLine("folder:" + getFolderName + " " + item);
            }
        }
    }
}
