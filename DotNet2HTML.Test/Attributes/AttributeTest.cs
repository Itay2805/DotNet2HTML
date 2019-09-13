using DotNet2HTML.Attributes;
using DotNet2HTML.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Test.Attributes
{

    [TestClass]
    public class AttributeTest
    {

        [TestMethod]
        public void TestRender()
        {
            HtmlAttribute attributeWithValue = new HtmlAttribute("href", "http://example.com");
            Assert.AreEqual(" href=\"http://example.com\"", attributeWithValue.Render());

            HtmlAttribute attribute = new HtmlAttribute("required", null);
            Assert.AreEqual(" required", attribute.Render());

            HtmlAttribute nullAttribute = new HtmlAttribute(null, null);
            Assert.AreEqual("", nullAttribute.Render());
        }

        [TestMethod]
        public void TestSetAttribute()
        {
            ContainerTag testTag = new ContainerTag("a");
            testTag.Attr("href", "http://example.com");
            testTag.Attr("href", "http://example.org");
            Assert.AreEqual("<a href=\"http://example.org\"></a>", testTag.Render());
        }

    }
}
