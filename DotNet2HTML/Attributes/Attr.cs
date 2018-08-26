using DotNet2HTML.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2HTML.Attributes
{
    public class Attr
    {

        public const string ACCEPT = "accept";
        public const string ACCEPT_CHARSET = "accept-charset";
        public const string ACCESSKEY = "accesskey";
        public const string ACTION = "action";
        public const string ALIGN = "align";
        public const string ALT = "alt";
        public const string ASYNC = "async";
        public const string AUTOCOMPLETE = "autocomplete";
        public const string AUTOFOCUS = "autofocus";
        public const string AUTOPLAY = "autoplay";
        public const string AUTOSAVE = "autosave";
        public const string BORDER = "border";
        public const string BUFFERED = "buffered";
        public const string CHALLENGE = "challenge";
        public const string CHARSET = "charset";
        public const string CHECKED = "checked";
        public const string CITE = "cite";
        public const string CLASS = "class";
        public const string COLOR = "color";
        public const string COLS = "cols";
        public const string COLSPAN = "colspan";
        public const string CONTENT = "content";
        public const string CONTENTEDITABLE = "contenteditable";
        public const string CONTEXTMENU = "contextmenu";
        public const string CONTROLS = "controls";
        public const string COORDS = "coords";
        public const string DATA = "data";
        public const string DATETIME = "datetime";
        public const string DEFAULT = "default";
        public const string DEFER = "defer";
        public const string DIR = "dir";
        public const string DIRNAME = "dirname";
        public const string DISABLED = "disabled";
        public const string DOWNLOAD = "download";
        public const string DRAGGABLE = "draggable";
        public const string DROPZONE = "dropzone";
        public const string ENCTYPE = "enctype";
        public const string FOR = "for";
        public const string FORM = "form";
        public const string FORMACTION = "formaction";
        public const string HEADERS = "headers";
        public const string HEIGHT = "height";
        public const string HIDDEN = "hidden";
        public const string HIGH = "high";
        public const string HREF = "href";
        public const string HREFLANG = "hreflang";
        public const string HTTP_EQUIV = "http-equiv";
        public const string ICON = "icon";
        public const string ID = "id";
        public const string ISMAP = "ismap";
        public const string ITEMPROP = "itemprop";
        public const string KEYTYPE = "keytype";
        public const string KIND = "kind";
        public const string LABEL = "label";
        public const string LANG = "lang";
        public const string LANGUAGE = "language";
        public const string LIST = "list";
        public const string LOOP = "loop";
        public const string LOW = "low";
        public const string MANIFEST = "manifest";
        public const string MAX = "max";
        public const string MAXLENGTH = "maxlength";
        public const string MEDIA = "media";
        public const string METHOD = "method";
        public const string MIN = "min";
        public const string MULTIPLE = "multiple";
        public const string NAME = "name";
        public const string NOVALIDATE = "novalidate";
        public const string OPEN = "open";
        public const string OPTIMUM = "optimum";
        public const string PATTERN = "pattern";
        public const string PING = "ping";
        public const string PLACEHOLDER = "placeholder";
        public const string POSTER = "poster";
        public const string PRELOAD = "preload";
        public const string PUBDATE = "pubdate";
        public const string RADIOGROUP = "radiogroup";
        public const string READONLY = "readonly";
        public const string REL = "rel";
        public const string REQUIRED = "required";
        public const string REVERSED = "reversed";
        public const string ROLE = "role";
        public const string ROWS = "rows";
        public const string ROWSPAN = "rowspan";
        public const string SANDBOX = "sandbox";
        public const string SCOPE = "scope";
        public const string SCOPED = "scoped";
        public const string SEAMLESS = "seamless";
        public const string SELECTED = "selected";
        public const string SHAPE = "shape";
        public const string SIZE = "size";
        public const string SIZES = "sizes";
        public const string SPAN = "span";
        public const string SPELLCHECK = "spellcheck";
        public const string SRC = "src";
        public const string SRCDOC = "srcdoc";
        public const string SRCLANG = "srclang";
        public const string SRCSET = "srcset";
        public const string START = "start";
        public const string STEP = "step";
        public const string STYLE = "style";
        public const string SUMMARY = "summary";
        public const string TABINDEX = "tabindex";
        public const string TARGET = "target";
        public const string TITLE = "title";
        public const string TYPE = "type";
        public const string USEMAP = "usemap";
        public const string VALUE = "value";
        public const string WIDTH = "width";
        public const string WRAP = "wrap";

        public class ShortForm
        {
            public string ID { get; }
            public string Classes { get; }

            public ShortForm(string id, string classes)
            {
                ID = id;
                Classes = classes;
            }

            public bool HasId()
            {
                return !string.IsNullOrEmpty(ID);
            }

            public bool HasClasses()
            {
                return !string.IsNullOrEmpty(Classes);
            }

        }

        public static ShortForm ShortFromFromAttrsString(string attrs)
        {
            if (!attrs.Contains(".") && !attrs.Contains("#"))
            {
                throw new InvalidOperationException("String must contain either id (#) or class (.)");
            }
            if (attrs.Split('#').Length > 2)
            {
                throw new InvalidOperationException("Only one id (#) allowed");
            }
            string id = "";
            StringBuilder classes = new StringBuilder();
            foreach (string attr in attrs.Split('.'))
            {
                if (attr.Contains("#"))
                {
                    if (!attr.StartsWith("#"))
                    {
                        throw new InvalidOperationException("# cannot be in the middle of string");
                    }
                    id = attr.Replace("#", "");
                }
                else
                {
                    classes.Append(attr).Append(" ");
                }
            }
            return new ShortForm(id.Trim(), classes.ToString().Trim());
        }

        public static T AddTo<T>(T tag, ShortForm shortForm)
            where T : Tag<T>
        {
            if (shortForm.HasId() && shortForm.HasClasses())
            {
                return tag.WithId(shortForm.ID).WithClass(shortForm.Classes);
            }
            if (shortForm.HasId())
            {
                return tag.WithId(shortForm.ID);
            }
            if (shortForm.HasClasses())
            {
                return tag.WithClass(shortForm.Classes);
            }
            return tag;
        }

    }
}
