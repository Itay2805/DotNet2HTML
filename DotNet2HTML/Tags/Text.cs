using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Tags
{
    public class Text : DomContent
    {

        private readonly string text;

        public Text(string text)
        {
            this.text = text;
        }

        public override void RenderModel(StringBuilder writer, object model)
        {
            writer.Append(Config.TextEscaper(text));
        }

    }
}
