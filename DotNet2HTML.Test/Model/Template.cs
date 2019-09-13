using DotNet2HTML.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Test.Model
{
    public abstract class Template<T> : DomContent
    {
        public override void RenderModel(StringBuilder writer, object model)
        {
            RenderTemplate(writer, (T)model);
        }

        public abstract void RenderTemplate(StringBuilder writer, T model);
    }
}
