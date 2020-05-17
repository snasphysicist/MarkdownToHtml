

using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownLink : IHtmlable
    {

        private static Regex regexLinkImmediate = new Regex(
            @"^\[(.*[^\\])\]\(.*[^\\])\)"
        );

        private static Regex regexLinkReference = new Regex(
            @"^\[(.*[^\\])\]\[.*[^\\])\]"
        );

        private static Regex regexLinkSelfReference = new Regex(
            @"^\[(.*[^\\])\]"
        );

        IHtmlable[] content;

        const string tag = "a";

        private string href = "";

        public const MarkdownElementType Type = MarkdownElementType.Link;

        public MarkdownLink(
            IHtmlable[] content,
            string href
        ) {
            this.content = content;
            this.href = href;
        }

        public string ToHtml() 
        {
            string html = $"<{tag} href=\"{href}\">";
            foreach (IHtmlable htmlable in content)
            {
                html += htmlable.ToHtml();
            }
            html += $"</{tag}>";
            return html;
        }

        public static bool CanParseFrom(
            string line,
            ReferencedUrl[] urls
        ) {
            if (regexLinkImmediate.Match(line).Success)
            {
                return true;
            }
            Match linkMatch = regexLinkReference.Match(line);
            if (linkMatch.Success)
            {
                string reference = linkMatch.Groups[2].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == reference)
                    {
                        return true;
                    }
                }
            }
            linkMatch = regexLinkSelfReference.Match(line);
            if (linkMatch.Success)
            {
                string reference = linkMatch.Groups[1].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == reference)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Shared code for parsing emphasis sections
        public static ParseResult ParseFrom(
            string line,
            ReferencedUrl[] urls
        ) {
            ParseResult result = new ParseResult();
            if (
                !CanParseFrom(
                    line,
                    urls
                )
            ) {
                return result;
            }
            Match linkMatch;
            // Format: [text](url)
            linkMatch = regexLinkImmediate.Match(line);
            if (linkMatch.Success)
            {
                string text = linkMatch.Groups[1].Value;
                string url = linkMatch.Groups[2].Value;
                result.Success = true;
                result.Line = regexLinkImmediate.Replace(
                    line,
                    ""
                );
                result.AddContent(
                    new MarkdownLink(
                        MarkdownParser.ParseInnerText(
                            text
                        ),
                        url
                    )
                );
            }
            // Format: [text][id]    [id]: url
            linkMatch = regexLinkReference.Match(line);
            if (linkMatch.Success)
            {
                string text = linkMatch.Groups[1].Value;
                string reference = linkMatch.Groups[2].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == reference)
                    {
                        result.Success = true;
                        result.Line = regexLinkReference.Replace(
                            line,
                            ""
                        );
                        result.AddContent(
                            new MarkdownLink(
                                MarkdownParser.ParseInnerText(
                                    text
                                ),
                                url.Url
                            )
                        );
                    }
                }
            }
            // Format: [text]   [text]: url
            linkMatch = regexLinkSelfReference.Match(line);
            if (linkMatch.Success)
            {
                string text = linkMatch.Groups[1].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == text)
                    {
                        result.Success = true;
                        result.Line = regexLinkSelfReference.Replace(
                            line,
                            ""
                        );
                        result.AddContent(
                            new MarkdownLink(
                                MarkdownParser.ParseInnerText(
                                    text
                                ),
                                url.Url
                            )
                        );
                    }
                }
            }
            return result;
        }

    }
}
