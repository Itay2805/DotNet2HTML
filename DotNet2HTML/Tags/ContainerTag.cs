using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Tags
{
    public class ContainerTag : Tag<ContainerTag>
    {

        public List<DomContent> Children { get; } = new List<DomContent>();

        public int ChildrenCount { get => Children.Count; }

        public ContainerTag(string tagName)
            : base(tagName)
        {

        }

        /// <summary>
        /// Appends a <see cref="DomContent"/> to the end of this element
        /// </summary>
        /// <param name="child"><see cref="DomContent"/> to be appended</param>
        /// <returns>itself for easy chaining</returns>
        public ContainerTag With(DomContent child)
        {
            if (this == child)
            {
                throw new Exception("Cannot append a tag to itself.");
            }
            if (child == null)
            {
                return this;
            }
            Children.Add(child);
            return this;
        }

        /// <summary>
        /// Call <see cref="With(DomContent)"/> based on condition
        /// </summary>
        /// <param name="condition">the condition to use</param>
        /// <param name="child"><see cref="DomContent"/> to be appended if condition met</param>
        /// <returns>itself for easy chaining</returns>
        public ContainerTag CondWith(bool condition, DomContent child)
        {
            return condition ? With(child) : this;
        }

        /// <summary>
        /// Appends a list of DomContent-objects to the end of this element
        /// </summary>
        /// <param name="children"><see cref="DomContent"/>s to be appended</param>
        /// <returns>for easy chaining</returns>
        public ContainerTag With(IEnumerable<DomContent> children)
        {
            if (children != null)
            {
                var enumorator = children.GetEnumerator();
                while (enumorator.MoveNext())
                {
                    With(enumorator.Current);
                }
            }
            return this;
        }

        /// <summary>
        /// Appends the <see cref="DomContent"/>s to the end of this element
        /// </summary>
        /// <param name="children"><see cref="DomContent"/>s to be appended</param>
        /// <returns>itself for easy chaining</returns>
        public ContainerTag With(params DomContent[] children)
        {
            foreach (DomContent child in children)
            {
                With(child);
            }
            return this;
        }

        /// <summary>
        /// Call <see cref="With(DomContent[])"/> based on condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public ContainerTag CondWith(bool condition, params DomContent[] children)
        {
            return condition ? With(children) : this;
        }

        /// <summary>
        /// Appends a <see cref="Text"/> to this element
        /// </summary>
        /// <param name="text">the text to be appended</param>
        /// <returns>itself for easy chaining</returns>
        public ContainerTag WithText(string text)
        {
            return With(new Text(text));
        }

        /// <summary>
        /// Render the ContainerTag and its children, adding newlines before each
        /// child and using <see cref="Config.Identer"/> to indent child based on how deep
        /// in the tree it is
        /// </summary>
        /// <returns>the rendered and formatted string</returns>
        public string RenderFormatted()
        {
            return RenderFormatted(0);
        }

        private string RenderFormatted(int lvl)
        {
            StringBuilder sb = new StringBuilder();
            RenderOpenTag(sb, null);
            if (HasTagName() && !IsSelfFormattingTag())
            {
                sb.Append("\n");
            }
            if (ChildrenCount != 0)
            {
                foreach (DomContent c in Children)
                {
                    lvl++;
                    if (c is ContainerTag ctag)
                    {
                        if (ctag.HasTagName())
                        {
                            sb.Append(Config.Identer(lvl, ctag.RenderFormatted(lvl)));
                        }
                        else
                        {
                            sb.Append(Config.Identer(lvl - 1, ctag.RenderFormatted(lvl - 1)));
                        }
                    }
                    else if (IsSelfFormattingTag())
                    {
                        sb.Append(Config.Identer(0, c.Render()));
                    }
                    else
                    {
                        sb.Append(Config.Identer(lvl, c.Render())).Append("\n");
                    }
                    lvl--;
                }
            }
            if (!IsSelfFormattingTag())
            {
                sb.Append(Config.Identer(lvl, ""));
            }
            RenderCloseTag(sb);
            if (HasTagName())
            {
                sb.Append("\n");
            }
            return sb.ToString();
        }

        private bool IsSelfFormattingTag()
        {
            return TagName == "textarea" || "pre" == TagName;
        }

        public override void RenderModel(StringBuilder writer, object model)
        {
            RenderOpenTag(writer, model);
            if (Children != null && ChildrenCount != 0)
            {
                foreach (DomContent child in Children)
                {
                    child.RenderModel(writer, model);
                }
            }
            RenderCloseTag(writer);
        }

    }
}
