using DotNet2HTML.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Tags
{
    public abstract class Tag<T> : DomContent
        where T : Tag<T>
    {

        public string TagName { get; protected set; }
        public List<HtmlAttribute> Attributes { get; protected set; } = new List<HtmlAttribute>();

        protected Tag(string tagName)
        {
            TagName = tagName;
        }

        protected bool HasTagName()
        {
            return !string.IsNullOrEmpty(TagName);
        }

        protected string RenderOpenTag()
        {
            StringBuilder builder = new StringBuilder();
            RenderOpenTag(builder, null);
            return builder.ToString();
        }

        protected string RenderCloseTag()
        {
            StringBuilder builder = new StringBuilder();
            RenderCloseTag(builder);
            return builder.ToString();
        }

        protected void RenderOpenTag(StringBuilder writer, object model)
        {
            if (!HasTagName())
            {
                return;
            }
            writer.Append("<").Append(TagName);
            foreach (var attribute in Attributes)
            {
                attribute.RenderModel(writer, model);
            }
            writer.Append(">");
        }

        protected void RenderCloseTag(StringBuilder writer)
        {
            if (!HasTagName())
            {
                return;
            }
            writer.Append("</");
            writer.Append(TagName);
            writer.Append(">");
        }

        /// <summary>
        /// Sets an attribute on an element
        /// </summary>
        /// <param name="name">the attribute</param>
        /// <param name="value">the attribute value</param>
        protected void SetAttribute(string name, string value)
        {
            if (value == null)
            {
                Attributes.Add(new HtmlAttribute(name));
            }
            foreach (var attribute in Attributes)
            {
                if (attribute.Name.Equals(name))
                {
                    attribute.Value = value;
                }
            }
            Attributes.Add(new HtmlAttribute(name, value));
        }

        /// <summary>
        /// Sets a custom attribute
        /// </summary>
        /// <param name="attribute">the attribute name</param>
        /// <param name="value">the attribute value</param>
        /// <returns>itself for easy chaining</returns>
        public T Attr(string attribute, object value)
        {
            SetAttribute(attribute, value?.ToString());
            return (T)this;
        }

        /// <summary>
        /// Adds the specified attribute. If the Tag previously contained an attribute with the same name, the old attribute is replaced by the specified attribute.
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <returns>itself for easy chaining</returns>
        public T Attr(HtmlAttribute attribute)
        {
            var enumerator = Attributes.GetEnumerator();
            string name = attribute.Name;
            if (name != null)
            {
                while (enumerator.MoveNext())
                {
                    HtmlAttribute existingAttribute = enumerator.Current;
                    if (existingAttribute.Name.Equals(name))
                    {
                        Attributes.Remove(existingAttribute);
                    }
                }
            }
            Attributes.Add(attribute);
            return (T)this;
        }

        /// <summary>
        /// Sets a custom attribute without value
        /// </summary>
        /// <param name="attribute">the attribute name</param>
        /// <returns>itself for easy chaining</returns>
        /// <seealso cref="Attr(string, object)"/>
        public T Attr(string attribute)
        {
            return Attr(attribute, null);
        }

        /// <summary>
        /// Call <see cref="Attr(string, object)"/> based on condition
        /// </summary>
        /// <param name="condition">the condition</param>
        /// <param name="attribute">the attribute name</param>
        /// <param name="value">the attribute value</param>
        /// <returns>itself for easy chaining</returns>
        public T CondAttr(bool condition, string attribute, object value)
        {
            return condition ? Attr(attribute, value) : (T)this;
        }

        public override int GetHashCode()
        {
            return this.Render().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Tag<T>))
            {
                return false;
            }
            return ((Tag<T>)obj).Render() == this.Render();
        }

        /// <summary>
        /// Convenience methods that call attr with predefined attributes
        /// </summary>
        /// <param name="classes"></param>
        /// <returns>itself for easy chaining</returns>
        public T WithClasses(params string[] classes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in classes)
            {
                sb.Append(s ?? "").Append(" ");
            }
            return Attr(DotNet2HTML.Attributes.Attr.CLASS, sb.ToString().Trim());
        }

        public T IsAutoComplete()
        {
            return Attr(DotNet2HTML.Attributes.Attr.AUTOCOMPLETE, null);
        }

        public T IsAutoFocus()
        {
            return Attr(DotNet2HTML.Attributes.Attr.AUTOFOCUS, null);
        }

        public T IsHidden()
        {
            return Attr(DotNet2HTML.Attributes.Attr.HIDDEN, null);
        }

        public T IsRequired()
        {
            return Attr(DotNet2HTML.Attributes.Attr.REQUIRED, null);
        }

        public T WithAlt(string alt)
        {
            return Attr(DotNet2HTML.Attributes.Attr.ALT, alt);
        }

        public T WithAction(string action)
        {
            return Attr(DotNet2HTML.Attributes.Attr.ACTION, action);
        }

        public T WithCharset(string charset)
        {
            return Attr(DotNet2HTML.Attributes.Attr.CHARSET, charset);
        }

        public T WithClass(string className)
        {
            return Attr(DotNet2HTML.Attributes.Attr.CLASS, className);
        }

        public T WithContent(string content)
        {
            return Attr(DotNet2HTML.Attributes.Attr.CONTENT, content);
        }

        public T WithDir(string dir)
        {
            return Attr(DotNet2HTML.Attributes.Attr.DIR, dir);
        }

        public T WithHref(string href)
        {
            return Attr(DotNet2HTML.Attributes.Attr.HREF, href);
        }

        public T WithId(string id)
        {
            return Attr(DotNet2HTML.Attributes.Attr.ID, id);
        }

        public T WithData(string dataAttr, string value)
        {
            return Attr(DotNet2HTML.Attributes.Attr.DATA + "-" + dataAttr, value);
        }

        public T WithLang(string lang)
        {
            return Attr(DotNet2HTML.Attributes.Attr.LANG, lang);
        }

        public T WithMethod(string method)
        {
            return Attr(DotNet2HTML.Attributes.Attr.METHOD, method);
        }

        public T WithName(string name)
        {
            return Attr(DotNet2HTML.Attributes.Attr.NAME, name);
        }

        public T WithPlaceholder(string placeholder)
        {
            return Attr(DotNet2HTML.Attributes.Attr.PLACEHOLDER, placeholder);
        }

        public T WithTarget(string target)
        {
            return Attr(DotNet2HTML.Attributes.Attr.TARGET, target);
        }

        public T WithTitle(string title)
        {
            return Attr(DotNet2HTML.Attributes.Attr.TITLE, title);
        }

        public T WithType(string type)
        {
            return Attr(DotNet2HTML.Attributes.Attr.TYPE, type);
        }

        public T WithRel(string rel)
        {
            return Attr(DotNet2HTML.Attributes.Attr.REL, rel);
        }

        public T WithRole(string role)
        {
            return Attr(DotNet2HTML.Attributes.Attr.ROLE, role);
        }

        public T WithSrc(string src)
        {
            return Attr(DotNet2HTML.Attributes.Attr.SRC, src);
        }

        public T WithStyle(string style)
        {
            return Attr(DotNet2HTML.Attributes.Attr.STYLE, style);
        }

        public T WithValue(object value)
        {
            return Attr(DotNet2HTML.Attributes.Attr.VALUE, value?.ToString());
        }

        public T WithCondAutoComplete(bool condition)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.AUTOCOMPLETE, null);
        }

        public T WithCondAutoFocus(bool condition)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.AUTOFOCUS, null);
        }

        public T WithCondHidden(bool condition)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.HIDDEN, null);
        }

        public T WithCondRequired(bool condition)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.REQUIRED, null);
        }

        public T WithCondAlt(bool condition, string alt)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.ALT, alt);
        }

        public T WithCondAction(bool condition, string action)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.ACTION, action);
        }

        public T WithCharset(bool condition, string charset)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.CHARSET, charset);
        }

        public T WithCondClass(bool condition, string className)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.CLASS, className);
        }

        public T WithCondContent(bool condition, string content)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.CONTENT, content);
        }

        public T WithCondDir(bool condition, string dir)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.DIR, dir);
        }

        public T WithCondHref(bool condition, string href)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.HREF, href);
        }

        public T WithCondId(bool condition, string id)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.ID, id);
        }

        public T WithCondData(bool condition, string dataAttr, string value)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.DATA + "-" + dataAttr, value);
        }

        public T WithCondLang(bool condition, string lang)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.LANG, lang);
        }

        public T WithCondMethod(bool condition, string method)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.METHOD, method);
        }

        public T WithCondName(bool condition, string name)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.NAME, name);
        }

        public T WithCondPlaceholder(bool condition, string placeholder)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.PLACEHOLDER, placeholder);
        }

        public T WithCondTarget(bool condition, string target)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.TARGET, target);
        }

        public T WithCondTitle(bool condition, string title)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.TITLE, title);
        }

        public T WithCondType(bool condition, string type)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.TYPE, type);
        }

        public T WithCondRel(bool condition, string rel)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.REL, rel);
        }

        public T WithCondSrc(bool condition, string src)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.SRC, src);
        }

        public T WithCondStyle(bool condition, string style)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.STYLE, style);
        }

        public T WithCondValue(bool condition, string value)
        {
            return CondAttr(condition, DotNet2HTML.Attributes.Attr.VALUE, value);
        }

    }
}
