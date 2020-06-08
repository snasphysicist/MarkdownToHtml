
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public static class TypeToTag
    {
        private static Dictionary<MarkdownElementType, string> tags
            = new Dictionary<MarkdownElementType, string>()
        {
            {MarkdownElementType.Heading1, "h1"},
            {MarkdownElementType.Heading2, "h2"},
            {MarkdownElementType.Heading3, "h3"},
            {MarkdownElementType.Heading4, "h4"},
            {MarkdownElementType.Heading5, "h5"},
            {MarkdownElementType.Heading6, "h6"},
            {MarkdownElementType.Heading1Underlined, "h1"},
            {MarkdownElementType.Heading2Underlined, "h2"},
            {MarkdownElementType.Emphasis, "em"},
            {MarkdownElementType.Strong, "strong"},
            {MarkdownElementType.Strikethrough, "s"},
            {MarkdownElementType.OrderedList, "ol"},
            {MarkdownElementType.UnorderedList, "ul"},
            {MarkdownElementType.ListItem, "li"},
            {MarkdownElementType.Link, "a"},
            {MarkdownElementType.Image, "img"},
            {MarkdownElementType.CodeInline, "code"},
            {MarkdownElementType.CodeBlock, "code"},
            {MarkdownElementType.Quote, "blockquote"},
            {MarkdownElementType.HorizontalRule, "hr"},
            {MarkdownElementType.Paragraph, "p"},
            {MarkdownElementType.Text, ""},
            {MarkdownElementType.Linebreak, "br"},
            {MarkdownElementType.Preformatted, "pre"}
        };

        public static string Tag(
            this MarkdownElementType type
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