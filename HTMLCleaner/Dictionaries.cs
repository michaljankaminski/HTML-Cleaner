using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    class Dictionaries
    {
        public Dictionary<string, bool> TagWithoutClosing { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> TagWithOptionalClosing { get; set; } = new Dictionary<string, bool>();
        public Dictionaries()
        {//span, b, u, em, strong, h1, h2, h3, h4, h5, h6
            //html, head, body, p, dt, dd, li, option, thead, th, tbody, tr, td, tfoot, colgroup
            TagWithOptionalClosing.Add("html", false);
            TagWithOptionalClosing.Add("head", false);
            TagWithOptionalClosing.Add("body", false);
            TagWithOptionalClosing.Add("p", false);
            TagWithOptionalClosing.Add("dt", false);
            TagWithOptionalClosing.Add("dd", false);
            TagWithOptionalClosing.Add("li", false);
            TagWithOptionalClosing.Add("option", false);
            TagWithOptionalClosing.Add("thead", false);
            TagWithOptionalClosing.Add("th", false);
            TagWithOptionalClosing.Add("tbody", false);
            TagWithOptionalClosing.Add("tr", false);
            TagWithOptionalClosing.Add("td", false);
            TagWithOptionalClosing.Add("tfoot", false);
            TagWithOptionalClosing.Add("colgroup", false);
            //img, input, br, hr, meta, link
            TagWithoutClosing.Add("img", false);
            TagWithoutClosing.Add("input", false);
            TagWithoutClosing.Add("br", false);
            TagWithoutClosing.Add("hr", false);
            TagWithoutClosing.Add("meta", false);
            TagWithoutClosing.Add("link", false);


        }
    }
}
