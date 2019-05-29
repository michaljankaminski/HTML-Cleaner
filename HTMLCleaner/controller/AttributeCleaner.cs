using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class AttributeCleaner
    {
        public string AttributeRemoval(string input)
        {
            if (input.IndexOf("<style") != -1)
                return input;
            var matches = RegexPatterns.StyleAttribute.Matches(input);
            if (matches.Count == 0)
                return input;
            else
            {
                foreach(Match match in matches)
                {
                    var style = match.Value.Substring(match.Value.IndexOf('"'));
                    style = style.Replace(@"""", string.Empty);
                    var temp = CleanAttributes(style);
                    temp = @" style=""" + temp + @"""";
                    input = input.Replace(match.Value, temp);
                }
                return input;
            }
        }

        string CleanAttributes(string style)
        {
            var temp = string.Empty;
            var attributes = style.Split(';');
            attributes = attributes.Where(value => !ShouldAttributeBeRemoved(value)).ToArray();
            foreach (string att in attributes)
            {
                if (att.IndexOf("font-size") != -1)
                    temp += ChangeFont(att) + ";";
                else
                    temp += att + ";";
            }
            return temp;
        }

        bool ShouldAttributeBeRemoved(string attribute)
        {
            if (attribute.IndexOf(':') == -1)
                return true;
            attribute = attribute.Substring(0, attribute.IndexOf(':'));
            if (Dictionaries.Instance.AttributeToRemove.ContainsKey(attribute.Trim()))
                return true;
            else
                return false;


        }

        string ChangeFont(string input)
        {
            var temp = input.Substring(0, input.IndexOf(':')+1);
            temp += " 14px";
            return temp;
        }

        public string CleanStyleTag(string input)
        {
            bool done = false;
            var final_string = string.Empty;
            var before = string.Empty;
            int starting = 0;
            int current_open = -1;
            int current_closed = -1;
            while (!done)
            {
                if (input.IndexOf('{',starting) != -1)
                {
                    current_open = input.IndexOf('{',starting);
                    current_closed = input.IndexOf('}',starting);
                    before = input.Substring(starting, current_open - starting);
                    var temp = input.Substring(current_open + 1,current_closed - current_open-1);
                    temp = CleanAttributes(temp);
                    final_string += before + "{"+ temp + "}";
                    starting = current_closed + 1;
                }
                else
                    done = true;

            }
            return final_string;

            
        }
    }
}
