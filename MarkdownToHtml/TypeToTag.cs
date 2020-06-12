
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public static class TypeToTag
    {
        private static Dictionary<ElementType, string> tags
            = new Dictionary<ElementType, string>()
        {
            {ElementType.Heading1, "h1"},
            {ElementType.Heading2, "h2"},
            {ElementType.Heading3, "h3"},
            {ElementType.Heading4, "h4"},
            {ElementType.Heading5, "h5"},
            {ElementType.Heading6, "h6"},
            {ElementType.Heading1Underlined, "h1"},
            {ElementType.Heading2Underlined, "h2"},
            {ElementType.Emphasis, "em"},
            {ElementType.Strong, "strong"},
            {ElementType.Strikethrough, "s"},
            {ElementType.OrderedList, "ol"},
            {ElementType.UnorderedList, "ul"},
            {ElementType.ListItem, "li"},
            {ElementType.Link, "a"},
            {ElementType.Image, "img"},
            {ElementType.CodeInline, "code"},
            {ElementType.CodeBlock, "code"},
            {ElementType.Quote, "blockquote"},
            {ElementType.HorizontalRule, "hr"},
            {ElementType.Paragraph, "p"},
            {ElementType.Text, ""},
            {ElementType.Linebreak, "br"},
            {ElementType.Preformatted, "pre"}
        };

        public static string Tag(
            this ElementType type
        ) {
            string tag = tags[
                type
            ];
            if (tag != null)
            {
                return tag;
            } else {
                return "";
            }
        }
    }
}