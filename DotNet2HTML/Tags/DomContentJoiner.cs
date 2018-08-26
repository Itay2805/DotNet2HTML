using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet2HTML.Tags
{
    public static class DomContentJoiner
    {

        private static readonly Regex PERIOD_FIX = new Regex("\\s\\.");
        private static readonly Regex COMMA_FIX = new Regex("\\s\\,");

        public static UnescapedText Join(string delimiter, bool fixPeriodAndCommaSpacing, params object[] stringOrDomObjects)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in stringOrDomObjects)
            {
                if (o is string)
                {
                    sb.Append(((string)o).Trim()).Append(delimiter);
                }
                else if (o is DomContent)
                {
                    sb.Append(((DomContent)o).Render().Trim()).Append(delimiter);
                }
                else if (o == null)
                {
                    //Discard null objects so If/Ifelse can be used with join
                }
                else
                {
                    throw new InvalidOperationException("You can only join DomContent and string objects");
                }
            }
            string joined = sb.ToString().Trim();
            if (fixPeriodAndCommaSpacing)
            {
                joined = PERIOD_FIX.Replace(joined, ".");
                joined = COMMA_FIX.Replace(joined, ",");
            }
            return new UnescapedText(joined);
        }

    }
}
