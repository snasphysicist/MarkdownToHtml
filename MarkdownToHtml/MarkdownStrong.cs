
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownStrong : IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^\*{2}.+\*{2}.*"
            + @"|^_{2}.+_{2}.*"
        );

        IHtmlable[] content;

        const string tag = "strong";

        public const MarkdownElementType Type = MarkdownElementType.Strong;

        public MarkdownStrong(
            IHtmlable[] content
        ) {
            this.content = content;
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
            string line
        ) {
            return regexParseable.Match(line).Success;
        }

        public static ParseResult ParseFrom(
            string line
        ) {
            if (!CanParseFrom(line))
            {
                // Return a failed result if this cannot be parsed
                ParseResult result = new ParseResult();
                result.Line = line;
                return result;
            }
            // Otherwise, parse and return result
            if (line.StartsWith("**"))
            {
                return ParseStrongSection(
                    line,
                    "**"
                );
            } else {
                return ParseStrongSection(
                    line,
                    "__"
                );
            }
        }

        // Shared code for parsing strong sections
        private static ParseResult ParseStrongSection(
            string line,
            string delimiter
        ) {
            ParseResult result = new ParseResult();
            int j = 2;
            // Find closing two characters
            while (
                (j < line.Length)
                && !(
                    (line.Substring(j-1, 2) == delimiter)
                    && (line[j-2] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // Fail if closing characters cannot be found
                result.Line = line;
                return result;
            }
            // Parse everything inside the strong section delimiters
            MarkdownStrong element = new MarkdownStrong(
                MarkdownParser.ParseInnerText(
                    line.Substring(2, j - 3)
                )
            );
            result.AddContent(element);
            result.Line = line.Substring(j + 1);
            result.Success = true;
            // Return the line string minus the content we parsed
            return result;
        }

    }
}
