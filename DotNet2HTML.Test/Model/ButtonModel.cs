using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Test.Model
{
    public class ButtonModel
    {

        public string Text { get; private set; }

        public ButtonModel(string text)
        {
            Text = text;
        }

    }
}
