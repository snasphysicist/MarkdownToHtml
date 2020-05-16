
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownHeading : IHtmlable
    {

        private static Regex regexSingleLineHeading = new Regex(
            @"^#{1,6}(.+)?#*"
        );

        private static Regex regexDoubleLineHeading = new Regex(
            @"^=+$|^-+$"
        );

        IHtmlable[] content;

        string tag;

        public MarkdownElementType Type
        { get; private set; }
        
        public MarkdownHeading(
            int level,
            IHtmlable[] content
        ) {
            this.content = content;
            if ((level > 0) && (level < 7))
            {
                this.tag = $"h{level}";
            } else {
                // If not a valid heading level, fall back to paragraph
                this.tag = "p";
            }
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

        public string ToHtml() 
        {
            string html = $"<{tag}>";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
        }

        public static bool CanParseFrom(
            ArraySegment<string> lines
        ) {
            return (
                CanParseSingleLineHeading(lines)
                || CanParseDoubleLineHeading(lines)
            );
        }

        private static bool CanParseSingleLineHeading(
            ArraySegment<string> lines
        ) {
            return regexSingleLineHeading.Match(lines[0]).Success;
        }

        private static bool CanParseDoubleLineHeading(
            ArraySegment<string> lines
        ) {
            bool isDoubleLineHeading = false;
            if (lines.Count > 1)
            {
                isDoubleLineHeading = regexDoubleLineHeading.Match(lines[1]).Success;
            }
            return isDoubleLineHeading;
        }

        public static ParseResult ParseFrom(
            ArraySegment<string> lines
        ) {
            if (CanParseSingleLineHeading(lines))
            {
                return ParseSingleLineHeading(lines);
            } else if (CanParseDoubleLineHeading(lines)) 
            {
                return ParseDoubleLineHeading(lines);
            } else {
                return new ParseResult();
            }
        }

        private static ParseResult ParseSingleLineHeading(
            ArraySegment<string> lines
        ) {
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
                        content
                    )
                )
            );
            return result;
        }

        private static ParseResult ParseDoubleLineHeading(
            ArraySegment<string> lines
        ) {
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
                    lines[0]
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
