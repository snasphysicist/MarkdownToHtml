
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownHorizontalRule : MarkdownElementBase, IHtmlable
    {

        static Regex regexHorizontalRule = new Regex(
            @"^[\s|\*]{3,}$"
            + @"|^[\s|-]{3,}$"
        );

        public MarkdownHorizontalRule() 
        {
            Type = MarkdownElementType.HorizontalRule;
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            // Check if the format is correct (* or - plus whitespace only)
            bool correctFormat = regexHorizontalRule.Match(line).Success;
            // Check there are enough - or * characters (3++)
            bool enoughNonWhitespace = (
                (line.Length - line.Replace("*", "").Replace("-", "").Length)
                > 2
            );
            return correctFormat && enoughNonWhitespace;
        }

        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            input.FirstLine = "";
            result.Success = true;
            result.AddContent(
                new MarkdownHorizontalRule()
            );
            return result;
        }

    }
}
