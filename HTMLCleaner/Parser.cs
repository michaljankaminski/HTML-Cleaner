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
        TreeRoot root { get; set; } = null;
        public void ReadFiles(List<string> files)
        {
            foreach (string file in files)
                this.BuildTree(file);
        }

        void BuildTree(string file)
        {
            root = new TreeRoot();
            root.BuildTree(file);        
        }

        public void GenerateOutput()
        {
            if (root == null)
                Logger.WriteLine("Plik nie został załadowany");
            else
            {
                OutputGenerator generator = new OutputGenerator();
                generator.Generate(root);
            }
        }

        public void DrawTree()
        {

            TreeNode current = root.Child;
            Logger.WriteLine(root.Doctype);
            DrawChild(current);
        }

        void DrawChild(TreeNode node)
        {
            Logger.WriteLine(node.FullName);
            foreach (TreeNode current in node.Children)
                DrawChild(current);
        }

        
    }
}
