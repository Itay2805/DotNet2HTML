using DotNet2HTML.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Test.Tags
{

    [TestClass]
    public class RenderFormattedTest
    {

        [TestMethod]
        public void TestFormattedTags()
        {
            Assert.AreEqual(Div(P("Hello")).RenderFormatted(), "<div>\n    <p>\n        Hello\n    </p>\n</div>\n");
        }

        [TestMethod]
        public void TestFormattedTags_DoesntFormatPre()
        {
            Assert.AreEqual(Div(Pre("public void renderModel(Appendable writer, Object model) throws IOException {\n" +
                "        writer.append(text);\n" +
                "    }")).RenderFormatted(), "<div>\n" +
                "    <pre>public void renderModel(Appendable writer, Object model) throws IOException {\n" +
                "        writer.append(text);\n" +
                "    }</pre>\n" +
                "</div>\n");
        }

        [TestMethod]
        public void TestFormattedTags_DoesntFormatTextArea()
        {
            Assert.AreEqual(Div(TextArea("fred\ntom")).RenderFormatted(), "<div>\n" +
                "    <textarea>fred\n" +
                "tom</textarea>\n" +
                "</div>\n");
        }

        [TestMethod]
        public void TestFormattedTags_Each()
        {
            Assert.AreEqual(Ul(Each(new int[] { 1, 2, 3 }, i => Li("Number " + i))).RenderFormatted(),
                "<ul>\n" +
                    "    <li>\n" +
                    "        Number 1\n" +
                    "    </li>\n" +
                    "    <li>\n" +
                    "        Number 2\n" +
                    "    </li>\n" +
                    "    <li>\n" +
                    "        Number 3\n" +
                    "    </li>\n" +
                    "</ul>\n"
            );
        }

    }
}
