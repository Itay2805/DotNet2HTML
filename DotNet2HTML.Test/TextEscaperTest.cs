using Microsoft.VisualStudio.TestTools.UnitTesting;

using DotNet2HTML;
using DotNet2HTML.Utils;

namespace DotNet2HTML.Test
{
    [TestClass]
    public class TextEscaperTest
    {
        [TestMethod]
        public void TestDefaultTextEscaper()
        {
            Assert.AreEqual("&lt;div&gt;&lt;/div&gt;", Config.TextEscaper("<div></div>"));
        }

        [TestMethod]
        public void TestUserProvidedTextEscaper()
        {
            Config.TextEscaper = (text) => text;
            Assert.AreEqual("<div></div>", Config.TextEscaper("<div></div>"));

            // reset escaper just in case
            Config.TextEscaper = EscapeUtils.Escape;
        }

    }
}
