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
        public bool NoClosing { get; set; } = false;
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
            Logger.WriteLine(Level + " " + Name + "   " + current_line_arg);
            Dictionaries dictionaries = new Dictionaries();
            if (dictionaries.TagWithoutClosing.ContainsKey(this.Name))
            {
                this.NoClosing = true;
                this.Closed = true;
                current_match++;
                return;
            }
                
            current_match++;
            string tag_value = string.Empty;
            while (!reader.EndOfStream)
            {
                regex = RegexPatterns.HTMLTag;
                matches = regex.Matches(current_line);
                if (current_match < matches.Count)
                {
                    tag_value = string.Empty;
                    for (int i = current_match; i < matches.Count; i++)
                    {
                        Children.Add(new TreeNode(this, Level, reader, i, current_line));
                    }
                }
                else
                {
                    if (matches.Count != 0)
                        tag_value = current_line.Substring(GetPositionOfTagEnd(matches[matches.Count - 1].Value,current_line));
                }

                regex = RegexPatterns.HTMLTagClosed;
                matches = regex.Matches(current_line);
                foreach(Match match in matches)
                {
                    if (CloseTag(GetSimpleClosingTag(match.Value),this)==1 && tag_value != string.Empty)
                    {
                        var temp_match = regex.Match(tag_value);
                        this.Value = tag_value.Substring(0, temp_match.Index);
                    }

                }
                if (this.Closed)
                    break;

                current_match = 0;
                current_line = reader.ReadLine();
                tag_value += current_line;
            }
        }

        void CheckValuesForTags(string line)
        {

        }

        int GetPositionOfTagEnd(string match, string line)
        {
             return line.IndexOf(match) + match.Length;
        }

        int CloseTag(string name, TreeNode current_arg)
        {
            TreeNode current = current_arg;
            bool current_closed = false;
            while (current != null)
            {
                if (current.Name == name)
                {
                    current.Closed = true;
                        if (!current_closed)
                            return 1;
                    return 2;
                }
                current = current.Parent;
                current_closed = true;
            }
            return -1;
        }
        private string GetSimpleTag(string full_tag)
        {
            if (full_tag.IndexOf(' ') != -1)
                return full_tag.Substring(1, full_tag.IndexOf(' ') < full_tag.IndexOf('>') ? full_tag.IndexOf(' ') -1: full_tag.IndexOf('>'));
            else
                return full_tag.Substring(1,full_tag.IndexOf('>')-1);
        }

        private string GetSimpleClosingTag(string full_tag)
        {
            return full_tag.Substring(2,full_tag.Length - 3);
        }


    }
}
