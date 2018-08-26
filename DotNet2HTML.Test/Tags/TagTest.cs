using DotNet2HTML.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Test.Tags
{

    [TestClass]
    public class TagTest
    {

        [TestMethod]
        public void TestTagRender()
        {
            ContainerTag testTag = new ContainerTag("a");
            testTag.Attr("href", "http://example.com");
            Assert.AreEqual("<a href=\"http://example.com\"></a>", testTag.Render());
        }

        [TestMethod]
        public void TestComplexTagRender()
        {
            ContainerTag complexTestTag = Html().With(Body().With(Header(), Main().With(P("Main stuff...")), Footer().CondWith(1 == 1, P("Conditional with!"))));
            string expectedResult = "<html><body><header></header><main><p>Main stuff...</p></main><footer><p>Conditional with!</p></footer></body></html>";
            Assert.AreEqual(expectedResult, complexTestTag.Render());
        }

    }
}
