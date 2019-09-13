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
    public class ComplexRenderTest
    {

        public string RenderTest()
        {
            bool USER_SHOULD_LOG_IN = true;
            bool USER_SHOULD_SIGN_UP = false;
            return Document().Render() +
                Html().With(
                    Head().With(
                        Title().WithText("Test")
                    ),
                    Body().With(
                        Header().With(
                            H1().With(
                                Text("Test Header "),
                                A("with link").WithHref("http://example.com"),
                                Text(".")
                            )
                        ),
                        Main().With(
                            H2("Test Form"),
                            Div().With(
                                Input().WithType("email").WithName("email").WithPlaceholder("Email"),
                                Input().WithType("password").WithName("password").WithPlaceholder("Password")
                            ).CondWith(USER_SHOULD_LOG_IN, Button().WithType("submit").WithText("Login")
                            ).CondWith(USER_SHOULD_SIGN_UP, Button().WithType("submit").WithText("Signup"))
                        ),
                        Footer().Attr(Attr.CLASS, "footer").CondAttr(1 == 1, Attr.ID, "id").WithText("Test Footer"),
                        Script().WithSrc("/testScript.js")
                    )
                ).Render();
        }

        public string RenderTest2()
        {
            bool USER_SHOULD_LOG_IN = true;
            bool USER_SHOULD_SIGN_UP = false;
            return Document().Render() +
                Html(
                    Head(
                        Title("Test")
                    ),
                    Body(
                        Header(
                            H1(
                                Text("Test Header "),
                                A("with link").WithHref("http://example.com"),
                                Text(".")
                            )
                        ),
                        Main(
                            H2("Test Form"),
                            Div(
                                Input().WithType("email").WithName("email").WithPlaceholder("Email"),
                                Input().WithType("password").WithName("password").WithPlaceholder("Password"),
                                If(USER_SHOULD_LOG_IN, Button().WithType("submit").WithText("Login")),
                                If(USER_SHOULD_SIGN_UP, Button().WithType("submit").WithText("Signup"))
                            )
                        ),
                        Footer("Test Footer").Attr(Attr.CLASS, "footer").CondAttr(1 == 1, Attr.ID, "id"),
                        Script().WithSrc("/testScript.js")
                    )
                ).Render();
        }

        private string RenderTest3()
        {
            bool USER_SHOULD_LOG_IN = true;
            bool USER_SHOULD_SIGN_UP = false;
            return Document().Render() + "\n" +
                Html(
                    Head(
                        Title("Test")
                    ),
                    Body(
                        Header(
                            H1(
                                Text("Test Header "),
                                A("with link").WithHref("http://example.com"),
                                Text(".")
                            )
                        ),
                        Main(
                            H2("Test Form"),
                            Div(
                                Input().WithType("email").WithName("email").WithPlaceholder("Email"),
                                Input().WithType("password").WithName("password").WithPlaceholder("Password"),
                                If(USER_SHOULD_LOG_IN, Button().WithType("submit").WithText("Login")),
                                If(USER_SHOULD_SIGN_UP, Button().WithType("submit").WithText("Signup"))
                            )
                        ),
                        Footer("Test Footer").Attr(Attr.CLASS, "footer").CondAttr(1 == 1, Attr.ID, "id"),
                        Script().WithSrc("/testScript.js")
                    )
                ).RenderFormatted();
        }

        [TestMethod]
        public void TestComplexRender()
        {
            string expectedResult = "<!DOCTYPE html><html><head><title>Test</title></head><body><header><h1>Test Header <a href=\"http://example.com\">with link</a>.</h1></header><main><h2>Test Form</h2><div><input type=\"email\" name=\"email\" placeholder=\"Email\"><input type=\"password\" name=\"password\" placeholder=\"Password\"><button type=\"submit\">Login</button></div></main><footer class=\"footer\" id=\"id\">Test Footer</footer><script src=\"/testScript.js\"></script></body></html>";
            Assert.AreEqual(expectedResult, RenderTest());
            Assert.AreEqual(expectedResult, RenderTest2());
        }

        [TestMethod]
        public void TestComplexRender_Formatted()
        {
            Assert.AreEqual(
                    "<!DOCTYPE html>\n"
                    + "<html>\n"
                    + "    <head>\n"
                    + "        <title>\n"
                    + "            Test\n"
                    + "        </title>\n"
                    + "    </head>\n"
                    + "    <body>\n"
                    + "        <header>\n"
                    + "            <h1>\n"
                    + "                Test Header \n"
                    + "                <a href=\"http://example.com\">\n"
                    + "                    with link\n"
                    + "                </a>\n"
                    + "                .\n"
                    + "            </h1>\n"
                    + "        </header>\n"
                    + "        <main>\n"
                    + "            <h2>\n"
                    + "                Test Form\n"
                    + "            </h2>\n"
                    + "            <div>\n"
                    + "                <input type=\"email\" name=\"email\" placeholder=\"Email\">\n"
                    + "                <input type=\"password\" name=\"password\" placeholder=\"Password\">\n"
                    + "                <button type=\"submit\">\n"
                    + "                    Login\n"
                    + "                </button>\n"
                    + "            </div>\n"
                    + "        </main>\n"
                    + "        <footer class=\"footer\" id=\"id\">\n"
                    + "            Test Footer\n"
                    + "        </footer>\n"
                    + "        <script src=\"/testScript.js\">\n"
                    + "        </script>\n"
                    + "    </body>\n"
                    + "</html>\n",
                    RenderTest3());
        }

    }
}
