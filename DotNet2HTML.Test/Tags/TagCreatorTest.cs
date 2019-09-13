using DotNet2HTML.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static DotNet2HTML.TagCreator;

namespace DotNet2HTML.Test.Tags
{

    [TestClass]
    public class TagCreatorTest
    {

        private List<Employee> employees = new List<Employee> {
            new Employee { Id = 1, Name = "Name 1", Title = "Title 1" },
            new Employee { Id = 2, Name = "Name 2", Title = "Title 2" },
            new Employee { Id = 3, Name = "Name 3", Title = "Title 3" },
        };

        private Dictionary<int, Employee> employeeMap = new Dictionary<int, Employee> {
            { 1, new Employee { Id = 1, Name = "Name 1", Title = "Title 1" } },
            { 2, new Employee { Id = 2, Name = "Name 2", Title = "Title 2" } },
            { 3, new Employee { Id = 3, Name = "Name 3", Title = "Title 3" } },
        };

        [TestMethod]
        public void TestDocument()
        {
            Config.CloseEmptyTags = true;
            Assert.AreEqual("<!DOCTYPE html>", Document().Render());
            Assert.AreEqual("<!DOCTYPE html><html></html>", Document(Html()));
            Config.CloseEmptyTags = false;
            Assert.AreEqual("<!DOCTYPE html>", Document().Render());
            Assert.AreEqual("<!DOCTYPE html><html></html>", Document(Html()));
        }

        [TestMethod]
        public void TestIf()
        {
            string expected = "<div><p>Test</p><a href=\"#\">Test</a></div>";
            string actual = Div(
                    P("Test"),
                    If(1 == 1, A("Test").WithHref("#")),
                    If(1 == 2, A("Test").WithHref("#"))
                ).Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIfElse()
        {
            string expected = "<div><p>Tast</p></div>";
            string actual = Div(IfElse(1 == 2, P("Test"), P("Tast"))).Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJoin()
        {
            string expected = "This is my joined string. It has a <a href=\"#\">link in the middle</a> and <code>code at the end</code>.";
            string actual = Join("This is my joined string. It has a", A("link in the middle").WithHref("#"), "and", Code("code at the end"), ".").Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJoinWithNulls()
        {
            string expected = "This is my joined string. It has ignored null content in the middle.";
            string actual = Join("This is my joined string.", If(false, "this should not be displayed"), "It has ignored null content in the middle.").Render();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEach()
        {
            string dotnet2htmlMap = Ul().With(
                Li("Begin list"),
                Each(employees, employee => Li(
                    H2(employee.Name),
                    P(employee.Title)
                ))
            ).Render();
            Assert.AreEqual("<ul><li>Begin list</li><li><h2>Name 1</h2><p>Title 1</p></li><li><h2>Name 2</h2><p>Title 2</p></li><li><h2>Name 3</h2><p>Title 3</p></li></ul>", dotnet2htmlMap);
        }

        [TestMethod]
        public void TestEachWithMap()
        {
            string dotnet2htmlMap = Ul().With(
                Li("Begin list"),
                Each(employeeMap, entry => Li(entry.Key + "-" + entry.Value.Name))
            ).Render();

            Assert.AreEqual("<ul><li>Begin list</li><li>1-Name 1</li><li>2-Name 2</li><li>3-Name 3</li></ul>", dotnet2htmlMap);
        }

        [TestMethod]
        public void TestEachWithMapAndBiFunction()
        {
           string dotnet2htmlMap = Ul().With(
               Li("Begin list"),
               Each(employeeMap, (id, employee) => Li(id + "-" + employee.Name))
           ).Render();

           Assert.AreEqual("<ul><li>Begin list</li><li>1-Name 1</li><li>2-Name 2</li><li>3-Name 3</li></ul>", dotnet2htmlMap);
        }

        [TestMethod]
        public void TestFilter()
        {
            string dotnet2htmlMap = Ul().With(
                Li("Begin list"),
                Each(Filter(employees, e => e.Id % 2 == 1), employee => Li(
                    H2(employee.Name),
                    P(employee.Title)
                ))
            ).Render();

            Assert.AreEqual("<ul><li>Begin list</li><li><h2>Name 1</h2><p>Title 1</p></li><li><h2>Name 3</h2><p>Title 3</p></li></ul>", dotnet2htmlMap);
        }

        [TestMethod]
        public void TestAllTags()
        {
            //Special Tags
            Assert.AreEqual(Tag("tagname").Render(), "<tagname></tagname>");
            Assert.AreEqual(EmptyTag("tagname").Render(), "<tagname>");
            Assert.AreEqual(Text("text").Render(), "text");
            Assert.AreEqual(Text("<script> and \"</script>\"").Render(), "&lt;script&gt; and &quot;&lt;/script&gt;&quot;");
            Assert.AreEqual(RawHtml("<script>").Render(), "<script>");

            //EmptyTags
            Assert.AreEqual(Document().Render(), "<!DOCTYPE html>");
            Assert.AreEqual(Area().Render(), "<area>");
            Assert.AreEqual(Base().Render(), "<base>");
            Assert.AreEqual(Br().Render(), "<br>");
            Assert.AreEqual(Col().Render(), "<col>");
            Assert.AreEqual(Embed().Render(), "<embed>");
            Assert.AreEqual(Hr().Render(), "<hr>");
            Assert.AreEqual(Img().Render(), "<img>");
            Assert.AreEqual(Input().Render(), "<input>");
            Assert.AreEqual(Keygen().Render(), "<keygen>");
            Assert.AreEqual(Link().Render(), "<link>");
            Assert.AreEqual(Meta().Render(), "<meta>");
            Assert.AreEqual(Param().Render(), "<param>");
            Assert.AreEqual(Source().Render(), "<source>");
            Assert.AreEqual(Track().Render(), "<track>");
            Assert.AreEqual(Wbr().Render(), "<wbr>");

            //ContainerTags
            Assert.AreEqual(A().Render(), "<a></a>");
            Assert.AreEqual(A("Text").Render(), "<a>Text</a>");
            Assert.AreEqual(Abbr().Render(), "<abbr></abbr>");
            Assert.AreEqual(Address().Render(), "<address></address>");
            Assert.AreEqual(Article().Render(), "<article></article>");
            Assert.AreEqual(Aside().Render(), "<aside></aside>");
            Assert.AreEqual(Audio().Render(), "<audio></audio>");
            Assert.AreEqual(B().Render(), "<b></b>");
            Assert.AreEqual(B("Text").Render(), "<b>Text</b>");
            Assert.AreEqual(Bdi().Render(), "<bdi></bdi>");
            Assert.AreEqual(Bdi("Text").Render(), "<bdi>Text</bdi>");
            Assert.AreEqual(Bdo().Render(), "<bdo></bdo>");
            Assert.AreEqual(Bdo("Text").Render(), "<bdo>Text</bdo>");
            Assert.AreEqual(Blockquote().Render(), "<blockquote></blockquote>");
            Assert.AreEqual(Blockquote("Text").Render(), "<blockquote>Text</blockquote>");
            Assert.AreEqual(Body().Render(), "<body></body>");
            Assert.AreEqual(Button().Render(), "<button></button>");
            Assert.AreEqual(Button("Text").Render(), "<button>Text</button>");
            Assert.AreEqual(Canvas().Render(), "<canvas></canvas>");
            Assert.AreEqual(Caption().Render(), "<caption></caption>");
            Assert.AreEqual(Caption("Text").Render(), "<caption>Text</caption>");
            Assert.AreEqual(Cite().Render(), "<cite></cite>");
            Assert.AreEqual(Cite("Text").Render(), "<cite>Text</cite>");
            Assert.AreEqual(Code().Render(), "<code></code>");
            Assert.AreEqual(ColGroup().Render(), "<colgroup></colgroup>");
            Assert.AreEqual(DataList().Render(), "<datalist></datalist>");
            Assert.AreEqual(Dd().Render(), "<dd></dd>");
            Assert.AreEqual(Dd("Text").Render(), "<dd>Text</dd>");
            Assert.AreEqual(Del().Render(), "<del></del>");
            Assert.AreEqual(Del("Text").Render(), "<del>Text</del>");
            Assert.AreEqual(Details().Render(), "<details></details>");
            Assert.AreEqual(Dfn().Render(), "<dfn></dfn>");
            Assert.AreEqual(Dfn("Text").Render(), "<dfn>Text</dfn>");
            Assert.AreEqual(Dialog().Render(), "<dialog></dialog>");
            Assert.AreEqual(Dialog("Text").Render(), "<dialog>Text</dialog>");
            Assert.AreEqual(Div().Render(), "<div></div>");
            Assert.AreEqual(Dl().Render(), "<dl></dl>");
            Assert.AreEqual(Dt().Render(), "<dt></dt>");
            Assert.AreEqual(Dt("Text").Render(), "<dt>Text</dt>");
            Assert.AreEqual(Em().Render(), "<em></em>");
            Assert.AreEqual(Em("Text").Render(), "<em>Text</em>");
            Assert.AreEqual(FieldSet().Render(), "<fieldset></fieldset>");
            Assert.AreEqual(FigCaption().Render(), "<figcaption></figcaption>");
            Assert.AreEqual(FigCaption("Text").Render(), "<figcaption>Text</figcaption>");
            Assert.AreEqual(Figure().Render(), "<figure></figure>");
            Assert.AreEqual(Footer().Render(), "<footer></footer>");
            Assert.AreEqual(Form().Render(), "<form></form>");
            Assert.AreEqual(H1().Render(), "<h1></h1>");
            Assert.AreEqual(H1("Text").Render(), "<h1>Text</h1>");
            Assert.AreEqual(H2().Render(), "<h2></h2>");
            Assert.AreEqual(H2("Text").Render(), "<h2>Text</h2>");
            Assert.AreEqual(H3().Render(), "<h3></h3>");
            Assert.AreEqual(H3("Text").Render(), "<h3>Text</h3>");
            Assert.AreEqual(H4().Render(), "<h4></h4>");
            Assert.AreEqual(H4("Text").Render(), "<h4>Text</h4>");
            Assert.AreEqual(H5().Render(), "<h5></h5>");
            Assert.AreEqual(H5("Text").Render(), "<h5>Text</h5>");
            Assert.AreEqual(H6().Render(), "<h6></h6>");
            Assert.AreEqual(H6("Text").Render(), "<h6>Text</h6>");
            Assert.AreEqual(Head().Render(), "<head></head>");
            Assert.AreEqual(Header().Render(), "<header></header>");
            Assert.AreEqual(Html().Render(), "<html></html>");
            Assert.AreEqual(I().Render(), "<i></i>");
            Assert.AreEqual(I("Text").Render(), "<i>Text</i>");
            Assert.AreEqual(IFrame().Render(), "<iframe></iframe>");
            Assert.AreEqual(Ins().Render(), "<ins></ins>");
            Assert.AreEqual(Ins("Text").Render(), "<ins>Text</ins>");
            Assert.AreEqual(Kbd().Render(), "<kbd></kbd>");
            Assert.AreEqual(Label().Render(), "<label></label>");
            Assert.AreEqual(Label("Text").Render(), "<label>Text</label>");
            Assert.AreEqual(Legend().Render(), "<legend></legend>");
            Assert.AreEqual(Legend("Text").Render(), "<legend>Text</legend>");
            Assert.AreEqual(Li().Render(), "<li></li>");
            Assert.AreEqual(Li("Text").Render(), "<li>Text</li>");
            Assert.AreEqual(Main().Render(), "<main></main>");
            Assert.AreEqual(Map().Render(), "<map></map>");
            Assert.AreEqual(Mark().Render(), "<mark></mark>");
            Assert.AreEqual(Menu().Render(), "<menu></menu>");
            Assert.AreEqual(MenuItem().Render(), "<menuitem></menuitem>");
            Assert.AreEqual(Meter().Render(), "<meter></meter>");
            Assert.AreEqual(Nav().Render(), "<nav></nav>");
            Assert.AreEqual(NoScript().Render(), "<noscript></noscript>");
            Assert.AreEqual(Object().Render(), "<object></object>");
            Assert.AreEqual(Ol().Render(), "<ol></ol>");
            Assert.AreEqual(OptGroup().Render(), "<optgroup></optgroup>");
            Assert.AreEqual(Option().Render(), "<option></option>");
            Assert.AreEqual(Option("Text").Render(), "<option>Text</option>");
            Assert.AreEqual(Output().Render(), "<output></output>");
            Assert.AreEqual(P().Render(), "<p></p>");
            Assert.AreEqual(P("Text").Render(), "<p>Text</p>");
            Assert.AreEqual(Pre().Render(), "<pre></pre>");
            Assert.AreEqual(Progress().Render(), "<progress></progress>");
            Assert.AreEqual(Q().Render(), "<q></q>");
            Assert.AreEqual(Q("Text").Render(), "<q>Text</q>");
            Assert.AreEqual(Rp().Render(), "<rp></rp>");
            Assert.AreEqual(Rt().Render(), "<rt></rt>");
            Assert.AreEqual(Ruby().Render(), "<ruby></ruby>");
            Assert.AreEqual(S().Render(), "<s></s>");
            Assert.AreEqual(S("Text").Render(), "<s>Text</s>");
            Assert.AreEqual(Samp().Render(), "<samp></samp>");
            Assert.AreEqual(Script().Render(), "<script></script>");
            Assert.AreEqual(Section().Render(), "<section></section>");
            Assert.AreEqual(Select().Render(), "<select></select>");
            Assert.AreEqual(Small().Render(), "<small></small>");
            Assert.AreEqual(Small("Text").Render(), "<small>Text</small>");
            Assert.AreEqual(Span().Render(), "<span></span>");
            Assert.AreEqual(Span("Text").Render(), "<span>Text</span>");
            Assert.AreEqual(Strong().Render(), "<strong></strong>");
            Assert.AreEqual(Strong("Text").Render(), "<strong>Text</strong>");
            Assert.AreEqual(Style().Render(), "<style></style>");
            Assert.AreEqual(Sub().Render(), "<sub></sub>");
            Assert.AreEqual(Sub("Text").Render(), "<sub>Text</sub>");
            Assert.AreEqual(Summary().Render(), "<summary></summary>");
            Assert.AreEqual(Summary("Text").Render(), "<summary>Text</summary>");
            Assert.AreEqual(Sup().Render(), "<sup></sup>");
            Assert.AreEqual(Sup("Text").Render(), "<sup>Text</sup>");
            Assert.AreEqual(Table().Render(), "<table></table>");
            Assert.AreEqual(Tbody().Render(), "<tbody></tbody>");
            Assert.AreEqual(Td().Render(), "<td></td>");
            Assert.AreEqual(Td("Text").Render(), "<td>Text</td>");
            Assert.AreEqual(TextArea().Render(), "<textarea></textarea>");
            Assert.AreEqual(Tfoot().Render(), "<tfoot></tfoot>");
            Assert.AreEqual(Th().Render(), "<th></th>");
            Assert.AreEqual(Th("Text").Render(), "<th>Text</th>");
            Assert.AreEqual(Thead().Render(), "<thead></thead>");
            Assert.AreEqual(Time().Render(), "<time></time>");
            Assert.AreEqual(Title().Render(), "<title></title>");
            Assert.AreEqual(Title("Text").Render(), "<title>Text</title>");
            Assert.AreEqual(Tr().Render(), "<tr></tr>");
            Assert.AreEqual(U().Render(), "<u></u>");
            Assert.AreEqual(U("Text").Render(), "<u>Text</u>");
            Assert.AreEqual(Ul().Render(), "<ul></ul>");
            Assert.AreEqual(Var().Render(), "<var></var>");
            Assert.AreEqual(Video().Render(), "<video></video>");
        }

    }
}
