using DotNet2HTML.Attributes;
using DotNet2HTML.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Test.Tags
{

    [TestClass]
    public class ConvenienceMethodTest
    {

        [TestMethod]
        public void TestAllConvenienceMethods()
        {
            Assert.AreEqual("<input autocomplete>", Input().IsAutoComplete().Render());
            Assert.AreEqual("<input autofocus>", Input().IsAutoFocus().Render());
            Assert.AreEqual("<input hidden>", Input().IsHidden().Render());
            Assert.AreEqual("<input required>", Input().IsRequired().Render());
            Assert.AreEqual("<img alt=\"An image\">", Img().WithAlt("An image").Render());
            Assert.AreEqual("<form action=\"post\"></form>", Form().WithAction("post").Render());
            Assert.AreEqual("<meta charset=\"UTF-8\">", Meta().WithCharset("UTF-8").Render());
            Assert.AreEqual("<div class=\"test-class\"></div>", Div().WithClass("test-class").Render());
            Assert.AreEqual("<meta content=\"Test Content\">", Meta().WithContent("Test Content").Render());
            Assert.AreEqual("<a href=\"http://example.com\"></a>", A().WithHref("http://example.com").Render());
            Assert.AreEqual("<div id=\"test-id\"></div>", Div().WithId("test-id").Render());
            Assert.AreEqual("<div data-testdata=\"test\"></div>", Div().WithData("testdata", "test").Render());
            Assert.AreEqual("<form method=\"get\"></form>", Form().WithMethod("get").Render());
            Assert.AreEqual("<input name=\"test-name\">", Input().WithName("test-name").Render());
            Assert.AreEqual("<input placeholder=\"test-placeholder\">", Input().WithPlaceholder("test-placeholder").Render());
            Assert.AreEqual("<a target=\"_blank\"></a>", A().WithTarget("_blank").Render());
            Assert.AreEqual("<a title=\"Title\"></a>", A().WithTitle("Title").Render());
            Assert.AreEqual("<input type=\"email\">", Input().WithType("email").Render());
            Assert.AreEqual("<link rel=\"stylesheet\">", Link().WithRel("stylesheet").Render());
            Assert.AreEqual("<link role=\"role\">", Link().WithRole("role").Render());
            Assert.AreEqual("<img src=\"/img/test.png\">", Img().WithSrc("/img/test.png").Render());
            Assert.AreEqual("<div style=\"background:red;\"></div>", Div().WithStyle("background:red;").Render());
            Assert.AreEqual("<input value=\"test-value\">", Input().WithValue("test-value").Render());
        }

    }
}
