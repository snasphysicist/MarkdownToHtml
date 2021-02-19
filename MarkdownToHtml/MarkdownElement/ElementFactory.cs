
namespace MarkdownToHtml
{
    public class ElementFactory
    {
        public ElementFactory() {}
        
        public Element New(
            ElementType type
        ) {
            ElementDetails details = new ElementDetails(
                type,
                NewLineStatus(type)
            );
            IHtmlWriter writer;
            if (type != ElementType.HorizontalRule)
            {
                writer = new TagOnlyWriter();
            } else 
            {
                writer = new TagOnlySelfClosingWriter();
            }
            return new Element(
                details,
                writer
            );
        }

        public Element New(
            ElementType type,
            IHtmlable[] content
        ) {
            ElementDetails details = new ElementDetails(
                type,
                NewLineStatus(type),
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
                NewLineStatus(type),
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
                NewLineStatus(type),
                content,
                attributes
            );
            return new Element(
                details,
                new TagContentAttributesWriter()
            );
        }

        private static ElementDetails.FollowWithNewLine NewLineStatus(ElementType type)
        {
            switch (type)
            {
                case ElementType.Heading1:
                case ElementType.Heading2:
                case ElementType.Heading3:
                case ElementType.Heading4:
                case ElementType.Heading5:
                case ElementType.Heading6:
                case ElementType.Heading1Underlined:
                case ElementType.Heading2Underlined:
                case ElementType.Paragraph:
                    return ElementDetails.FollowWithNewLine.Yes;
                default:
                    return ElementDetails.FollowWithNewLine.No; 
            }
        }
    }
}