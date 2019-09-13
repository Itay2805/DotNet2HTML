using DotNet2HTML.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Attributes
{
    public class HtmlAttribute : IRenderable
    {

        public string Name { get; }
        public object Value { get; set; }

        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = Config.TextEscaper(value);
        }

        public HtmlAttribute(string name)
        {
            Name = name;
            Value = null;
        }

        public virtual void RenderModel(StringBuilder writer, object model)
        {
            if(Name == null)
            {
                return;
            }

            writer.Append(" ");
            writer.Append(Name);
            if(Value != null)
            {
                writer.Append("=\"");
                writer.Append(Value.ToString());
                writer.Append("\"");
            }

        }
    }
}
