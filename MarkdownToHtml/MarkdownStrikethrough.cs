
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownStrikethrough : IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^~{2}.*~{2}.*"
        );

        IHtmlable[] content;

        const string tag = "s";

        public const MarkdownElementType Type = MarkdownElementType.Strikethrough;

        public MarkdownStrikethrough(
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

        public static ParseResult ParseStrikethroughSection(
            string line
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(line))
            {
                // Fail immediately if we cannot parse this text as strikethrough
                result.Line = line;
                return result;
            }
            int j = 2;
            // Find closing tildes
            while (
                (j < line.Length)
                && !(
                    (line.Substring(j-1, 2) == "~~")
                    && (line[j-2] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                // Fail if we cannot find the closing squiggles
                result.Line = line;
                return result;
            }
            // Parse everything inside the stars
            MarkdownStrikethrough element = new MarkdownStrikethrough(
                MarkdownParser.ParseInnerText(
                    line.Substring(2, j - 3)
                )
            );
            result.AddContent(element);
            result.Line = line.Substring(j + 1);
            result.Success = true;
            return result;
        }

    }
}
