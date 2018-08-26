using DotNet2HTML.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Tags
{
    public class EmptyTag : Tag<EmptyTag>
    {

        public EmptyTag(string tagName)
            : base(tagName)
        {

        }

        public override void RenderModel(StringBuilder writer, object model)
        {
            writer.Append("<").Append(TagName);
            foreach (HtmlAttribute attribute in Attributes)
            {
                attribute.RenderModel(writer, model);
            }
            if (Config.CloseEmptyTags)
            {
                writer.Append("/");
            }
            writer.Append(">");
        }

    }
}
