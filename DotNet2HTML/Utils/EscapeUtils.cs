using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Utils
{
    public static class EscapeUtils
    {

        public static string Escape(string s)
        {
            if (s == null)
            {
                return null;
            }
            StringBuilder escapedText = new StringBuilder();
            char currentChar;
            for (int i = 0; i < s.Length; i++)
            {
                currentChar = s[i];
                switch (currentChar)
                {
                    case '<':
                        escapedText.Append("&lt;");
                        break;
                    case '>':
                        escapedText.Append("&gt;");
                        break;
                    case '&':
                        escapedText.Append("&amp;");
                        break;
                    case '"':
                        escapedText.Append("&quot;");
                        break;
                    case '\'':
                        escapedText.Append("&#x27;");
                        break;
                    default:
                        escapedText.Append(currentChar);
                        break;
                }
            }
            return escapedText.ToString();
        }

    }
}
