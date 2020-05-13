
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownHorizontalRule : IHtmlable
    {

        static Regex regexHorizontalRule = new Regex(
            @"^[\s|\*]{3,}"
            + @"|^[\s|-]{3,}"
        );

        string tag = "hr";

        public const MarkdownElementType Type = MarkdownElementType.HorizontalRule;

        public MarkdownHorizontalRule() 
        {}

        public string ToHtml() {
            return $"<{tag}>";
        }

        public static bool CanParseFrom(
            ArraySegment<string> lines
        ) {
            // Check if the format is correct (* or - plus whitespace only)
            bool correctFormat = regexHorizontalRule.Match(lines[0]).Success;
            // Check there are enough - or * characters (3++)
            bool enoughNonWhitespace = (
                (lines[0].Length - lines[0].Replace(" ", "").Length)
                > 2
            );
            return correctFormat && enoughNonWhitespace;
        }

        public static ParseResult ParseFrom(
            ArraySegment<string> lines
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(lines))
            {
                return result;
            }
            lines[0] = "";
            result.Success = true;
            result.AddContent(
                new MarkdownHorizontalRule()
            );
            return result;
        }

    }
}
