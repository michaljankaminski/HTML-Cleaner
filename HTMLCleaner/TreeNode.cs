using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class TreeNode
    {
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();
        public TreeNode Parent { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Level { get; set; } = 0;
        public bool Closed { get; set; } = false;
        public TreeNode(TreeNode parent, int level, StreamReader reader, int current_match, string current_line_arg)
        {
            
            this.Level = ++level;
            if (parent != null)
                this.Parent = parent;
            var current_line = current_line_arg;
            var regex = RegexPatterns.HTMLTag;
            var matches = regex.Matches(current_line);
            this.FullName = matches[current_match].Value;
            this.Name = GetSimpleTag(this.FullName);
            Console.WriteLine(Name);
            Dictionaries dictionaries = new Dictionaries();
            if (dictionaries.TagWithoutClosing.ContainsKey(this.Name))
                this.Closed = true;

            current_match++;
            while (!reader.EndOfStream)
            {
                regex = RegexPatterns.HTMLTagClosed;
                matches = regex.Matches(current_line);
                foreach(Match match in matches)
                {
                    CloseTag(GetSimpleTag(match.Value));
                }
                if (this.Closed)
                    break;
                regex = RegexPatterns.HTMLTag;
                matches = regex.Matches(current_line);
                for (int i = current_match;i<matches.Count; i++)
                {
                    Children.Add(new TreeNode(this, Level, reader, current_match, current_line));
                }
                current_match = 0;
                current_line = reader.ReadLine();
            }
        }
        bool CloseTag(string name)
        {
            TreeNode current = this;
            while (current != null)
            {
                if (current.Name == name)
                {
                    current.Closed = true;
                    return true;
                }
                current = current.Parent;    

            }
            return false;
        }
        private string GetSimpleTag(string full_tag)
        {//<html>
            if (full_tag.IndexOf(' ') != -1)
                return full_tag.Substring(1, full_tag.IndexOf(' ') < full_tag.IndexOf('>') ? full_tag.IndexOf(' ') : full_tag.IndexOf('>')-1);
            else
                return full_tag.Substring(1,full_tag.IndexOf('>')-1);
        }
    }
}
