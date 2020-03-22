using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileOrganizer
{
    class Program
    {
        
        static void Main(string[] args)
        {

            Console.WriteLine("Choose which folder you want to organize!");

            Console.WriteLine("1: User\\Documents!");
            Console.WriteLine("2: User\\Downloads!");
            Console.WriteLine("3: User\\Desktop!");
            Console.WriteLine("4: Custom! example: C:\\Users\\TestUser\\Desktop\\TestFolder");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Select one wiht type the correct number!");
            string path = GetSelectedPath();
            Run(path);
        }

        static string GetSelectedPath()
        {
            string path = "";
            switch (Console.ReadLine())
            {
                case "1":
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    return path;

                case "2":
                    string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
                    path = Path.Combine(userRoot, "Downloads");
                    return path;
                case "3":
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    return path;
                case "4":
                    Console.WriteLine("Add your custom path:");
                    string pathConsole = Console.ReadLine();
                    if (IsValidPath(pathConsole))
                    {
                        Console.WriteLine("Valid Path!");
                        path = pathConsole;
                    }
                    return path;

                default:
                    Console.WriteLine("Invalid selection!");
                    path = "0"; 
                    return path;
            }
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

        static bool IsValidPath(string path)
        {
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (!driveCheck.IsMatch(path.Substring(0, 3))) return false;
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
                return false;

            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(path));
            if (!dir.Exists)
                dir.Create();
            return true;
        }
    }
}
