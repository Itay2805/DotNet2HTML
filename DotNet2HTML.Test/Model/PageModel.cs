using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Test.Model
{
    public class PageModel
    {

        public string Title { get; private set; }
        public string Text { get; private set; }
        public ButtonModel ButtonModel { get; private set; }

        public PageModel(string title, string text, ButtonModel buttonModel)
        {
            Title = title;
            Text = text;
            ButtonModel = buttonModel;
        }
    }
}
