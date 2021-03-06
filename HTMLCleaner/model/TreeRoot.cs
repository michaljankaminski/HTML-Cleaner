﻿using System;
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
        public string FilePath { get; set; } = string.Empty;
        public TreeNode Child { get; set; } = null;
        public string Doctype { get; set; } = string.Empty;
        public void BuildTree(string file)
        {
            Dictionaries.Instance.CurrentNumber = 0;
            this.FilePath = file;
            string current_line = String.Empty;
            using (StreamReader reader = new StreamReader(file))
            {
                Regex regex = RegexPatterns.Doctype;
                current_line = reader.ReadLine();
                Dictionaries.Instance.CurrentNumber++;
                Match match = regex.Match(current_line);
                if (match.Value != string.Empty)
                    this.Doctype = match.Value;
                while (!reader.EndOfStream)
                {
                    regex = RegexPatterns.HTMLTag;
                    if (regex.Match(current_line).Value != string.Empty)
                    {
                        int current_open = 0;
                        int current_closed = 0;
                        Child = new TreeNode(null, reader,ref current_open, ref current_closed, current_line );
                    }
                    current_line = reader.ReadLine();
                    Dictionaries.Instance.CurrentNumber++;
                }
            }
        }
    }
}
