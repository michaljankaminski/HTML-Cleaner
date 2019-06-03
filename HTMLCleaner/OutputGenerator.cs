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

        int current_level = -1;

        public OutputGenerator()
        {
            Generate = GenerateFile;

        }

        public void GenerateCss(string output)
        {
            using (StreamWriter writer = new StreamWriter("css_" + DateTime.Now.ToString("MM_dd_yyyy HH_mm") + ".css"))
            {
                writer.Write(output);
            }
        }


        public void GenerateFile(TreeRoot root)
        {
            File.Move(root.FilePath, root.FilePath + "_copy");
            current_level++;
            using (StreamWriter writer = new StreamWriter(root.FilePath))
            {
                writer.WriteLine(root.Doctype);
                this.SetDataForNodeFile(root.Child, writer);
            }
        }
        void SetDataForNodeFile(TreeNode node, StreamWriter writer)
        {
            string tablutators = "";
            for (int i = 0; i < current_level; i++)
            {
                tablutators += tablutators;
            }
            current_level++;
            if (node.NoClosing)
            {
                writer.WriteLine(tablutators + node.FullName);
            }
            else if (node.Children.Count == 0)
                writer.WriteLine(tablutators + node.FullName + node.Value + StringModifier.GetClosingTag(node.Name));
            else
            {
                writer.WriteLine(tablutators + node.FullName + node.Value);
                foreach (TreeNode current in node.Children)
                    SetDataForNodeFile(current, writer);
                writer.WriteLine(tablutators + StringModifier.GetClosingTag(node.Name));

            }
            current_level--;
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
            else if (node.Children.Count == 0)
                Logger.WriteLine(node.FullName + node.Value + StringModifier.GetClosingTag(node.Name));
            else
            {
                Logger.WriteLine(node.FullName + node.Value);
                foreach (TreeNode current in node.Children)
                    SetDataForNodeLogger(current);
                Logger.WriteLine(StringModifier.GetClosingTag(node.Name));
            }
        }

    }
}
