using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string path = @"C:\Users\Herczeg Zoltán\Desktop\Test";
            
            

            Run(path);
        }

        [PermissionSet(SecurityAction.Demand, Name ="FullTrust")]
        private static void Run(string path)
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {

                watcher.Path = path;

                watcher.Changed += OnChanged;
                
                watcher.NotifyFilter = NotifyFilters.Size;
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit .");

                while (Console.Read() != 'q') ;
            } 
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            LoopThrough(e.FullPath);
        }

        static void LoopThrough(string path)
        {
            path = Path.GetDirectoryName(path);

            List<string> filesList = Directory.GetFiles(path).ToList();
            List<string> folderList = Directory.GetDirectories(path).ToList();

            foreach (var item in filesList)
            {
                GetFolderNameOfFile(item);
                MoveFile(item);
            }
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
            try
            {
                File.Move(filePath, Path.Combine(filePathWithoutFileName, extension, name));
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not move the file yet probably because it was large and still copying into the folder!");                try
                {
                    MoveFile(filePath);
                    Console.WriteLine("It could move the file!");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
