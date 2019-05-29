using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCleaner
{
    sealed class Dictionaries
    {
        public int CurrentNumber { get; set; } = 0;
        private static Dictionaries _Instance = null;

        public static Dictionaries Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Dictionaries();
                }
                return _Instance;
            }
        }

        public Dictionary<string, bool> TagWithoutClosing { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> TagWithOptionalClosing { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> TagToRemove { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> AttributeToRemove { get; set; } = new Dictionary<string, bool>();
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

            TagToRemove.Add("img", false);
            TagToRemove.Add("frame", false);
            TagToRemove.Add("iframe", false);

            AttributeToRemove.Add("text-decoration", false);
            AttributeToRemove.Add("color", false);
            AttributeToRemove.Add("text-shadow", false);
            AttributeToRemove.Add("background-color", false);
            AttributeToRemove.Add("transition", false);
            AttributeToRemove.Add("transform", false);

        }
    }
}
