
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class ElementFactory
    {
        public ElementFactory() {}
        
        public Element New(
            ElementType type
        ) {
            ElementDetails details = new ElementDetails(
                type
            );
            return new Element(
                details,
                new TagOnlyWriter()
            );
        }

        public Element New(
            ElementType type,
            IHtmlable[] content
        ) {
            ElementDetails details = new ElementDetails(
                type,
                content
            );
            return new Element(
                details,
                new TagContentWriter()
            );
        }

        public Element New(
            ElementType type,
            IHtmlable content
        )
        {
            return New(
                type,
                new IHtmlable[]
                {
                    content
                }
            );
        }

        public Element New(
            ElementType type,
            Attribute[] attributes
        ) {
            ElementDetails details = new ElementDetails(
                type,
                attributes
            );
            return new Element(
                details,
                new TagAttributesWriter()
            );
        }

        public Element New(
            ElementType type,
            IHtmlable[] content,
            Attribute[] attributes
        ) {
            ElementDetails details = new ElementDetails(
                type,
                content,
                attributes
            );
            return new Element(
                details,
                new TagContentAttributesWriter()
            );
        }
    }
}