using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Settings.HTML_Content
{
    public class General
    {
        public const string REPLACE_ELEMENT_CLASS = "%elementClass%";
        public const string REPLACE_URL = "%url%";
        public const string REPLACE_TARGET = "%target%";

        public struct UL
        {
            private const string OPEN_TAG = "<ul class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</ul>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct LI
        {
            private const string OPEN_TAG = "<li class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</li>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct TR
        {
            private const string OPEN_TAG = "<tr class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</tr>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct THEAD
        {
            private const string OPEN_TAG = "<thead class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</thead>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct TABLE
        {
            private const string OPEN_TAG = "<table class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</table>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct TBODY
        {
            private const string OPEN_TAG = "<tbody class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</tbody>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct TH
        {
            private const string OPEN_TAG = "<th class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</th>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct TD
        {
            private const string OPEN_TAG = "<td class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</td>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct ANCHOR
        {
            private const string OPEN_TAG = "<a class=\"" + REPLACE_ELEMENT_CLASS + "\" href=\"" + REPLACE_URL + "\" target=\"" + REPLACE_TARGET + "\">";
            private const string CLOSE_TAG = "</a>";

            public static string GetOpenTag(string href = "", string @class = "", string target = "")
            {
                return OPEN_TAG
                    .Replace(REPLACE_ELEMENT_CLASS, @class)
                    .Replace(REPLACE_URL, href)
                    .Replace(REPLACE_TARGET, target);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct I_FONT_AWESOME
        {
            private const string OPEN_TAG = "<i class=\"" + REPLACE_ELEMENT_CLASS + "\"></i>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG
                    .Replace(REPLACE_ELEMENT_CLASS, @class);
            }
        }

        public struct SPAN
        {
            private const string OPEN_TAG = "<span class=\"" + REPLACE_ELEMENT_CLASS + "\">";
            private const string CLOSE_TAG = "</span>";

            public static string GetOpenTag(string @class = "")
            {
                return OPEN_TAG.Replace(REPLACE_ELEMENT_CLASS, @class);
            }

            public static string GetCloseTag()
            {
                return CLOSE_TAG;
            }
        }

        public struct CustomClasses
        {
            public const string NAV_CHILD_MENU = "nav child_menu";
        }

        public struct FontAwesomeCodes
        {
            public const string FONT_AWESOME_CODE_CHEVRON_DOWN = "fa fa-chevron-down";
        }
    }
}
