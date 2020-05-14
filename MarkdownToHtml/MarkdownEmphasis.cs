
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownEmphasis : IHtmlable
    {

        private static Regex regexParseable = new Regex(
            @"^\*(.*?[^\\])\*"
            + @"|^_(.*?[^\\])_"
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

        public static bool CanParseFrom(
            string line
        ) {
            return regexParseable.Match(line).Success;
        }

        public static ParseResult ParseFrom(
            string line
        ) {
            ParseResult result = new ParseResult();
            if (!CanParseFrom(line))
            {
                // Return a failed result if cannot parse from this line
                result.Line = line;
                return result;
            }
            // Otherwise parse and return result
            Match contentMatch = regexParseable.Match(line);
            string content;
            if (contentMatch.Groups[1].Value.Length != 0)
            {
                content = contentMatch.Groups[1].Value;
            } else {
                content = contentMatch.Groups[2].Value;
            }
            // Parse everything inside the stars
            MarkdownEmphasis element = new MarkdownEmphasis(
                MarkdownParser.ParseInnerText(
                    content
                )
            );
            result.AddContent(element);
            result.Line = line.Substring(
                content.Length + 2
            );
            result.Success = true;
            return result;
        }

    }
}