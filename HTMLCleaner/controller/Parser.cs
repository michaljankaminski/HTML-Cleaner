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
        List<TreeRoot> roots { get; set; } = null;
        public void ReadFiles(List<string> files)
        {
            roots = new List<TreeRoot>();
            foreach (string file in files)
                this.BuildTree(file);
        }

        void BuildTree(string file)
        {
            var root = new TreeRoot();
            root.BuildTree(file);
            roots.Add(root);

        }

        public void GenerateOutput()
        {
            foreach(TreeRoot root in roots)
            {
                if (root == null)
                    Logger.WriteLine("Plik nie został załadowany");
                else
                {
                    OutputGenerator generator = new OutputGenerator();
                    generator.Generate(root);
                }
            }

        }
        public void CleanAttributes()
        {
            AttributeCleaner cleaner = new AttributeCleaner();
            foreach ( TreeRoot root in roots)
            {
                TreeNode current = root.Child;
                CleanAttributesChild(current, cleaner);
            }

        }
        void CleanAttributesChild(TreeNode node,AttributeCleaner cleaner)
        {
            if (node.Name == "style")
                CleanStyleTag(node, cleaner);
            node.FullName = cleaner.AttributeRemoval(node.FullName);
            foreach (TreeNode current in node.Children)
                CleanAttributesChild(current, cleaner);
        }

        void CleanStyleTag(TreeNode node, AttributeCleaner cleaner)
        {
            node.Value = cleaner.CleanStyleTag(node.Value);
        }

        public void RemoveUnwantedTags()
        {
            TagCleaner cleaner = new TagCleaner();
            foreach (TreeRoot root in roots)
            {
                TreeNode current = root.Child;
                RemoveUnwantedTagChild(current, cleaner);
            }
        }
        void RemoveUnwantedTagChild(TreeNode node, TagCleaner cleaner)
        {
            if (cleaner.TagRemover(node) != 1)
            {
                try
                {
                    foreach (TreeNode current in node.Children)
                        RemoveUnwantedTagChild(current, cleaner);
                }
                catch
                {
                    RemoveUnwantedTags();
                }

            }

        }

        public void CheckForNotClosedTags()
        {
            foreach (TreeRoot root in roots)
            {
                TreeNode current = root.Child;
                current.Closed = true;
                CheckForNotClosedTagsChild(current);                 
            }
        }

        void CheckForNotClosedTagsChild(TreeNode node)
        {
            
            if (!node.Closed)
            {
                if (Dictionaries.Instance.TagWithOptionalClosing.ContainsKey(node.Name))
                    Logger.WriteLine("Warning! The "+ node.Name + " tag which starts at "+ node.LineNumber +" line should have closing");
                else
                {
                    throw new Exception("Error! The " + node.Name + " tag which starts at " + node.LineNumber + " line does not have closing");
                }
                    
            }
            foreach (TreeNode current in node.Children)
                CheckForNotClosedTagsChild(current);
        }

        public void ParseCss(string file)
        {
            using(StreamReader reader = new StreamReader(file))
            {
                var css = reader.ReadToEnd();
                AttributeCleaner cleaner = new AttributeCleaner();
                OutputGenerator generator = new OutputGenerator();
                generator.GenerateCss(cleaner.CleanStyleTag(css));
            }
        }
    }
}
