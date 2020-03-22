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
                GetFolderNameOfFile(item);
                MoveFile(item);
            }

            //foreach (var item in folderList)
            //{
            //    string getFolderName = new DirectoryInfo(Path.GetDirectoryName(item)).Name;

            //    Console.WriteLine("folder:" + getFolderName + " " + item);
            //}
        }


        static bool GetFolderNameOfFile(string item)
        {
            var extension = Path.GetExtension(item);
            var filePathWithoutFileName = Path.GetDirectoryName(item);
            
            if (!File.Exists(Path.Combine(filePathWithoutFileName,extension)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Path.Combine(filePathWithoutFileName, extension));
            }
            return true;
        }

        static void MoveFile(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            var name = Path.GetFileName(filePath);
            var filePathWithoutFileName = Path.GetDirectoryName(filePath);

            File.Move(filePath,Path.Combine(filePathWithoutFileName, extension,name));
        }
    }
}
