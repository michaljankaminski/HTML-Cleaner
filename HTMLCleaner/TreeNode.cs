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
        public TreeNode(TreeNode parent, int level, StreamReader reader,ref int current_match_opening,ref int current_match_closing, string current_line_arg)
        {
            
            this.Level = ++level;
            if (parent != null)
                this.Parent = parent;
            var current_line = current_line_arg;
            var regex = RegexPatterns.HTMLTag;
            var matches = regex.Matches(current_line);

            var regex_closed = RegexPatterns.HTMLTagClosed;
            var matches_for_closed = regex_closed.Matches(current_line);

            this.FullName = matches[current_match_opening].Value;
            this.Name = StringModifier.GetSimpleTag(this.FullName);
            //Logger.WriteLine(Level + " " + Name + "   " + current_line_arg);
            current_match_opening++;

            if (ShouldTagBeClosed(this.Name))
                return;
                
            string tag_value = string.Empty;

            while (!reader.EndOfStream)
            {
                //Logger.WriteLine(this.Name + "     ");
                //Logger.WriteLine(current_line);

                while (current_match_opening < matches.Count || current_match_closing < matches_for_closed.Count)
                {
                    if (current_match_opening != 1000 && current_match_closing != 1000 && matches.Count > 0 && matches_for_closed.Count > 0 && current_match_opening != matches.Count && current_match_closing != matches_for_closed.Count)
                    {
                        if ((matches_for_closed[current_match_closing].Index < matches[current_match_opening].Index) )
                        {
                            if (CloseTag(StringModifier.GetSimpleClosingTag(matches_for_closed[current_match_closing].Value), this) == 1)
                            {
                                var rgx = RegexPatterns.HTMLTagValue(this.Name);
                                Match match = rgx.Match(current_line);
                                Logger.WriteLine(match.Value);
                            }
                            current_match_closing++;
                            
                        }
                        else
                        {
                            Children.Add(new TreeNode(this, Level, reader, ref current_match_opening, ref current_match_closing, current_line));
                        }
                    }
                    else if (current_match_closing != 1000 && matches_for_closed.Count > 0 && current_match_closing != matches_for_closed.Count)
                    {
                        if (CloseTag(StringModifier.GetSimpleClosingTag(matches_for_closed[current_match_closing].Value), this) == 1)
                        {
                            var rgx = RegexPatterns.HTMLTagValue(this.Name);
                            Match match = rgx.Match(current_line);
                            if (string.IsNullOrEmpty(match.Value))
                            {
                                this.Name = GetTagValue(match.Value);
                            }
                            Logger.WriteLine(match.Value);
                        }
                        current_match_closing++;
                    }
                    else if (current_match_opening != 1000 && matches.Count > 0 && current_match_opening != matches.Count)
                    {
                        Children.Add(new TreeNode(this, Level, reader, ref current_match_opening, ref current_match_closing, current_line));
                    }
                    else
                    {
                        Logger.WriteLine("");
                    }

                }

                if (this.Closed)
                    break;
                current_match_opening = 1000;
                current_match_closing = 1000;
                current_line = reader.ReadLine();
                tag_value += current_line;

                if (!string.IsNullOrEmpty(current_line))
                {
                    matches = regex.Matches(current_line);
                    matches_for_closed = regex_closed.Matches(current_line);
                    if (matches.Count != 0)
                        current_match_opening = 0;
                    if (matches_for_closed.Count != 0)
                        current_match_closing = 0;

                }

            }
        }

        string GetTagValue(string match)
        {
            string temp = match.Substring(match.IndexOf('>') + 1, match.IndexOf('<',match.IndexOf('<')+1)- match.IndexOf('>'));
            return temp;
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

        private bool ShouldTagBeClosed(string name)
        {
            Dictionaries dictionaries = new Dictionaries();
            if (dictionaries.TagWithoutClosing.ContainsKey(name))
            {
                this.NoClosing = true;
                this.Closed = true;
                return true;
            }
            else
                return false;
        }

    }
}
