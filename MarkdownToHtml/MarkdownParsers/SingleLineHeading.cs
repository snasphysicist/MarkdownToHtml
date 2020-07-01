
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
            return regexSingleLineHeading.Match(input[0].Text).Success;
        }

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                return result;
            }
            ElementType headingLevel = level(
                input[0].Text
            );
            Match contentMatch = regexSingleLineHeading.Match(input[0].Text);
            input[0].Text = contentMatch.Groups[1].Value.StripTrailingCharacters(
                '#'
            ).StripLeadingCharacters(
                ' '
            );
            Element element = new ElementFactory().New(
                headingLevel,
                MarkdownParser.ParseInnerText(
                    input
                )
            );
            result.Success = true;
            result.AddContent(
                element
            );
            input[0].WasParsed();
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