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
        public int LineNumber { get; set; } = 0;

        public TreeNode(TreeNode parent, StreamReader reader,ref int current_match_opening,ref int current_match_closing, string current_line_arg)
        {
            
            if (parent != null)
                this.Parent = parent;
            var current_line = current_line_arg;
            this.LineNumber = Dictionaries.Instance.CurrentNumber;
            var regex = RegexPatterns.HTMLTag;
            var matches = regex.Matches(current_line);

            var regex_closed = RegexPatterns.HTMLTagClosed;
            var matches_for_closed = regex_closed.Matches(current_line);

            this.FullName = matches[current_match_opening].Value;
            this.Name = StringModifier.GetSimpleTag(this.FullName);

            current_match_opening++;
            if (this.Name == "style")
            {
                StyleValue(reader, current_line_arg);
                this.Closed = true;
                return;
            }

            if (ShouldTagBeClosed(this.Name))
                return;
                
            string tag_value = string.Empty;

            while (!reader.EndOfStream)
            {
                while (current_match_opening < matches.Count || current_match_closing < matches_for_closed.Count)
                {
                    if (current_match_opening != 0x7FFFFFFF && current_match_closing != 0x7FFFFFFF && matches.Count > 0 && matches_for_closed.Count > 0 && current_match_opening != matches.Count && current_match_closing != matches_for_closed.Count)
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
                            }
                            current_match_closing++;
                            
                        }
                        else
                        {
                            Children.Add(new TreeNode(this, reader, ref current_match_opening, ref current_match_closing, current_line));
                        }
                    }
                    else if (current_match_closing != 0x7FFFFFFF && matches_for_closed.Count > 0 && current_match_closing != matches_for_closed.Count)
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
                    else if (current_match_opening != 0x7FFFFFFF && matches.Count > 0 && current_match_opening != matches.Count)
                    {

                        Children.Add(new TreeNode(this, reader, ref current_match_opening, ref current_match_closing, current_line));
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
                current_match_opening = 0x7FFFFFFF;
                current_match_closing = 0x7FFFFFFF;
                current_line = reader.ReadLine();
                Dictionaries.Instance.CurrentNumber++;

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
        void StyleValue(StreamReader reader, string current_line)
        {
            
            if (current_line.IndexOf("</style>") != -1)
                this.Value = StringModifier.GetTagValue(current_line);
            else
            {
                var value = string.Empty;
                value += current_line.Substring(StringModifier.GetPositionOfTagEnd("<style>", current_line, 0));
                while (!reader.EndOfStream)
                {
                    current_line = reader.ReadLine();
                    Dictionaries.Instance.CurrentNumber++;
                    if (current_line.IndexOf("</style>") == -1)
                        this.Value += current_line;
                    else
                    {
                        this.Value += current_line.Substring(0, current_line.IndexOf("</style>"));
                        break;
                    }

                }

            }
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
            if (Dictionaries.Instance.TagWithoutClosing.ContainsKey(name))
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
