using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class StringModifier
    {
        public static string GetSimpleTag(string full_tag)
        {
            if (full_tag.IndexOf(' ') != -1)
                return full_tag.Substring(1, full_tag.IndexOf(' ') < full_tag.IndexOf('>') ? full_tag.IndexOf(' ') - 1 : full_tag.IndexOf('>'));
            else
                return full_tag.Substring(1, full_tag.IndexOf('>') - 1);
        }

        public static string GetSimpleClosingTag(string full_tag)
        {
            return full_tag.Substring(2, full_tag.Length - 3);
        }

        public static string GetTagValue(string match)
        {
            return match.Substring(match.IndexOf('>') + 1, match.IndexOf('<', match.IndexOf('<') + 1) - match.IndexOf('>') - 1);
        }

        public static int GetPositionOfTagEnd(string match, string line, int startindex)
        {
            return line.IndexOf(match, startindex) + match.Length;
        }
    }
}
