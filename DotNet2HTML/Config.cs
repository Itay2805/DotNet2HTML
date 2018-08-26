using System;
using System.Collections.Generic;
using System.Text;
using DotNet2HTML.Utils;

namespace DotNet2HTML
{
    public static class Config
    {

        public const string FOUR_SPACES = "    ";

        /// <summary>
        /// Change this to configure text-escaping
        /// For example, to disable escaping do <code>Config.TextEscaper = (text) => text;</code>
        /// </summary>
        public static TextEscaper TextEscaper { get; set; } = EscapeUtils.Escape;

        /// <summary>
        /// Change this to configure css-minification.
        /// There is no default minifier
        /// </summary>
        public static Minifier CssMinifier { get; set; } = (text) => text;

        /// <summary>
        /// Change this to configure js-minification.
        /// There is no default minifier
        /// </summary>
        public static Minifier JsMinifier { get; set; } = (text) => text;

        /// <summary>
        /// Change this to configure indentation when rendering formatted html
        /// The default is four spaces
        /// </summary>
        public static Identer Identer { get; set; } = (level, text) =>
        {
            StringBuilder sb = new StringBuilder(FOUR_SPACES.Length * level + text.Length);
            while (level-- != 0)
            {
                sb.Append(FOUR_SPACES);
            }
            sb.Append(text);
            return sb.ToString();
        };

        /// <summary>
        /// Change this to configure enable/disable closing empty tags
        /// The default is to NOT close them
        /// </summary>
        public static bool CloseEmptyTags { get; set; } = false;

    }
}
