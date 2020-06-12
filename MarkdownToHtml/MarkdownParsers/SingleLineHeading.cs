
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class SingleLineHeading : IMarkdownParser
    {
        private static Regex regexSingleLineHeading = new Regex(
            @"^#{1,6}(.+)?#*"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            return regexSingleLineHeading.Match(input.FirstLine).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            ArraySegment<string> lines = input.Lines();
            ElementType headingLevel = level(
                lines[0]
            );
            Match contentMatch = regexSingleLineHeading.Match(lines[0]);
            string content = Utils.StripLeadingCharacter(
                Utils.StripTrailingCharacter(
                    contentMatch.Groups[1].Value,
                    '#'
                ),
                ' '
            );
            lines[0] = "";
            result.Success = true;
            Element element = new ElementFactory().New(
                headingLevel,
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        content
                    )
                )
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