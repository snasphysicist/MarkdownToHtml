
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class ElementDetails  
    {
        public enum FollowWithNewLine {
            Yes,
            No
        };

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

        public FollowWithNewLine NewLine {
            get; private set;
        }

        private IHtmlable[] content = new IHtmlable[]{};

        private Attribute[] attributes = new Attribute[]{};

        public IEnumerable<IHtmlable> Content()
        {
            foreach (IHtmlable item in content) 
            {
                yield return item;
            }
        }

        public IEnumerable<Attribute> Attributes()
        {
            foreach (Attribute item in attributes)
            {
                yield return item;
            }
        }

        public ElementDetails(
            ElementType type,
            FollowWithNewLine newLine
        ) {
            Type = type;
            NewLine = newLine;
        }

        public ElementDetails(
            ElementType type,
            FollowWithNewLine newLine,
            IHtmlable[] content
        ) : this(type, newLine) 
        {
            this.content = content;
        }

        public ElementDetails(
            ElementType type,
            FollowWithNewLine newLine,
            Attribute[] attributes
        ) : this(type, newLine) 
        {
            this.attributes = attributes;
        }

        public ElementDetails(
            ElementType type,
            FollowWithNewLine newLine,
            IHtmlable[] content,
            Attribute[] attributes
        ) : this(type, newLine, content)
        {
            this.attributes = attributes;
        }
    }
}