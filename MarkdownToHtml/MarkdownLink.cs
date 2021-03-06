
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownLink : MarkdownElementFull, IHtmlable
    {

        private static Regex regexLinkImmediate = new Regex(
            @"^\[(.*[^\\])\]\((.*[^\\])\)"
        );

        private static Regex regexLinkReference = new Regex(
            @"^\[(.*[^\\])\]\[(.*[^\\])\]"
        );

        private static Regex regexLinkSelfReference = new Regex(
            @"^\[(.*[^\\])\]"
        );

        public MarkdownLink(
            IHtmlable[] content,
            string href
        ) {
            Type = MarkdownElementType.Link;
            this.content = content;
            attributes = new Dictionary<string, string>();
            attributes.Add(
                "href",
                href
            );
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            ReferencedUrl[] urls = input.Urls;
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

        public static ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            ReferencedUrl[] urls = input.Urls;
            ParseResult result = new ParseResult();
            if (
                !CanParseFrom(input)
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
                            new ParseInput(
                                input,
                                text
                            )
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
                                    new ParseInput(
                                        input,
                                        text
                                    )
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
                                    new ParseInput(
                                        input,
                                        text
                                    )
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
