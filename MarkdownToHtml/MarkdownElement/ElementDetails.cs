
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

        private Attribute[] attributes = new Attribute[]{};

        public IEnumerable<IHtmlable> Content()
        {
            foreach (IHtmlable item in content) {
                yield return item;
            }
        }

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
            Attribute[] attributes
        ) : this(type, content)
        {
            this.attributes = attributes;
        }
    }
}