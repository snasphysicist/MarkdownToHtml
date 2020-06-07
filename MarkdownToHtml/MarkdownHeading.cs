
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownHeading : MarkdownElementWithContent, IHtmlable
    {

        private static Regex regexSingleLineHeading = new Regex(
            @"^#{1,6}(.+)?#*"
        );

        private static Regex regexDoubleLineHeading = new Regex(
            @"^=+$|^-+$"
        );
        
        public MarkdownHeading(
            int level,
            IHtmlable[] content
        ) {
            this.content = content;
            switch (level)
            {
                case 1:
                    Type = MarkdownElementType.Heading1;
                    break;
                case 2:
                    Type = MarkdownElementType.Heading2;
                    break;
                case 3:
                    Type = MarkdownElementType.Heading3;
                    break;
                case 4:
                    Type = MarkdownElementType.Heading4;
                    break;
                case 5:
                    Type = MarkdownElementType.Heading5;
                    break;
                case 6:
                    Type = MarkdownElementType.Heading6;
                    break;
                default:
                    Type = MarkdownElementType.Paragraph;
                    break;
            }
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            return (
                CanParseSingleLineHeading(input)
                || CanParseDoubleLineHeading(input)
            );
        }

        private static bool CanParseSingleLineHeading(
            ParseInput input
        ) {
            return regexSingleLineHeading.Match(input.FirstLine).Success;
        }

        private static bool CanParseDoubleLineHeading(
            ParseInput input
        ) {
            bool isDoubleLineHeading = false;
            if (input.Lines().Count > 1)
            {
                isDoubleLineHeading = regexDoubleLineHeading.Match(input.Lines()[1]).Success;
            }
            return isDoubleLineHeading;
        }

        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            if (CanParseSingleLineHeading(input))
            {
                return ParseSingleLineHeading(input);
            } else if (CanParseDoubleLineHeading(input)) 
            {
                return ParseDoubleLineHeading(input);
            } else {
                return new ParseResult();
            }
        }

        private static ParseResult ParseSingleLineHeading(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
            ParseResult result = new ParseResult();
            // Calculate heading level, (maximum 6)
            int level = 0;
            while (
                (level < 6)
                 && (lines[0][level] == '#')
            )
            {
                level++;
            }
            Match contentMatch = regexSingleLineHeading.Match(lines[0]);
            string content = StripLeadingCharacter(
                StripTrailingCharacter(
                    contentMatch.Groups[1].Value,
                    '#'
                ),
                ' '
            );
            lines[0] = "";
            result.Success = true;
            result.AddContent(
                new MarkdownHeading(
                    level,
                    MarkdownParser.ParseInnerText(
                        new ParseInput(
                            input,
                            content
                        )
                    )
                )
            );
            return result;
        }

        private static ParseResult ParseDoubleLineHeading(
            ParseInput input
        ) {
            ArraySegment<string> lines = input.Lines();
            ParseResult result = new ParseResult();
            int level;
            if (lines[1].StartsWith("="))
            {
                level = 1;
            } else {
                level = 2;
            }
            MarkdownHeading element = new MarkdownHeading(
                level,
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

        private static string StripTrailingCharacter(
            string line,
            char character
        ) {
            while (
                (line.Length > 0)
                && (line[^1] == character)
            ) {
                line = line.Substring(
                    0,
                    line.Length - 1 
                );
            }
            return line;
        }

        private static string StripLeadingCharacter(
            string line,
            char character
        ) {
            while (
                (line.Length > 0)
                && (line[0] == character)
            ) {
                line = line.Substring(1);
            }
            return line;
        }

    }
}
