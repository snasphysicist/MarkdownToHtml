
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class ElementDetails  
    {
        public ElementType Type
        {
            get; private set;
        }

        public string Tag
        {
            get {
                return Type.Tag();
            }
        }

        private IHtmlable[] content = new IHtmlable[]{};

        private Dictionary<string, string> attributes = new Dictionary<string, string>();

        public ElementDetails(
            ElementType type
        ) {
            Type = type;
        }

        public ElementDetails(
            ElementType type,
            IHtmlable[] content
        ) : this(type) 
        {
            this.content = content;
        }

        public ElementDetails(
            ElementType type,
            IHtmlable[] content,
            Dictionary<string, string> attributes
        ) : this(type, content)
        {
            this.attributes = attributes;
        }
    }
}