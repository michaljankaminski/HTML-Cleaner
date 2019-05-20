using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class TreeRoot
    {
        public TreeNode Child { get; set; } = null;
        public string Doctype { get; set; } = String.Empty;
        public void BuildTree(string file)
        {
            string current_line = String.Empty;
            using (StreamReader reader = new StreamReader(file))
            {
                Regex regex = RegexPatterns.Doctype;
                current_line = reader.ReadLine();
                Match match = regex.Match(current_line);
                if (match.Value != string.Empty)
                    this.Doctype = match.Value;
                while (!reader.EndOfStream)
                {
                    regex = RegexPatterns.HTMLTag;
                    if (regex.Match(current_line).Value != string.Empty)
                    {
                        Child = new TreeNode(null, 0, reader,0,current_line );
                    }
                    current_line = reader.ReadLine();
                }
            }
        }
    }
}
