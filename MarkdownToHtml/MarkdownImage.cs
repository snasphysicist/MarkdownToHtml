
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class MarkdownImage : MarkdownElementWithAttributes, IHtmlable
    {

        private static Regex regexImageImmediateNoTitle = new Regex(
            @"^!\[(.*[^\\])\]\((\S*[^\\])\)"
        );

        private static Regex regexImageImmediateWithTitle = new Regex(
            @"^!\[(.*[^\\])\]\((.*?[^\\])\s+""(.+)""\s*\)"
        );

        private static Regex regexImageReference = new Regex(
            @"^!\[(.*[^\\])\]\[(.*[^\\])\]"
        );

        private string tag;
        public MarkdownImage(
            string href,
            string altText,
            string title
        ) {
            Type = MarkdownElementType.Image;
            tag = Type.Tag();
            this.attributes = new Dictionary<string, string>();
            attributes.Add(
                "src",
                href
            );
            attributes.Add(
                "alt",
                altText
            );
            attributes.Add(
                "title",
                title
            );
        }

        public static bool CanParseFrom(
            ParseInput input
        ) {
            string line = input.FirstLine;
            ReferencedUrl[] urls = input.Urls;
            if (
                regexImageImmediateNoTitle.Match(line).Success
                || regexImageImmediateWithTitle.Match(line).Success
            )
            {
                return true;
            }
            Match linkMatch = regexImageReference.Match(line);
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
            // Format: ![text](url)
            linkMatch = regexImageImmediateNoTitle.Match(line);
            if (linkMatch.Success)
            {
                string text = linkMatch.Groups[1].Value;
                string url = linkMatch.Groups[2].Value;
                string title = "";
                result.Success = true;
                result.Line = regexImageImmediateNoTitle.Replace(
                    line,
                    ""
                );
                result.AddContent(
                    new MarkdownImage(
                        url,
                        text,
                        title
                    )
                );
            }
            // Format: ![text](url "title")
            linkMatch = regexImageImmediateWithTitle.Match(line);
            if (linkMatch.Success)
            {
                string text = linkMatch.Groups[1].Value;
                string url = linkMatch.Groups[2].Value;
                string title = linkMatch.Groups[3].Value;
                result.Success = true;
                result.Line = regexImageImmediateWithTitle.Replace(
                    line,
                    ""
                );
                result.AddContent(
                    new MarkdownImage(
                        url,
                        text,
                        title
                    )
                );
            }
            // Format: [text][id]    [id]: url     (title optional)
            linkMatch = regexImageReference.Match(line);
            if (linkMatch.Success)
            {
                string text = linkMatch.Groups[1].Value;
                string reference = linkMatch.Groups[2].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == reference)
                    {
                        result.Success = true;
                        result.Line = regexImageReference.Replace(
                            line,
                            ""
                        );
                        result.AddContent(
                            new MarkdownImage(
                                url.Url,
                                text,
                                url.Title
                            )
                        );
                    }
                }
            }
            return result;
        }

    }
}
