using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Tags
{
    public interface IRenderable
    {

        void RenderModel(StringBuilder writer, object model);

    }

    public static class IRenderableHelper
    {

        public static void Render(this IRenderable renderable, StringBuilder writer)
        {
            renderable.RenderModel(writer, null);
        }

        public static string Render(this IRenderable renderable)
        {
            StringBuilder builder = new StringBuilder();
            renderable.Render(builder);
            return builder.ToString();
        }

    }

}
