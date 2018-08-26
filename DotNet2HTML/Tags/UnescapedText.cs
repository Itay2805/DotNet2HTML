using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Tags
{
    public class UnescapedText : DomContent
    {

        private readonly string text;

        public UnescapedText(string text)
        {
            this.text = text;
        }

        public override void RenderModel(StringBuilder writer, object model)
        {
            writer.Append(text);
        }
    }
}
