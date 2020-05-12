
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownEmphasis : IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^\*.*\*.*"
            + @"|^_.*_.*s"
        );

        IHtmlable[] content;

        const string tag = "em";

        public const MarkdownElementType Type = MarkdownElementType.Emphasis;

        public MarkdownEmphasis(
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

        static bool CanParseFrom(
            string line
        ) {
            return regexParseable.Match(line).Success;
        }

        static ParseResult ParseFrom(
            string line
        ) {
            if (!CanParseFrom(line))
            {
                // Return a failed result if cannot parse from this line
                ParseResult result = new ParseResult();
                result.Line = line;
                return result;
            }
            // Otherwise, attempt to parse and return result
            if (line.StartsWith("*"))
            {
                return ParseEmphasisSection(
                    line,
                    '*'
                );
            } else {
                return ParseEmphasisSection(
                    line,
                    '_'
                );
            }
        }

        // Shared code for parsing emphasis sections
        private static ParseResult ParseEmphasisSection(
            string line,
            char delimiter
        ) {
            ParseResult result = new ParseResult();
            int j = 1;
            // Find closing star
            while (
                (j < line.Length)
                && !(
                    (line[j] == delimiter)
                    && (line[j-1] != '\\')
                )
            ) {
                j++;
            }
            if (j >= line.Length)
            {
                result.Line = line;
                return result;
            }
            // Parse everything inside the stars
            MarkdownEmphasis element = new MarkdownEmphasis(
                MarkdownParser.ParseInnerText(
                    line.Substring(1, j - 1)
                )
            );
            result.AddContent(element);
            result.Line = line.Substring(j + 1);
            result.Success = true;
            return result;
        }

    }
}