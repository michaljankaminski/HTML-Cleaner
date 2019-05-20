using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class Parser
    {
        public void ReadFiles(List<string> files)
        {
            foreach (string file in files)
                this.BuildTree(file);
        }

        void BuildTree(string file)
        {
            TreeRoot root = new TreeRoot();
            root.BuildTree(file);
            
        }
        void DrawTree(TreeRoot root)
        {
            TreeNode current = root.Child;
            Console.WriteLine(root.Doctype);
            while (current != null)
            {
                Console.WriteLine(current.FullName);
                foreach (TreeNode node in current.Children)
                    current = node;
            }
        }
    }
}
