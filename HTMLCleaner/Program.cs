using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "HTML Cleaner";
            if (args.Count() == 0)
                Logger.WriteLine("There are no arguments given");
            List<string> files = new List<string>();
            List<string> css_files = new List<string>();
            foreach(string file in args)
            {
                
                if (!File.Exists(file))
                    Logger.WriteLine("File does not exist: " + file);
                else
                {
                    if (Path.GetExtension(file) == ".html")
                        files.Add(file);
                    else if (Path.GetExtension(file) == ".css")
                        css_files.Add(file);
                }
                    
            }
            Logger.WriteLine("The application starts...");
            Parser parser = new Parser();
            parser.ReadFiles(files);
            foreach (var root in parser.roots)
            {
                Logger.WriteLine("Working on: "+ Path.GetFileName(root.FilePath));
                try
                {
                    parser.CheckForNotClosedTags(root);
                    parser.CleanAttributes(root);
                    parser.RemoveUnwantedTags(root);
                    parser.GenerateOutput(root);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }

            }

            foreach (string file in css_files)
                parser.ParseCss(file);

            Logger.WriteLine("The application has finished working.");
            Console.ReadKey();
        }
    }
}
