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
        public List<TreeRoot> roots { get; set; } = null;
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

        public void GenerateOutput(TreeRoot root)
        {
            if (root == null)
                Logger.WriteLine("Plik nie został załadowany");
            else
            {
                OutputGenerator generator = new OutputGenerator();
                generator.Generate(root);
            }

        }
        public void CleanAttributes(TreeRoot root)
        {
            AttributeCleaner cleaner = new AttributeCleaner();
            TreeNode current = root.Child;
            CleanAttributesChild(current, cleaner);
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

        public void RemoveUnwantedTags(TreeRoot root)
        {
            TagCleaner cleaner = new TagCleaner();
            TreeNode current = root.Child;
            RemoveUnwantedTagChild(current, cleaner, root);
        }
        void RemoveUnwantedTagChild(TreeNode node, TagCleaner cleaner, TreeRoot root)
        {
            if (cleaner.TagRemover(node) != 1)
            {
                try
                {
                    foreach (TreeNode current in node.Children)
                        RemoveUnwantedTagChild(current, cleaner, root);
                }
                catch
                {
                    RemoveUnwantedTags(root);
                }

            }

        }

        public void CheckForNotClosedTags(TreeRoot root)
        {
            TreeNode current = root.Child;
            current.Closed = true;
            CheckForNotClosedTagsChild(current, root);                 
        }

        void CheckForNotClosedTagsChild(TreeNode node,TreeRoot root)
        {
            
            if (!node.Closed)
            {
                if (Dictionaries.Instance.TagWithOptionalClosing.ContainsKey(node.Name))
                    Logger.WriteLine("[" + Path.GetFileName(root.FilePath)+ "]" + "Warning! The "+ node.Name + " tag which starts at "+ node.LineNumber +" line should have closing");
                else
                {
                    throw new Exception("[" + Path.GetFileName(root.FilePath) + "]" + "Error! The " + node.Name + " tag which starts at " + node.LineNumber + " line does not have closing");
                }
                    
            }
            foreach (TreeNode current in node.Children)
                CheckForNotClosedTagsChild(current, root);
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
