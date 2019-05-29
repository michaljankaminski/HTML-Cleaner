using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class TagCleaner
    {
        
        public int TagRemover(TreeNode current)
        {
            if (Dictionaries.Instance.TagToRemove.ContainsKey(current.Name))
            {
                TreeNode parent = current.Parent;
                for (int i = 0; i < parent.Children.Count; i++)
                {
                    if (parent.Children[i] == current)
                    {
                        parent.Children.RemoveAt(i);
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
