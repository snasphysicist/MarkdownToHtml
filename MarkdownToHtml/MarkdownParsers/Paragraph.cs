
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class Paragraph : IMarkdownParser
    {
        private static Element linebreak = new ElementFactory().New(
            ElementType.Linebreak
        );

        public bool CanParseFrom(
            ParseInput input
        )
        {
            return true;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            ParseResult innerContent = new MultiLineText().ParseFrom(
                input
            );
            Element paragraph = new ElementFactory().New(
                ElementType.Paragraph,
                innerContent.GetContent()
            );
            result.Success = true;
            result.AddContent(
                paragraph
            );
            return result;
        }
    }
}