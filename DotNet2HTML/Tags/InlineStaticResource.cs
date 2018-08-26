using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Tags
{
    public static class InlineStaticResource
    {

        public enum TargetFormat
        {
            CSS_MIN,
            CSS,
            JS_MIN,
            JS
        }

        public static ContainerTag Get(string path, TargetFormat format)
        {
            // support finding files inside the assembly

            string file = File.ReadAllText(path);
            switch (format)
            {
                case TargetFormat.CSS_MIN:
                   return Style().With(RawHtml(Config.CssMinifier(file)));
                case TargetFormat.JS_MIN:
                    return Script().With(RawHtml(Config.CssMinifier(file)));
                case TargetFormat.CSS:
                    return Style().With(RawHtml(file));
                case TargetFormat.JS:
                    return Script().With(RawHtml(file));
                default:
                    throw new Exception("Invalid target format");
            }
        }

    }
}
