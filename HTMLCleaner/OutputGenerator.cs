using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class OutputGenerator
    {
        public delegate void Generator(TreeRoot root);

        public Generator Generate;

        public OutputGenerator()
        {
            Generate = GenerateFile;

        }
        public void GenerateInLogger(TreeRoot root)
        {
            Logger.WriteLine(root.Doctype);
            this.SetDataForNodeLogger(root.Child);           
        }
        void SetDataForNodeLogger(TreeNode node)
        {
            if (node.NoClosing)
            {
                Logger.WriteLine(node.FullName);
            }
            else if (node.Children.Count==0)
                Logger.WriteLine(node.FullName + node.Value + this.GetClosingTag(node.Name));
            else
            {
                Logger.WriteLine(node.FullName + node.Value);
                foreach (TreeNode current in node.Children)
                    SetDataForNodeLogger(current);
                Logger.WriteLine(this.GetClosingTag(node.Name));
            }
                

        }

        public void GenerateFile(TreeRoot root)
        {
            using (StreamWriter writer = new StreamWriter("index_" + DateTime.Now.ToString("MM_dd_yyyy HH_mm") + ".html"))
            {
                writer.WriteLine(root.Doctype);
                this.SetDataForNodeFile(root.Child, writer);
            }
        }
        void SetDataForNodeFile(TreeNode node, StreamWriter writer)
        {
            if (node.NoClosing)
            {
                writer.WriteLine(node.FullName);
            }
            else if (node.Children.Count == 0)
                writer.WriteLine(node.FullName + node.Value + this.GetClosingTag(node.Name));
            else
            {
                writer.WriteLine(node.FullName + node.Value);
                foreach (TreeNode current in node.Children)
                    SetDataForNodeFile(current, writer);
                writer.WriteLine(this.GetClosingTag(node.Name));

            }
        }

        string GetClosingTag(string input)
        {
            return "</" + input + ">";
        }
    }
}
