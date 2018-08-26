using DotNet2HTML.Attributes;
using DotNet2HTML.Tags;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNet2HTML
{
    public class TagCreator
    {

        /// <summary>
        /// Generic if-expression to do if'ing inside method calls
        /// </summary>
        /// <typeparam name="T">The derived generic parameter type</typeparam>
        /// <param name="condition">the condition to if-on</param>
        /// <param name="ifValue">the value to return if condition is true</param>
        /// <returns>value if condition is true, null (default(T)) otherwise</returns>
        public static T If<T>(bool condition, T ifValue)
        {
            return condition ? ifValue : default(T);
        }

        /// <summary>
        /// Like <see cref="If{T}(bool, T)"/>, but returns eles-value instead of null
        /// </summary>
        /// <typeparam name="T">The derived generic parameter type</typeparam>
        /// <param name="condition">the condition to if-on</param>
        /// <param name="ifValue">the value to return if condition is true</param>
        /// <param name="elseValue">the value to return if condition is false</param>
        /// <returns>ifValue if condition is true, elseValue otherwise</returns>
        public static T IfElse<T>(bool condition, T ifValue, T elseValue)
        {
            return condition ? ifValue : elseValue;
        }

        /// <summary>
        /// Returns a <see cref="Attr.ShortForm"/> object with either id, classes or both,
        /// obtained from parsing the input string using css selector syntax
        /// </summary>
        /// <param name="attrs">the string with shortform attributes, only id and class is supported</param>
        /// <returns>a <see cref="Attr.ShortForm"/> object</returns>
        public static Attr.ShortForm Attrs(string attrs)
        {
            return Attr.ShortFromFromAttrsString(attrs);
        }

        /// <summary>
        /// Returns the HTML created by concatenating the input elements,
        /// separated by space, in encounter order.
        /// Also removes spaces before periods and commas.
        /// </summary>
        /// <param name="stringOrDomObjects">the elements to join</param>
        /// <returns>joined elements as HTML</returns>
        public static UnescapedText Join(params object[] stringOrDomObjects)
        {
            return DomContentJoiner.Join(" ", true, stringOrDomObjects);
        }

        /*
         * 
         * Intended usage: {@literal each(numbers, n -> li(n.tostring()))}
         *
         * @param <T>        
         * @param collection , ex: a list of values "1, 2, 3"
         * @param mapper     , ex: {@literal "n -> li(n.tostring())"}
         * @return  {@literal (ex. docs: [li(1), li(2), li(3)])}
         */

        /// <summary>
        /// Creates a DomContent object containing HTML using a mapping function on a collection
        /// </summary>
        /// <typeparam name="T">The derived generic parameter type</typeparam>
        /// <param name="collection">the collection to iterate over</param>
        /// <param name="mapper">the mapping function</param>
        /// <returns><see cref="DomContent"/> containing mapped data</returns>
        public static DomContent Each<T>(IEnumerable<T> collection, Func<T, DomContent> mapper)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            ContainerTag tag = Tag(null);
            while (enumerator.MoveNext())
            {
                DomContent current = mapper(enumerator.Current);
                tag.With(current);
            }
            return tag;
        }

        /// <summary>
        /// Creates a DomContent object containing HTML using a mapping function on a map
        /// </summary>
        /// <typeparam name="I">The type of the keys</typeparam>
        /// <typeparam name="T">The type of the values</typeparam>
        /// <param name="map">the map to iterate over</param>
        /// <param name="mapper">the mapping function</param>
        /// <returns><see cref="DomContent"/>containing mapped data</returns>
        public static DomContent Each<I, T>(IDictionary<I, T> map, Func<I, T, DomContent> mapper)
        {
            IEnumerator<KeyValuePair<I, T>> enumerator = map.GetEnumerator();
            ContainerTag tag = Tag(null);
            while (enumerator.MoveNext())
            {
                DomContent current = mapper(enumerator.Current.Key, enumerator.Current.Value);
                tag.With(current);
            }
            return tag;
        }

        /// <summary>
        /// Filters a collection to a list, to be used with <see cref="Each{T}(IEnumerable{T}, Func{T, DomContent})"/>
        /// </summary>
        /// <typeparam name="T">The derived generic parameter type</typeparam>
        /// <param name="collection">the collection to filter</param>
        /// <param name="filter">the filter</param>
        /// <returns>the filtered collection as a list</returns>
        public static List<T> Filter<T>(IEnumerable<T> collection, Func<T, bool> filter)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            List<T> filtered = new List<T>();
            while (enumerator.MoveNext())
            {
                if (filter(enumerator.Current))
                {
                    filtered.Add(enumerator.Current);
                }
            }
            return filtered;
        }

        /// <summary>
        /// Wraps a string in an UnescapedText element
        /// </summary>
        /// <param name="html">the input html</param>
        /// <returns>the input html wrapped in an UnescapedText element</returns>
        public static UnescapedText RawHtml(string html)
        {
            return new UnescapedText(html);
        }

        /// <summary>
        /// Wraps a string in a Text element (does html-escaping)
        /// </summary>
        /// <param name="text">the input string</param>
        /// <returns>the input string, html-escaped</returns>
        public static Text Text(string text)
        {
            return new Text(text);
        }
        
        /// <summary>
        /// Return a complete html document string
        /// </summary>
        /// <param name="htmlTag">the html content of a website</param>
        /// <returns>document declaration and rendered html content</returns>
        public static string Document(ContainerTag htmlTag)
        {
            if (htmlTag.TagName == "html")
            {
                return Document().Render() + htmlTag.Render();
            }
            throw new Exception("Only HTML-tag can follow document declaration");
        }

        //Special tags
        public static ContainerTag Tag(string tagName)
        {
            return new ContainerTag(tagName);
        }

        public static EmptyTag EmptyTag(string tagName)
        {
            return new EmptyTag(tagName);
        }

        public static Text FileAsEscapedstring(string path)
        {
            return Text(File.ReadAllText(path));
        }

        public static UnescapedText FileAsString(string path)
        {
            return RawHtml(File.ReadAllText(path));
        }

        public static ContainerTag StyleWithInlineFile(string path)
        {
            return InlineStaticResource.Get(path, InlineStaticResource.TargetFormat.CSS);
        }

        public static ContainerTag ScriptWithInlineFile(string path)
        {
            return InlineStaticResource.Get(path, InlineStaticResource.TargetFormat.JS);
        }

        public static ContainerTag StyleWithInlineFileMin(string path)
        {
            return InlineStaticResource.Get(path, InlineStaticResource.TargetFormat.CSS_MIN);
        }

        public static ContainerTag ScriptWithInlineFileMin(string path)
        {
            return InlineStaticResource.Get(path, InlineStaticResource.TargetFormat.JS_MIN);
        }

        public static DomContent Document()
        {
            return RawHtml("<!DOCTYPE html>");
        }

        public static EmptyTag Area()
        {
            return new EmptyTag("area");
        }

        public static EmptyTag Area(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("area"), shortAttr);
        }

        public static EmptyTag Base()
        {
            return new EmptyTag("base");
        }

        public static EmptyTag Base(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("base"), shortAttr);
        }

        public static EmptyTag Br()
        {
            return new EmptyTag("br");
        }

        public static EmptyTag Br(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("br"), shortAttr);
        }

        public static EmptyTag Col()
        {
            return new EmptyTag("col");
        }

        public static EmptyTag Col(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("col"), shortAttr);
        }

        public static EmptyTag Embed()
        {
            return new EmptyTag("embed");
        }

        public static EmptyTag Embed(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("embed"), shortAttr);
        }

        public static EmptyTag Hr()
        {
            return new EmptyTag("hr");
        }

        public static EmptyTag Hr(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("hr"), shortAttr);
        }

        public static EmptyTag Img()
        {
            return new EmptyTag("img");
        }

        public static EmptyTag Img(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("img"), shortAttr);
        }

        public static EmptyTag Input()
        {
            return new EmptyTag("input");
        }

        public static EmptyTag Input(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("input"), shortAttr);
        }

        public static EmptyTag Keygen()
        {
            return new EmptyTag("keygen");
        }

        public static EmptyTag Keygen(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("keygen"), shortAttr);
        }

        public static EmptyTag Link()
        {
            return new EmptyTag("link");
        }

        public static EmptyTag Link(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("link"), shortAttr);
        }

        public static EmptyTag Meta()
        {
            return new EmptyTag("meta");
        }

        public static EmptyTag Meta(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("meta"), shortAttr);
        }

        public static EmptyTag Param()
        {
            return new EmptyTag("param");
        }

        public static EmptyTag Param(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("param"), shortAttr);
        }

        public static EmptyTag Source()
        {
            return new EmptyTag("source");
        }

        public static EmptyTag Source(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("source"), shortAttr);
        }

        public static EmptyTag Track()
        {
            return new EmptyTag("track");
        }

        public static EmptyTag Track(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("track"), shortAttr);
        }

        public static EmptyTag Wbr()
        {
            return new EmptyTag("wbr");
        }

        public static EmptyTag Wbr(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new EmptyTag("wbr"), shortAttr);
        }

        public static ContainerTag A()
        {
            return new ContainerTag("a");
        }

        public static ContainerTag A(string text)
        {
            return new ContainerTag("a").WithText(text);
        }

        public static ContainerTag A(params DomContent[] dc)
        {
            return new ContainerTag("a").With(dc);
        }

        public static ContainerTag A(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("a"), shortAttr);
        }

        public static ContainerTag A(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("a").WithText(text), shortAttr);
        }

        public static ContainerTag A(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("a").With(dc), shortAttr);
        }

        public static ContainerTag Abbr()
        {
            return new ContainerTag("abbr");
        }

        public static ContainerTag Abbr(string text)
        {
            return new ContainerTag("abbr").WithText(text);
        }

        public static ContainerTag Abbr(params DomContent[] dc)
        {
            return new ContainerTag("abbr").With(dc);
        }

        public static ContainerTag Abbr(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("abbr"), shortAttr);
        }

        public static ContainerTag Abbr(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("abbr").WithText(text), shortAttr);
        }

        public static ContainerTag Abbr(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("abbr").With(dc), shortAttr);
        }

        public static ContainerTag Address()
        {
            return new ContainerTag("address");
        }

        public static ContainerTag Address(string text)
        {
            return new ContainerTag("address").WithText(text);
        }

        public static ContainerTag Address(params DomContent[] dc)
        {
            return new ContainerTag("address").With(dc);
        }

        public static ContainerTag Address(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("address"), shortAttr);
        }

        public static ContainerTag Address(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("address").WithText(text), shortAttr);
        }

        public static ContainerTag Address(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("address").With(dc), shortAttr);
        }

        public static ContainerTag Article()
        {
            return new ContainerTag("article");
        }

        public static ContainerTag Article(string text)
        {
            return new ContainerTag("article").WithText(text);
        }

        public static ContainerTag Article(params DomContent[] dc)
        {
            return new ContainerTag("article").With(dc);
        }

        public static ContainerTag Article(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("article"), shortAttr);
        }

        public static ContainerTag Article(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("article").WithText(text), shortAttr);
        }

        public static ContainerTag Article(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("article").With(dc), shortAttr);
        }

        public static ContainerTag Aside()
        {
            return new ContainerTag("aside");
        }

        public static ContainerTag Aside(string text)
        {
            return new ContainerTag("aside").WithText(text);
        }

        public static ContainerTag Aside(params DomContent[] dc)
        {
            return new ContainerTag("aside").With(dc);
        }

        public static ContainerTag Aside(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("aside"), shortAttr);
        }

        public static ContainerTag Aside(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("aside").WithText(text), shortAttr);
        }

        public static ContainerTag Aside(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("aside").With(dc), shortAttr);
        }

        public static ContainerTag Audio()
        {
            return new ContainerTag("audio");
        }

        public static ContainerTag Audio(string text)
        {
            return new ContainerTag("audio").WithText(text);
        }

        public static ContainerTag Audio(params DomContent[] dc)
        {
            return new ContainerTag("audio").With(dc);
        }

        public static ContainerTag Audio(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("audio"), shortAttr);
        }

        public static ContainerTag Audio(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("audio").WithText(text), shortAttr);
        }

        public static ContainerTag Audio(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("audio").With(dc), shortAttr);
        }

        public static ContainerTag B()
        {
            return new ContainerTag("b");
        }

        public static ContainerTag B(string text)
        {
            return new ContainerTag("b").WithText(text);
        }

        public static ContainerTag B(params DomContent[] dc)
        {
            return new ContainerTag("b").With(dc);
        }

        public static ContainerTag B(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("b"), shortAttr);
        }

        public static ContainerTag B(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("b").WithText(text), shortAttr);
        }

        public static ContainerTag B(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("b").With(dc), shortAttr);
        }

        public static ContainerTag Bdi()
        {
            return new ContainerTag("bdi");
        }

        public static ContainerTag Bdi(string text)
        {
            return new ContainerTag("bdi").WithText(text);
        }

        public static ContainerTag Bdi(params DomContent[] dc)
        {
            return new ContainerTag("bdi").With(dc);
        }

        public static ContainerTag Bdi(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("bdi"), shortAttr);
        }

        public static ContainerTag Bdi(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("bdi").WithText(text), shortAttr);
        }

        public static ContainerTag Bdi(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("bdi").With(dc), shortAttr);
        }

        public static ContainerTag Bdo()
        {
            return new ContainerTag("bdo");
        }

        public static ContainerTag Bdo(string text)
        {
            return new ContainerTag("bdo").WithText(text);
        }

        public static ContainerTag Bdo(params DomContent[] dc)
        {
            return new ContainerTag("bdo").With(dc);
        }

        public static ContainerTag Bdo(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("bdo"), shortAttr);
        }

        public static ContainerTag Bdo(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("bdo").WithText(text), shortAttr);
        }

        public static ContainerTag Bdo(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("bdo").With(dc), shortAttr);
        }

        public static ContainerTag Blockquote()
        {
            return new ContainerTag("blockquote");
        }

        public static ContainerTag Blockquote(string text)
        {
            return new ContainerTag("blockquote").WithText(text);
        }

        public static ContainerTag Blockquote(params DomContent[] dc)
        {
            return new ContainerTag("blockquote").With(dc);
        }

        public static ContainerTag Blockquote(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("blockquote"), shortAttr);
        }

        public static ContainerTag Blockquote(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("blockquote").WithText(text), shortAttr);
        }

        public static ContainerTag Blockquote(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("blockquote").With(dc), shortAttr);
        }

        public static ContainerTag Body()
        {
            return new ContainerTag("body");
        }

        public static ContainerTag Body(string text)
        {
            return new ContainerTag("body").WithText(text);
        }

        public static ContainerTag Body(params DomContent[] dc)
        {
            return new ContainerTag("body").With(dc);
        }

        public static ContainerTag Body(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("body"), shortAttr);
        }

        public static ContainerTag Body(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("body").WithText(text), shortAttr);
        }

        public static ContainerTag Body(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("body").With(dc), shortAttr);
        }

        public static ContainerTag Button()
        {
            return new ContainerTag("button");
        }

        public static ContainerTag Button(string text)
        {
            return new ContainerTag("button").WithText(text);
        }

        public static ContainerTag Button(params DomContent[] dc)
        {
            return new ContainerTag("button").With(dc);
        }

        public static ContainerTag Button(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("button"), shortAttr);
        }

        public static ContainerTag Button(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("button").WithText(text), shortAttr);
        }

        public static ContainerTag Button(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("button").With(dc), shortAttr);
        }

        public static ContainerTag Canvas()
        {
            return new ContainerTag("canvas");
        }

        public static ContainerTag Canvas(string text)
        {
            return new ContainerTag("canvas").WithText(text);
        }

        public static ContainerTag Canvas(params DomContent[] dc)
        {
            return new ContainerTag("canvas").With(dc);
        }

        public static ContainerTag Canvas(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("canvas"), shortAttr);
        }

        public static ContainerTag Canvas(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("canvas").WithText(text), shortAttr);
        }

        public static ContainerTag Canvas(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("canvas").With(dc), shortAttr);
        }

        public static ContainerTag Caption()
        {
            return new ContainerTag("caption");
        }

        public static ContainerTag Caption(string text)
        {
            return new ContainerTag("caption").WithText(text);
        }

        public static ContainerTag Caption(params DomContent[] dc)
        {
            return new ContainerTag("caption").With(dc);
        }

        public static ContainerTag Caption(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("caption"), shortAttr);
        }

        public static ContainerTag Caption(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("caption").WithText(text), shortAttr);
        }

        public static ContainerTag Caption(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("caption").With(dc), shortAttr);
        }

        public static ContainerTag Cite()
        {
            return new ContainerTag("cite");
        }

        public static ContainerTag Cite(string text)
        {
            return new ContainerTag("cite").WithText(text);
        }

        public static ContainerTag Cite(params DomContent[] dc)
        {
            return new ContainerTag("cite").With(dc);
        }

        public static ContainerTag Cite(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("cite"), shortAttr);
        }

        public static ContainerTag Cite(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("cite").WithText(text), shortAttr);
        }

        public static ContainerTag Cite(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("cite").With(dc), shortAttr);
        }

        public static ContainerTag Code()
        {
            return new ContainerTag("code");
        }

        public static ContainerTag Code(string text)
        {
            return new ContainerTag("code").WithText(text);
        }

        public static ContainerTag Code(params DomContent[] dc)
        {
            return new ContainerTag("code").With(dc);
        }

        public static ContainerTag Code(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("code"), shortAttr);
        }

        public static ContainerTag Code(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("code").WithText(text), shortAttr);
        }

        public static ContainerTag Code(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("code").With(dc), shortAttr);
        }

        public static ContainerTag ColGroup()
        {
            return new ContainerTag("colgroup");
        }

        public static ContainerTag ColGroup(string text)
        {
            return new ContainerTag("colgroup").WithText(text);
        }

        public static ContainerTag ColGroup(params DomContent[] dc)
        {
            return new ContainerTag("colgroup").With(dc);
        }

        public static ContainerTag ColGroup(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("colgroup"), shortAttr);
        }

        public static ContainerTag ColGroup(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("colgroup").WithText(text), shortAttr);
        }

        public static ContainerTag ColGroup(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("colgroup").With(dc), shortAttr);
        }

        public static ContainerTag DataList()
        {
            return new ContainerTag("datalist");
        }

        public static ContainerTag DataList(string text)
        {
            return new ContainerTag("datalist").WithText(text);
        }

        public static ContainerTag DataList(params DomContent[] dc)
        {
            return new ContainerTag("datalist").With(dc);
        }

        public static ContainerTag DataList(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("datalist"), shortAttr);
        }

        public static ContainerTag DataList(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("datalist").WithText(text), shortAttr);
        }

        public static ContainerTag DataList(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("datalist").With(dc), shortAttr);
        }

        public static ContainerTag Dd()
        {
            return new ContainerTag("dd");
        }

        public static ContainerTag Dd(string text)
        {
            return new ContainerTag("dd").WithText(text);
        }

        public static ContainerTag Dd(params DomContent[] dc)
        {
            return new ContainerTag("dd").With(dc);
        }

        public static ContainerTag Dd(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("dd"), shortAttr);
        }

        public static ContainerTag Dd(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("dd").WithText(text), shortAttr);
        }

        public static ContainerTag Dd(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("dd").With(dc), shortAttr);
        }

        public static ContainerTag Del()
        {
            return new ContainerTag("del");
        }

        public static ContainerTag Del(string text)
        {
            return new ContainerTag("del").WithText(text);
        }

        public static ContainerTag Del(params DomContent[] dc)
        {
            return new ContainerTag("del").With(dc);
        }

        public static ContainerTag Del(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("del"), shortAttr);
        }

        public static ContainerTag Del(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("del").WithText(text), shortAttr);
        }

        public static ContainerTag Del(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("del").With(dc), shortAttr);
        }

        public static ContainerTag Details()
        {
            return new ContainerTag("details");
        }

        public static ContainerTag Details(string text)
        {
            return new ContainerTag("details").WithText(text);
        }

        public static ContainerTag Details(params DomContent[] dc)
        {
            return new ContainerTag("details").With(dc);
        }

        public static ContainerTag Details(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("details"), shortAttr);
        }

        public static ContainerTag Details(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("details").WithText(text), shortAttr);
        }

        public static ContainerTag Details(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("details").With(dc), shortAttr);
        }

        public static ContainerTag Dfn()
        {
            return new ContainerTag("dfn");
        }

        public static ContainerTag Dfn(string text)
        {
            return new ContainerTag("dfn").WithText(text);
        }

        public static ContainerTag Dfn(params DomContent[] dc)
        {
            return new ContainerTag("dfn").With(dc);
        }

        public static ContainerTag Dfn(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("dfn"), shortAttr);
        }

        public static ContainerTag Dfn(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("dfn").WithText(text), shortAttr);
        }

        public static ContainerTag Dfn(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("dfn").With(dc), shortAttr);
        }

        public static ContainerTag Dialog()
        {
            return new ContainerTag("dialog");
        }

        public static ContainerTag Dialog(string text)
        {
            return new ContainerTag("dialog").WithText(text);
        }

        public static ContainerTag Dialog(params DomContent[] dc)
        {
            return new ContainerTag("dialog").With(dc);
        }

        public static ContainerTag Dialog(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("dialog"), shortAttr);
        }

        public static ContainerTag Dialog(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("dialog").WithText(text), shortAttr);
        }

        public static ContainerTag Dialog(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("dialog").With(dc), shortAttr);
        }

        public static ContainerTag Div()
        {
            return new ContainerTag("div");
        }

        public static ContainerTag Div(string text)
        {
            return new ContainerTag("div").WithText(text);
        }

        public static ContainerTag Div(params DomContent[] dc)
        {
            return new ContainerTag("div").With(dc);
        }

        public static ContainerTag Div(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("div"), shortAttr);
        }

        public static ContainerTag Div(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("div").WithText(text), shortAttr);
        }

        public static ContainerTag Div(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("div").With(dc), shortAttr);
        }

        public static ContainerTag Dl()
        {
            return new ContainerTag("dl");
        }

        public static ContainerTag Dl(string text)
        {
            return new ContainerTag("dl").WithText(text);
        }

        public static ContainerTag Dl(params DomContent[] dc)
        {
            return new ContainerTag("dl").With(dc);
        }

        public static ContainerTag Dl(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("dl"), shortAttr);
        }

        public static ContainerTag Dl(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("dl").WithText(text), shortAttr);
        }

        public static ContainerTag Dl(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("dl").With(dc), shortAttr);
        }

        public static ContainerTag Dt()
        {
            return new ContainerTag("dt");
        }

        public static ContainerTag Dt(string text)
        {
            return new ContainerTag("dt").WithText(text);
        }

        public static ContainerTag Dt(params DomContent[] dc)
        {
            return new ContainerTag("dt").With(dc);
        }

        public static ContainerTag Dt(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("dt"), shortAttr);
        }

        public static ContainerTag Dt(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("dt").WithText(text), shortAttr);
        }

        public static ContainerTag Dt(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("dt").With(dc), shortAttr);
        }

        public static ContainerTag Em()
        {
            return new ContainerTag("em");
        }

        public static ContainerTag Em(string text)
        {
            return new ContainerTag("em").WithText(text);
        }

        public static ContainerTag Em(params DomContent[] dc)
        {
            return new ContainerTag("em").With(dc);
        }

        public static ContainerTag Em(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("em"), shortAttr);
        }

        public static ContainerTag Em(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("em").WithText(text), shortAttr);
        }

        public static ContainerTag Em(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("em").With(dc), shortAttr);
        }

        public static ContainerTag FieldSet()
        {
            return new ContainerTag("fieldset");
        }

        public static ContainerTag FieldSet(string text)
        {
            return new ContainerTag("fieldset").WithText(text);
        }

        public static ContainerTag FieldSet(params DomContent[] dc)
        {
            return new ContainerTag("fieldset").With(dc);
        }

        public static ContainerTag FieldSet(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("fieldset"), shortAttr);
        }

        public static ContainerTag FieldSet(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("fieldset").WithText(text), shortAttr);
        }

        public static ContainerTag FieldSet(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("fieldset").With(dc), shortAttr);
        }

        public static ContainerTag FigCaption()
        {
            return new ContainerTag("figcaption");
        }

        public static ContainerTag FigCaption(string text)
        {
            return new ContainerTag("figcaption").WithText(text);
        }

        public static ContainerTag FigCaption(params DomContent[] dc)
        {
            return new ContainerTag("figcaption").With(dc);
        }

        public static ContainerTag FigCaption(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("figcaption"), shortAttr);
        }

        public static ContainerTag FigCaption(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("figcaption").WithText(text), shortAttr);
        }

        public static ContainerTag FigCaption(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("figcaption").With(dc), shortAttr);
        }

        public static ContainerTag Figure()
        {
            return new ContainerTag("figure");
        }

        public static ContainerTag Figure(string text)
        {
            return new ContainerTag("figure").WithText(text);
        }

        public static ContainerTag Figure(params DomContent[] dc)
        {
            return new ContainerTag("figure").With(dc);
        }

        public static ContainerTag Figure(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("figure"), shortAttr);
        }

        public static ContainerTag Figure(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("figure").WithText(text), shortAttr);
        }

        public static ContainerTag Figure(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("figure").With(dc), shortAttr);
        }

        public static ContainerTag Footer()
        {
            return new ContainerTag("footer");
        }

        public static ContainerTag Footer(string text)
        {
            return new ContainerTag("footer").WithText(text);
        }

        public static ContainerTag Footer(params DomContent[] dc)
        {
            return new ContainerTag("footer").With(dc);
        }

        public static ContainerTag Footer(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("footer"), shortAttr);
        }

        public static ContainerTag Footer(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("footer").WithText(text), shortAttr);
        }

        public static ContainerTag Footer(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("footer").With(dc), shortAttr);
        }

        public static ContainerTag Form()
        {
            return new ContainerTag("form");
        }

        public static ContainerTag Form(string text)
        {
            return new ContainerTag("form").WithText(text);
        }

        public static ContainerTag Form(params DomContent[] dc)
        {
            return new ContainerTag("form").With(dc);
        }

        public static ContainerTag Form(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("form"), shortAttr);
        }

        public static ContainerTag Form(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("form").WithText(text), shortAttr);
        }

        public static ContainerTag Form(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("form").With(dc), shortAttr);
        }

        public static ContainerTag H1()
        {
            return new ContainerTag("h1");
        }

        public static ContainerTag H1(string text)
        {
            return new ContainerTag("h1").WithText(text);
        }

        public static ContainerTag H1(params DomContent[] dc)
        {
            return new ContainerTag("h1").With(dc);
        }

        public static ContainerTag H1(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("h1"), shortAttr);
        }

        public static ContainerTag H1(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("h1").WithText(text), shortAttr);
        }

        public static ContainerTag H1(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("h1").With(dc), shortAttr);
        }

        public static ContainerTag H2()
        {
            return new ContainerTag("h2");
        }

        public static ContainerTag H2(string text)
        {
            return new ContainerTag("h2").WithText(text);
        }

        public static ContainerTag H2(params DomContent[] dc)
        {
            return new ContainerTag("h2").With(dc);
        }

        public static ContainerTag H2(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("h2"), shortAttr);
        }

        public static ContainerTag H2(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("h2").WithText(text), shortAttr);
        }

        public static ContainerTag H2(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("h2").With(dc), shortAttr);
        }

        public static ContainerTag H3()
        {
            return new ContainerTag("h3");
        }

        public static ContainerTag H3(string text)
        {
            return new ContainerTag("h3").WithText(text);
        }

        public static ContainerTag H3(params DomContent[] dc)
        {
            return new ContainerTag("h3").With(dc);
        }

        public static ContainerTag H3(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("h3"), shortAttr);
        }

        public static ContainerTag H3(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("h3").WithText(text), shortAttr);
        }

        public static ContainerTag H3(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("h3").With(dc), shortAttr);
        }

        public static ContainerTag H4()
        {
            return new ContainerTag("h4");
        }

        public static ContainerTag H4(string text)
        {
            return new ContainerTag("h4").WithText(text);
        }

        public static ContainerTag H4(params DomContent[] dc)
        {
            return new ContainerTag("h4").With(dc);
        }

        public static ContainerTag H4(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("h4"), shortAttr);
        }

        public static ContainerTag H4(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("h4").WithText(text), shortAttr);
        }

        public static ContainerTag H4(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("h4").With(dc), shortAttr);
        }

        public static ContainerTag H5()
        {
            return new ContainerTag("h5");
        }

        public static ContainerTag H5(string text)
        {
            return new ContainerTag("h5").WithText(text);
        }

        public static ContainerTag H5(params DomContent[] dc)
        {
            return new ContainerTag("h5").With(dc);
        }

        public static ContainerTag H5(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("h5"), shortAttr);
        }

        public static ContainerTag H5(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("h5").WithText(text), shortAttr);
        }

        public static ContainerTag H5(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("h5").With(dc), shortAttr);
        }

        public static ContainerTag H6()
        {
            return new ContainerTag("h6");
        }

        public static ContainerTag H6(string text)
        {
            return new ContainerTag("h6").WithText(text);
        }

        public static ContainerTag H6(params DomContent[] dc)
        {
            return new ContainerTag("h6").With(dc);
        }

        public static ContainerTag H6(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("h6"), shortAttr);
        }

        public static ContainerTag H6(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("h6").WithText(text), shortAttr);
        }

        public static ContainerTag H6(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("h6").With(dc), shortAttr);
        }

        public static ContainerTag Head()
        {
            return new ContainerTag("head");
        }

        public static ContainerTag Head(string text)
        {
            return new ContainerTag("head").WithText(text);
        }

        public static ContainerTag Head(params DomContent[] dc)
        {
            return new ContainerTag("head").With(dc);
        }

        public static ContainerTag Head(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("head"), shortAttr);
        }

        public static ContainerTag Head(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("head").WithText(text), shortAttr);
        }

        public static ContainerTag Head(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("head").With(dc), shortAttr);
        }

        public static ContainerTag Header()
        {
            return new ContainerTag("header");
        }

        public static ContainerTag Header(string text)
        {
            return new ContainerTag("header").WithText(text);
        }

        public static ContainerTag Header(params DomContent[] dc)
        {
            return new ContainerTag("header").With(dc);
        }

        public static ContainerTag Header(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("header"), shortAttr);
        }

        public static ContainerTag Header(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("header").WithText(text), shortAttr);
        }

        public static ContainerTag Header(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("header").With(dc), shortAttr);
        }

        public static ContainerTag Html()
        {
            return new ContainerTag("html");
        }

        public static ContainerTag Html(string text)
        {
            return new ContainerTag("html").WithText(text);
        }

        public static ContainerTag Html(params DomContent[] dc)
        {
            return new ContainerTag("html").With(dc);
        }

        public static ContainerTag Html(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("html"), shortAttr);
        }

        public static ContainerTag Html(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("html").WithText(text), shortAttr);
        }

        public static ContainerTag Html(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("html").With(dc), shortAttr);
        }

        public static ContainerTag I()
        {
            return new ContainerTag("i");
        }

        public static ContainerTag I(string text)
        {
            return new ContainerTag("i").WithText(text);
        }

        public static ContainerTag I(params DomContent[] dc)
        {
            return new ContainerTag("i").With(dc);
        }

        public static ContainerTag I(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("i"), shortAttr);
        }

        public static ContainerTag I(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("i").WithText(text), shortAttr);
        }

        public static ContainerTag I(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("i").With(dc), shortAttr);
        }

        public static ContainerTag IFrame()
        {
            return new ContainerTag("iframe");
        }

        public static ContainerTag IFrame(string text)
        {
            return new ContainerTag("iframe").WithText(text);
        }

        public static ContainerTag IFrame(params DomContent[] dc)
        {
            return new ContainerTag("iframe").With(dc);
        }

        public static ContainerTag IFrame(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("iframe"), shortAttr);
        }

        public static ContainerTag IFrame(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("iframe").WithText(text), shortAttr);
        }

        public static ContainerTag IFrame(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("iframe").With(dc), shortAttr);
        }

        public static ContainerTag Ins()
        {
            return new ContainerTag("ins");
        }

        public static ContainerTag Ins(string text)
        {
            return new ContainerTag("ins").WithText(text);
        }

        public static ContainerTag Ins(params DomContent[] dc)
        {
            return new ContainerTag("ins").With(dc);
        }

        public static ContainerTag Ins(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("ins"), shortAttr);
        }

        public static ContainerTag Ins(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("ins").WithText(text), shortAttr);
        }

        public static ContainerTag Ins(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("ins").With(dc), shortAttr);
        }

        public static ContainerTag Kbd()
        {
            return new ContainerTag("kbd");
        }

        public static ContainerTag Kbd(string text)
        {
            return new ContainerTag("kbd").WithText(text);
        }

        public static ContainerTag Kbd(params DomContent[] dc)
        {
            return new ContainerTag("kbd").With(dc);
        }

        public static ContainerTag Kbd(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("kbd"), shortAttr);
        }

        public static ContainerTag Kbd(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("kbd").WithText(text), shortAttr);
        }

        public static ContainerTag Kbd(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("kbd").With(dc), shortAttr);
        }

        public static ContainerTag Label()
        {
            return new ContainerTag("label");
        }

        public static ContainerTag Label(string text)
        {
            return new ContainerTag("label").WithText(text);
        }

        public static ContainerTag Label(params DomContent[] dc)
        {
            return new ContainerTag("label").With(dc);
        }

        public static ContainerTag Label(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("label"), shortAttr);
        }

        public static ContainerTag Label(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("label").WithText(text), shortAttr);
        }

        public static ContainerTag Label(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("label").With(dc), shortAttr);
        }

        public static ContainerTag Legend()
        {
            return new ContainerTag("legend");
        }

        public static ContainerTag Legend(string text)
        {
            return new ContainerTag("legend").WithText(text);
        }

        public static ContainerTag Legend(params DomContent[] dc)
        {
            return new ContainerTag("legend").With(dc);
        }

        public static ContainerTag Legend(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("legend"), shortAttr);
        }

        public static ContainerTag Legend(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("legend").WithText(text), shortAttr);
        }

        public static ContainerTag Legend(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("legend").With(dc), shortAttr);
        }

        public static ContainerTag Li()
        {
            return new ContainerTag("li");
        }

        public static ContainerTag Li(string text)
        {
            return new ContainerTag("li").WithText(text);
        }

        public static ContainerTag Li(params DomContent[] dc)
        {
            return new ContainerTag("li").With(dc);
        }

        public static ContainerTag Li(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("li"), shortAttr);
        }

        public static ContainerTag Li(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("li").WithText(text), shortAttr);
        }

        public static ContainerTag Li(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("li").With(dc), shortAttr);
        }

        public static ContainerTag Main()
        {
            return new ContainerTag("main");
        }

        public static ContainerTag Main(string text)
        {
            return new ContainerTag("main").WithText(text);
        }

        public static ContainerTag Main(params DomContent[] dc)
        {
            return new ContainerTag("main").With(dc);
        }

        public static ContainerTag Main(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("main"), shortAttr);
        }

        public static ContainerTag Main(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("main").WithText(text), shortAttr);
        }

        public static ContainerTag Main(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("main").With(dc), shortAttr);
        }

        public static ContainerTag Map()
        {
            return new ContainerTag("map");
        }

        public static ContainerTag Map(string text)
        {
            return new ContainerTag("map").WithText(text);
        }

        public static ContainerTag Map(params DomContent[] dc)
        {
            return new ContainerTag("map").With(dc);
        }

        public static ContainerTag Map(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("map"), shortAttr);
        }

        public static ContainerTag Map(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("map").WithText(text), shortAttr);
        }

        public static ContainerTag Map(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("map").With(dc), shortAttr);
        }

        public static ContainerTag Mark()
        {
            return new ContainerTag("mark");
        }

        public static ContainerTag Mark(string text)
        {
            return new ContainerTag("mark").WithText(text);
        }

        public static ContainerTag Mark(params DomContent[] dc)
        {
            return new ContainerTag("mark").With(dc);
        }

        public static ContainerTag Mark(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("mark"), shortAttr);
        }

        public static ContainerTag Mark(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("mark").WithText(text), shortAttr);
        }

        public static ContainerTag Mark(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("mark").With(dc), shortAttr);
        }

        public static ContainerTag Menu()
        {
            return new ContainerTag("menu");
        }

        public static ContainerTag Menu(string text)
        {
            return new ContainerTag("menu").WithText(text);
        }

        public static ContainerTag Menu(params DomContent[] dc)
        {
            return new ContainerTag("menu").With(dc);
        }

        public static ContainerTag Menu(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("menu"), shortAttr);
        }

        public static ContainerTag Menu(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("menu").WithText(text), shortAttr);
        }

        public static ContainerTag Menu(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("menu").With(dc), shortAttr);
        }

        public static ContainerTag MenuItem()
        {
            return new ContainerTag("menuitem");
        }

        public static ContainerTag MenuItem(string text)
        {
            return new ContainerTag("menuitem").WithText(text);
        }

        public static ContainerTag MenuItem(params DomContent[] dc)
        {
            return new ContainerTag("menuitem").With(dc);
        }

        public static ContainerTag MenuItem(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("menuitem"), shortAttr);
        }

        public static ContainerTag MenuItem(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("menuitem").WithText(text), shortAttr);
        }

        public static ContainerTag MenuItem(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("menuitem").With(dc), shortAttr);
        }

        public static ContainerTag Meter()
        {
            return new ContainerTag("meter");
        }

        public static ContainerTag Meter(string text)
        {
            return new ContainerTag("meter").WithText(text);
        }

        public static ContainerTag Meter(params DomContent[] dc)
        {
            return new ContainerTag("meter").With(dc);
        }

        public static ContainerTag Meter(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("meter"), shortAttr);
        }

        public static ContainerTag Meter(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("meter").WithText(text), shortAttr);
        }

        public static ContainerTag Meter(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("meter").With(dc), shortAttr);
        }

        public static ContainerTag Nav()
        {
            return new ContainerTag("nav");
        }

        public static ContainerTag Nav(string text)
        {
            return new ContainerTag("nav").WithText(text);
        }

        public static ContainerTag Nav(params DomContent[] dc)
        {
            return new ContainerTag("nav").With(dc);
        }

        public static ContainerTag Nav(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("nav"), shortAttr);
        }

        public static ContainerTag Nav(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("nav").WithText(text), shortAttr);
        }

        public static ContainerTag Nav(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("nav").With(dc), shortAttr);
        }

        public static ContainerTag NoScript()
        {
            return new ContainerTag("noscript");
        }

        public static ContainerTag NoScript(string text)
        {
            return new ContainerTag("noscript").WithText(text);
        }

        public static ContainerTag NoScript(params DomContent[] dc)
        {
            return new ContainerTag("noscript").With(dc);
        }

        public static ContainerTag NoScript(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("noscript"), shortAttr);
        }

        public static ContainerTag NoScript(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("noscript").WithText(text), shortAttr);
        }

        public static ContainerTag NoScript(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("noscript").With(dc), shortAttr);
        }

        public static ContainerTag Object()
        {
            return new ContainerTag("object");
        }

        public static ContainerTag Object(string text)
        {
            return new ContainerTag("object").WithText(text);
        }

        public static ContainerTag Object(params DomContent[] dc)
        {
            return new ContainerTag("object").With(dc);
        }

        public static ContainerTag Object(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("object"), shortAttr);
        }

        public static ContainerTag Object(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("object").WithText(text), shortAttr);
        }

        public static ContainerTag Object(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("object").With(dc), shortAttr);
        }

        public static ContainerTag Ol()
        {
            return new ContainerTag("ol");
        }

        public static ContainerTag Ol(string text)
        {
            return new ContainerTag("ol").WithText(text);
        }

        public static ContainerTag Ol(params DomContent[] dc)
        {
            return new ContainerTag("ol").With(dc);
        }

        public static ContainerTag Ol(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("ol"), shortAttr);
        }

        public static ContainerTag Ol(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("ol").WithText(text), shortAttr);
        }

        public static ContainerTag Ol(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("ol").With(dc), shortAttr);
        }

        public static ContainerTag OptGroup()
        {
            return new ContainerTag("optgroup");
        }

        public static ContainerTag OptGroup(string text)
        {
            return new ContainerTag("optgroup").WithText(text);
        }

        public static ContainerTag OptGroup(params DomContent[] dc)
        {
            return new ContainerTag("optgroup").With(dc);
        }

        public static ContainerTag OptGroup(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("optgroup"), shortAttr);
        }

        public static ContainerTag OptGroup(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("optgroup").WithText(text), shortAttr);
        }

        public static ContainerTag OptGroup(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("optgroup").With(dc), shortAttr);
        }

        public static ContainerTag Option()
        {
            return new ContainerTag("option");
        }

        public static ContainerTag Option(string text)
        {
            return new ContainerTag("option").WithText(text);
        }

        public static ContainerTag Option(params DomContent[] dc)
        {
            return new ContainerTag("option").With(dc);
        }

        public static ContainerTag Option(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("option"), shortAttr);
        }

        public static ContainerTag Option(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("option").WithText(text), shortAttr);
        }

        public static ContainerTag Option(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("option").With(dc), shortAttr);
        }

        public static ContainerTag Output()
        {
            return new ContainerTag("output");
        }

        public static ContainerTag Output(string text)
        {
            return new ContainerTag("output").WithText(text);
        }

        public static ContainerTag Output(params DomContent[] dc)
        {
            return new ContainerTag("output").With(dc);
        }

        public static ContainerTag Output(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("output"), shortAttr);
        }

        public static ContainerTag Output(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("output").WithText(text), shortAttr);
        }

        public static ContainerTag Output(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("output").With(dc), shortAttr);
        }

        public static ContainerTag P()
        {
            return new ContainerTag("p");
        }

        public static ContainerTag P(string text)
        {
            return new ContainerTag("p").WithText(text);
        }

        public static ContainerTag P(params DomContent[] dc)
        {
            return new ContainerTag("p").With(dc);
        }

        public static ContainerTag P(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("p"), shortAttr);
        }

        public static ContainerTag P(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("p").WithText(text), shortAttr);
        }

        public static ContainerTag P(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("p").With(dc), shortAttr);
        }

        public static ContainerTag Pre()
        {
            return new ContainerTag("pre");
        }

        public static ContainerTag Pre(string text)
        {
            return new ContainerTag("pre").WithText(text);
        }

        public static ContainerTag Pre(params DomContent[] dc)
        {
            return new ContainerTag("pre").With(dc);
        }

        public static ContainerTag Pre(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("pre"), shortAttr);
        }

        public static ContainerTag Pre(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("pre").WithText(text), shortAttr);
        }

        public static ContainerTag Pre(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("pre").With(dc), shortAttr);
        }

        public static ContainerTag Progress()
        {
            return new ContainerTag("progress");
        }

        public static ContainerTag Progress(string text)
        {
            return new ContainerTag("progress").WithText(text);
        }

        public static ContainerTag Progress(params DomContent[] dc)
        {
            return new ContainerTag("progress").With(dc);
        }

        public static ContainerTag Progress(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("progress"), shortAttr);
        }

        public static ContainerTag Progress(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("progress").WithText(text), shortAttr);
        }

        public static ContainerTag Progress(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("progress").With(dc), shortAttr);
        }

        public static ContainerTag Q()
        {
            return new ContainerTag("q");
        }

        public static ContainerTag Q(string text)
        {
            return new ContainerTag("q").WithText(text);
        }

        public static ContainerTag Q(params DomContent[] dc)
        {
            return new ContainerTag("q").With(dc);
        }

        public static ContainerTag Q(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("q"), shortAttr);
        }

        public static ContainerTag Q(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("q").WithText(text), shortAttr);
        }

        public static ContainerTag Q(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("q").With(dc), shortAttr);
        }

        public static ContainerTag Rp()
        {
            return new ContainerTag("rp");
        }

        public static ContainerTag Rp(string text)
        {
            return new ContainerTag("rp").WithText(text);
        }

        public static ContainerTag Rp(params DomContent[] dc)
        {
            return new ContainerTag("rp").With(dc);
        }

        public static ContainerTag Rp(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("rp"), shortAttr);
        }

        public static ContainerTag Rp(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("rp").WithText(text), shortAttr);
        }

        public static ContainerTag Rp(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("rp").With(dc), shortAttr);
        }

        public static ContainerTag Rt()
        {
            return new ContainerTag("rt");
        }

        public static ContainerTag Rt(string text)
        {
            return new ContainerTag("rt").WithText(text);
        }

        public static ContainerTag Rt(params DomContent[] dc)
        {
            return new ContainerTag("rt").With(dc);
        }

        public static ContainerTag Rt(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("rt"), shortAttr);
        }

        public static ContainerTag Rt(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("rt").WithText(text), shortAttr);
        }

        public static ContainerTag Rt(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("rt").With(dc), shortAttr);
        }

        public static ContainerTag Ruby()
        {
            return new ContainerTag("ruby");
        }

        public static ContainerTag Ruby(string text)
        {
            return new ContainerTag("ruby").WithText(text);
        }

        public static ContainerTag Ruby(params DomContent[] dc)
        {
            return new ContainerTag("ruby").With(dc);
        }

        public static ContainerTag Ruby(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("ruby"), shortAttr);
        }

        public static ContainerTag Ruby(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("ruby").WithText(text), shortAttr);
        }

        public static ContainerTag Ruby(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("ruby").With(dc), shortAttr);
        }

        public static ContainerTag S()
        {
            return new ContainerTag("s");
        }

        public static ContainerTag S(string text)
        {
            return new ContainerTag("s").WithText(text);
        }

        public static ContainerTag S(params DomContent[] dc)
        {
            return new ContainerTag("s").With(dc);
        }

        public static ContainerTag S(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("s"), shortAttr);
        }

        public static ContainerTag S(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("s").WithText(text), shortAttr);
        }

        public static ContainerTag S(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("s").With(dc), shortAttr);
        }

        public static ContainerTag Samp()
        {
            return new ContainerTag("samp");
        }

        public static ContainerTag Samp(string text)
        {
            return new ContainerTag("samp").WithText(text);
        }

        public static ContainerTag Samp(params DomContent[] dc)
        {
            return new ContainerTag("samp").With(dc);
        }

        public static ContainerTag Samp(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("samp"), shortAttr);
        }

        public static ContainerTag Samp(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("samp").WithText(text), shortAttr);
        }

        public static ContainerTag Samp(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("samp").With(dc), shortAttr);
        }

        public static ContainerTag Script()
        {
            return new ContainerTag("script");
        }

        public static ContainerTag Script(string text)
        {
            return new ContainerTag("script").WithText(text);
        }

        public static ContainerTag Script(params DomContent[] dc)
        {
            return new ContainerTag("script").With(dc);
        }

        public static ContainerTag Script(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("script"), shortAttr);
        }

        public static ContainerTag Script(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("script").WithText(text), shortAttr);
        }

        public static ContainerTag Script(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("script").With(dc), shortAttr);
        }

        public static ContainerTag Section()
        {
            return new ContainerTag("section");
        }

        public static ContainerTag Section(string text)
        {
            return new ContainerTag("section").WithText(text);
        }

        public static ContainerTag Section(params DomContent[] dc)
        {
            return new ContainerTag("section").With(dc);
        }

        public static ContainerTag Section(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("section"), shortAttr);
        }

        public static ContainerTag Section(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("section").WithText(text), shortAttr);
        }

        public static ContainerTag Section(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("section").With(dc), shortAttr);
        }

        public static ContainerTag Select()
        {
            return new ContainerTag("select");
        }

        public static ContainerTag Select(string text)
        {
            return new ContainerTag("select").WithText(text);
        }

        public static ContainerTag Select(params DomContent[] dc)
        {
            return new ContainerTag("select").With(dc);
        }

        public static ContainerTag Select(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("select"), shortAttr);
        }

        public static ContainerTag Select(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("select").WithText(text), shortAttr);
        }

        public static ContainerTag Select(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("select").With(dc), shortAttr);
        }

        public static ContainerTag Small()
        {
            return new ContainerTag("small");
        }

        public static ContainerTag Small(string text)
        {
            return new ContainerTag("small").WithText(text);
        }

        public static ContainerTag Small(params DomContent[] dc)
        {
            return new ContainerTag("small").With(dc);
        }

        public static ContainerTag Small(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("small"), shortAttr);
        }

        public static ContainerTag Small(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("small").WithText(text), shortAttr);
        }

        public static ContainerTag Small(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("small").With(dc), shortAttr);
        }

        public static ContainerTag Span()
        {
            return new ContainerTag("span");
        }

        public static ContainerTag Span(string text)
        {
            return new ContainerTag("span").WithText(text);
        }

        public static ContainerTag Span(params DomContent[] dc)
        {
            return new ContainerTag("span").With(dc);
        }

        public static ContainerTag Span(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("span"), shortAttr);
        }

        public static ContainerTag Span(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("span").WithText(text), shortAttr);
        }

        public static ContainerTag Span(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("span").With(dc), shortAttr);
        }

        public static ContainerTag Strong()
        {
            return new ContainerTag("strong");
        }

        public static ContainerTag Strong(string text)
        {
            return new ContainerTag("strong").WithText(text);
        }

        public static ContainerTag Strong(params DomContent[] dc)
        {
            return new ContainerTag("strong").With(dc);
        }

        public static ContainerTag Strong(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("strong"), shortAttr);
        }

        public static ContainerTag Strong(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("strong").WithText(text), shortAttr);
        }

        public static ContainerTag Strong(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("strong").With(dc), shortAttr);
        }

        public static ContainerTag Style()
        {
            return new ContainerTag("style");
        }

        public static ContainerTag Style(string text)
        {
            return new ContainerTag("style").WithText(text);
        }

        public static ContainerTag Style(params DomContent[] dc)
        {
            return new ContainerTag("style").With(dc);
        }

        public static ContainerTag Style(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("style"), shortAttr);
        }

        public static ContainerTag Style(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("style").WithText(text), shortAttr);
        }

        public static ContainerTag Style(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("style").With(dc), shortAttr);
        }

        public static ContainerTag Sub()
        {
            return new ContainerTag("sub");
        }

        public static ContainerTag Sub(string text)
        {
            return new ContainerTag("sub").WithText(text);
        }

        public static ContainerTag Sub(params DomContent[] dc)
        {
            return new ContainerTag("sub").With(dc);
        }

        public static ContainerTag Sub(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("sub"), shortAttr);
        }

        public static ContainerTag Sub(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("sub").WithText(text), shortAttr);
        }

        public static ContainerTag Sub(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("sub").With(dc), shortAttr);
        }

        public static ContainerTag Summary()
        {
            return new ContainerTag("summary");
        }

        public static ContainerTag Summary(string text)
        {
            return new ContainerTag("summary").WithText(text);
        }

        public static ContainerTag Summary(params DomContent[] dc)
        {
            return new ContainerTag("summary").With(dc);
        }

        public static ContainerTag Summary(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("summary"), shortAttr);
        }

        public static ContainerTag Summary(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("summary").WithText(text), shortAttr);
        }

        public static ContainerTag Summary(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("summary").With(dc), shortAttr);
        }

        public static ContainerTag Sup()
        {
            return new ContainerTag("sup");
        }

        public static ContainerTag Sup(string text)
        {
            return new ContainerTag("sup").WithText(text);
        }

        public static ContainerTag Sup(params DomContent[] dc)
        {
            return new ContainerTag("sup").With(dc);
        }

        public static ContainerTag Sup(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("sup"), shortAttr);
        }

        public static ContainerTag Sup(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("sup").WithText(text), shortAttr);
        }

        public static ContainerTag Sup(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("sup").With(dc), shortAttr);
        }

        public static ContainerTag Table()
        {
            return new ContainerTag("table");
        }

        public static ContainerTag Table(string text)
        {
            return new ContainerTag("table").WithText(text);
        }

        public static ContainerTag Table(params DomContent[] dc)
        {
            return new ContainerTag("table").With(dc);
        }

        public static ContainerTag Table(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("table"), shortAttr);
        }

        public static ContainerTag Table(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("table").WithText(text), shortAttr);
        }

        public static ContainerTag Table(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("table").With(dc), shortAttr);
        }

        public static ContainerTag Tbody()
        {
            return new ContainerTag("tbody");
        }

        public static ContainerTag Tbody(string text)
        {
            return new ContainerTag("tbody").WithText(text);
        }

        public static ContainerTag Tbody(params DomContent[] dc)
        {
            return new ContainerTag("tbody").With(dc);
        }

        public static ContainerTag Tbody(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("tbody"), shortAttr);
        }

        public static ContainerTag Tbody(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("tbody").WithText(text), shortAttr);
        }

        public static ContainerTag Tbody(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("tbody").With(dc), shortAttr);
        }

        public static ContainerTag Td()
        {
            return new ContainerTag("td");
        }

        public static ContainerTag Td(string text)
        {
            return new ContainerTag("td").WithText(text);
        }

        public static ContainerTag Td(params DomContent[] dc)
        {
            return new ContainerTag("td").With(dc);
        }

        public static ContainerTag Td(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("td"), shortAttr);
        }

        public static ContainerTag Td(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("td").WithText(text), shortAttr);
        }

        public static ContainerTag Td(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("td").With(dc), shortAttr);
        }

        public static ContainerTag TextArea()
        {
            return new ContainerTag("textarea");
        }

        public static ContainerTag TextArea(string text)
        {
            return new ContainerTag("textarea").WithText(text);
        }

        public static ContainerTag TextArea(params DomContent[] dc)
        {
            return new ContainerTag("textarea").With(dc);
        }

        public static ContainerTag TextArea(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("textarea"), shortAttr);
        }

        public static ContainerTag TextArea(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("textarea").WithText(text), shortAttr);
        }

        public static ContainerTag TextArea(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("textarea").With(dc), shortAttr);
        }

        public static ContainerTag Tfoot()
        {
            return new ContainerTag("tfoot");
        }

        public static ContainerTag Tfoot(string text)
        {
            return new ContainerTag("tfoot").WithText(text);
        }

        public static ContainerTag Tfoot(params DomContent[] dc)
        {
            return new ContainerTag("tfoot").With(dc);
        }

        public static ContainerTag Tfoot(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("tfoot"), shortAttr);
        }

        public static ContainerTag Tfoot(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("tfoot").WithText(text), shortAttr);
        }

        public static ContainerTag Tfoot(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("tfoot").With(dc), shortAttr);
        }

        public static ContainerTag Th()
        {
            return new ContainerTag("th");
        }

        public static ContainerTag Th(string text)
        {
            return new ContainerTag("th").WithText(text);
        }

        public static ContainerTag Th(params DomContent[] dc)
        {
            return new ContainerTag("th").With(dc);
        }

        public static ContainerTag Th(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("th"), shortAttr);
        }

        public static ContainerTag Th(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("th").WithText(text), shortAttr);
        }

        public static ContainerTag Th(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("th").With(dc), shortAttr);
        }

        public static ContainerTag Thead()
        {
            return new ContainerTag("thead");
        }

        public static ContainerTag Thead(string text)
        {
            return new ContainerTag("thead").WithText(text);
        }

        public static ContainerTag Thead(params DomContent[] dc)
        {
            return new ContainerTag("thead").With(dc);
        }

        public static ContainerTag Thead(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("thead"), shortAttr);
        }

        public static ContainerTag Thead(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("thead").WithText(text), shortAttr);
        }

        public static ContainerTag Thead(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("thead").With(dc), shortAttr);
        }

        public static ContainerTag Time()
        {
            return new ContainerTag("time");
        }

        public static ContainerTag Time(string text)
        {
            return new ContainerTag("time").WithText(text);
        }

        public static ContainerTag Time(params DomContent[] dc)
        {
            return new ContainerTag("time").With(dc);
        }

        public static ContainerTag Time(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("time"), shortAttr);
        }

        public static ContainerTag Time(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("time").WithText(text), shortAttr);
        }

        public static ContainerTag Time(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("time").With(dc), shortAttr);
        }

        public static ContainerTag Title()
        {
            return new ContainerTag("title");
        }

        public static ContainerTag Title(string text)
        {
            return new ContainerTag("title").WithText(text);
        }

        public static ContainerTag Title(params DomContent[] dc)
        {
            return new ContainerTag("title").With(dc);
        }

        public static ContainerTag Title(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("title"), shortAttr);
        }

        public static ContainerTag Title(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("title").WithText(text), shortAttr);
        }

        public static ContainerTag Title(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("title").With(dc), shortAttr);
        }

        public static ContainerTag Tr()
        {
            return new ContainerTag("tr");
        }

        public static ContainerTag Tr(string text)
        {
            return new ContainerTag("tr").WithText(text);
        }

        public static ContainerTag Tr(params DomContent[] dc)
        {
            return new ContainerTag("tr").With(dc);
        }

        public static ContainerTag Tr(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("tr"), shortAttr);
        }

        public static ContainerTag Tr(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("tr").WithText(text), shortAttr);
        }

        public static ContainerTag Tr(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("tr").With(dc), shortAttr);
        }

        public static ContainerTag U()
        {
            return new ContainerTag("u");
        }

        public static ContainerTag U(string text)
        {
            return new ContainerTag("u").WithText(text);
        }

        public static ContainerTag U(params DomContent[] dc)
        {
            return new ContainerTag("u").With(dc);
        }

        public static ContainerTag U(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("u"), shortAttr);
        }

        public static ContainerTag U(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("u").WithText(text), shortAttr);
        }

        public static ContainerTag U(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("u").With(dc), shortAttr);
        }

        public static ContainerTag Ul()
        {
            return new ContainerTag("ul");
        }

        public static ContainerTag Ul(string text)
        {
            return new ContainerTag("ul").WithText(text);
        }

        public static ContainerTag Ul(params DomContent[] dc)
        {
            return new ContainerTag("ul").With(dc);
        }

        public static ContainerTag Ul(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("ul"), shortAttr);
        }

        public static ContainerTag Ul(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("ul").WithText(text), shortAttr);
        }

        public static ContainerTag Ul(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("ul").With(dc), shortAttr);
        }

        public static ContainerTag Var()
        {
            return new ContainerTag("var");
        }

        public static ContainerTag Var(string text)
        {
            return new ContainerTag("var").WithText(text);
        }

        public static ContainerTag Var(params DomContent[] dc)
        {
            return new ContainerTag("var").With(dc);
        }

        public static ContainerTag Var(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("var"), shortAttr);
        }

        public static ContainerTag Var(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("var").WithText(text), shortAttr);
        }

        public static ContainerTag Var(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("var").With(dc), shortAttr);
        }

        public static ContainerTag Video()
        {
            return new ContainerTag("video");
        }

        public static ContainerTag Video(string text)
        {
            return new ContainerTag("video").WithText(text);
        }

        public static ContainerTag Video(params DomContent[] dc)
        {
            return new ContainerTag("video").With(dc);
        }

        public static ContainerTag Video(Attr.ShortForm shortAttr)
        {
            return Attr.AddTo(new ContainerTag("video"), shortAttr);
        }

        public static ContainerTag Video(Attr.ShortForm shortAttr, string text)
        {
            return Attr.AddTo(new ContainerTag("video").WithText(text), shortAttr);
        }

        public static ContainerTag Video(Attr.ShortForm shortAttr, params DomContent[] dc)
        {
            return Attr.AddTo(new ContainerTag("video").With(dc), shortAttr);
        }

    }
}
