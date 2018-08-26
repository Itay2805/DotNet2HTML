using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Tags
{
    public abstract class DomContent : IRenderable
    {
        public abstract void RenderModel(StringBuilder writer, object model);
    }
}
