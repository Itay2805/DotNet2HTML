using DotNet2HTML.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Test.Model
{
    public class DynamicHrefAttribute : HtmlAttribute
    {
        public DynamicHrefAttribute() 
            : base("href")
        {
        }

        public override void RenderModel(StringBuilder writer, object model)
        {
            writer.Append(" ");
            writer.Append(Name);
            writer.Append("=\"");
            writer.Append(GetUrl(model));
            writer.Append("\"");
        }

        public string GetUrl(object model)
        {
            return "/";
        }
    }
}
