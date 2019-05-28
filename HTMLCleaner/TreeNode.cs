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
        public string ValueAfterStart { get; set; } = string.Empty;
        public bool Closed { get; set; } = false;
        public bool NoClosing { get; set; } = false;

        public TreeNode()
        {

        }

        public TreeNode(TreeNode parent, StreamReader reader,ref int current_match_opening,ref int current_match_closing, string current_line_arg)
        {
            
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
                                Match match = RegexPatterns.HTMLTagValue(this.Name).Match(current_line);
                                if (!string.IsNullOrEmpty(match.Value))
                                {
                                    this.Value = StringModifier.GetTagValue(match.Value);
                                }
                                current_match_closing++;
                                break;
                                //Logger.WriteLine(match.Value);
                            }
                            current_match_closing++;
                            
                        }
                        else
                        {
                            Children.Add(new TreeNode(this, reader, ref current_match_opening, ref current_match_closing, current_line));
                        }
                    }
                    else if (current_match_closing != 1000 && matches_for_closed.Count > 0 && current_match_closing != matches_for_closed.Count)
                    {
                        if (CloseTag(StringModifier.GetSimpleClosingTag(matches_for_closed[current_match_closing].Value), this) == 1)
                        {
                            Match match = RegexPatterns.HTMLTagValue(this.Name).Match(current_line);
                            if (!string.IsNullOrEmpty(match.Value))
                            {
                                this.Value = StringModifier.GetTagValue(match.Value);
                            }
                            //Logger.WriteLine(match.Value);
                        }
                        current_match_closing++;
                    }
                    else if (current_match_opening != 1000 && matches.Count > 0 && current_match_opening != matches.Count)
                    {

                        Children.Add(new TreeNode(this, reader, ref current_match_opening, ref current_match_closing, current_line));
                    }
                    else
                    {
                        Logger.WriteLine("");
                    }

                }
                Regex rgx = RegexPatterns.HTMLTagOpenedOrClosed;
                if(rgx.Matches(current_line).Count != 0)
                {
                    var match = rgx.Matches(current_line)[rgx.Matches(current_line).Count - 1];
                    tag_value += current_line.Substring(StringModifier.GetPositionOfTagEnd(match.Value,current_line,match.Index));
                }
                else
                {
                    tag_value += current_line;
                }
                if (this.Closed)
                    break;
                current_match_opening = 1000;
                current_match_closing = 1000;
                current_line = reader.ReadLine();

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

        string GetBeforeStartValue(string current_line)
        {

            return string.Empty;
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
