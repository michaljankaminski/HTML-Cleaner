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
            if (args.Count() == 0)
                Console.WriteLine("Brak podanych plików;");
            List<string> files = new List<string>();
            foreach(string file in args)
            {
                if (!File.Exists(file))
                    Console.WriteLine("Plik nie istnieje: " + file);
                else
                    files.Add(file);
            }
            Parser parser = new Parser();
            parser.ReadFiles(files);
            Console.WriteLine("Zakonczono działanie aplikacji");
            Console.ReadKey();
        }
    }
}
