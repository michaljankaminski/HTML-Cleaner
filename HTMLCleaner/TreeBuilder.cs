using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class TreeBuilder
    {
        TreeNode current_node = null;
        public void BuildTree(StreamReader reader)
        {
            string current_line = String.Empty;
            while(!reader.EndOfStream)
            {
                var matches = RegexPatterns.HTMLTagOpenedOrClosed.Matches(current_line);
                int current_iter = 0;
                while (current_iter < matches.Count)
                {
                    if (matches[current_iter].Value.IndexOf('/') == 2)
                    {

                    }
                    else
                    {
                        var node = new TreeNode();
                        
                        current_node.Children.Add()
                    }
                }

            }
        }
    }
}
