
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class DoubleLineHeading : IMarkdownParser
    {
        private static Regex regexDoubleLineHeading = new Regex(
            @"^=+$|^-+$"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            bool isDoubleLineHeading = false;
            if (input.Lines().Count > 1)
            {
                isDoubleLineHeading = regexDoubleLineHeading.Match(input.Lines()[1]).Success;
            }
            return isDoubleLineHeading;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
            ParseResult result = new ParseResult();
            ElementType headingLevel;
            if (lines[1].StartsWith("="))
            {
                headingLevel = ElementType.Heading1Underlined;
            } else {
                headingLevel = ElementType.Heading2Underlined;
            }
            Element element = new ElementFactory().New(
                headingLevel,
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        lines[0]
                    )
                )                
            );
            lines[0] = "";
            lines[1] = "";
            result.Success = true;
            result.AddContent(
                element
            );
            return result;
        }

        private ElementType level(
            string line
        ) {
            // Maximum level -> 6
            int level = 0;
            while (
                (level < 6)
                 && (line[level] == '#')
            )
            {
                level++;
            }
            switch (level)
            {
                case 1:
                    return ElementType.Heading1;
                case 2:
                    return ElementType.Heading2;
                case 3:
                    return ElementType.Heading3;
                case 4:
                    return ElementType.Heading4;
                case 5:
                    return ElementType.Heading5;
                case 6:
                    return ElementType.Heading6;
                default:
                    return ElementType.Paragraph;
            }
        }
    }
}