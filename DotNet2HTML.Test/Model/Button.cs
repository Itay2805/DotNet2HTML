using DotNet2HTML.Tags;
using System;
using System.Collections.Generic;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Test.Model
{
    public class Button : Template<PageModel>
    {

        private ContainerTag template;

        public Button()
        {
            template =
                Div()
                .WithClass("button")
                .With(
                    Div()
                    .WithClass("button-text")
                    .With(
                        new ButtonText()
                    )
                );
        }

        public override void RenderTemplate(StringBuilder writer, PageModel model)
        {
            template.RenderModel(writer, model.ButtonModel.Text);
        }
    }

    class ButtonText : Template<string>
    {
        public override void RenderTemplate(StringBuilder writer, string model)
        {
            writer.Append(model);
        }
    }
}
