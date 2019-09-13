using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Test.Model
{
    class BrowserTitle : Template<PageModel>
    {
        public override void RenderTemplate(StringBuilder writer, PageModel model)
        {
            writer.Append(model.Title);
        }
    }
}
