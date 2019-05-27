using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class RegexPatterns
    {
        public static Regex HTMLTag { get; set; } = new Regex(@"<[a-zA-Z]+ *[^>]*>");
        public static Regex HTMLTagClosed { get; set; } = new Regex(@"</[a-zA-Z]+>");
        public static Regex HTMLAttribute { get; set; } = new Regex(@" +[^=]+ *=");
        public static Regex HTMLAttributeValue { get; set; } = new Regex(@"""[^""]+""");
        public static Regex Doctype { get; set; } = new Regex(@"<!DOCTYPE[^>]*>");

        public static Regex HTMLTagValue(string tag)
        {
            Regex rgx = new Regex(@"<"+tag+" *[^>]*>" + @".*" + @"</"+tag+">");
            return rgx;
        }

    }
}
