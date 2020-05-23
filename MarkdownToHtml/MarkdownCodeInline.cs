
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownCodeInline : IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^`.*([^`])`.*"
        );

        IHtmlable[] content;

        const string tag = "code";

        public const MarkdownElementType Type = MarkdownElementType.CodeInline;

        public MarkdownCodeInline(
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
            ParseInput input
        ) {
            return regexParseable.Match(input.FirstLine).Success;
        }

        // Shared code for parsing emphasis sections
        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            ParseResult result = new ParseResult();
            if (!CanParseFrom(input))
            {
                // Fail immediately if this string cannot be parsed
                result.Line = line;
                return result;
            }
            int j = 1;
            // Find closing `
            while (
                (j < line.Length)
                && !(
                    (line[j] == '`')
                    && (line[j-1] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // If we cannot parse, then return line as is
                result.Line = line;
                return result;
            }
            // Parse everything inside the backticks
            MarkdownCodeInline element = new MarkdownCodeInline(
                MarkdownParser.ParseInnerText(
                    new ParseInput(
                        input,
                        line.Substring(1, j - 1)
                    )
                )
            );
            result.AddContent(element);
            result.Line = line.Substring(j + 1);
            result.Success = true;
            return result;
        }

    }
}
