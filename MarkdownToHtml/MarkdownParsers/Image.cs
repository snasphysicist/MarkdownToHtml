
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Image : IMarkdownParser
    {
        private static readonly string UrlAttributeName = "src";
        private static readonly string AltTextAttributeName = "alt";
        private static readonly string TitleAttributeName = "title";

        private static Regex regexImageImmediateNoTitle = new Regex(
            @"^!\[(.*[^\\])\]\((\S*[^\\])\)"
        );

        private static Regex regexImageImmediateWithTitle = new Regex(
            @"^!\[(.*[^\\])\]\((.*?[^\\])\s+""(.+)""\s*\)"
        );

        private static Regex regexImageReference = new Regex(
            @"^!\[(.*[^\\])\]\[(.*[^\\])\]"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
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

        public ParseResult ParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
            ReferencedUrl[] urls = input.Urls;
            ParseResult result = new ParseResult();
            if (
                !CanParseFrom(input)
            ) {
                return result;
            }
            LinkedList<Attribute> attributes = new LinkedList<Attribute>();
            Match linkMatch;
            // Format: ![text](url)
            linkMatch = regexImageImmediateNoTitle.Match(line);
            if (linkMatch.Success)
            {
                attributes.AddLast(
                    new Attribute(
                        UrlAttributeName,
                        linkMatch.Groups[2].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        AltTextAttributeName,
                        linkMatch.Groups[1].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        TitleAttributeName,
                        ""
                    )
                );
                result.Success = true;
                input[0].Text = regexImageImmediateNoTitle.Replace(
                    line,
                    ""
                );
            }
            // Format: ![text](url "title")
            linkMatch = regexImageImmediateWithTitle.Match(line);
            if (linkMatch.Success)
            {
                attributes.AddLast(
                    new Attribute(
                        UrlAttributeName,
                        linkMatch.Groups[2].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        AltTextAttributeName,
                        linkMatch.Groups[1].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        TitleAttributeName,
                        linkMatch.Groups[3].Value
                    )
                );
                result.Success = true;
                input[0].Text = regexImageImmediateWithTitle.Replace(
                    line,
                    ""
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
                        attributes.AddLast(
                            new Attribute(
                                UrlAttributeName,
                                url.Url
                            )
                        );
                        attributes.AddLast(
                            new Attribute(
                                AltTextAttributeName,
                                linkMatch.Groups[1].Value
                            )
                        );
                        attributes.AddLast(
                            new Attribute(
                                TitleAttributeName,
                                url.Title
                            )
                        );
                        result.Success = true;
                        input[0].Text = regexImageReference.Replace(
                            line,
                            ""
                        );
                        break;
                    }
                }
            }
            result.AddContent(
                new ElementFactory().New(
                    ElementType.Image,
                    attributes.ToArray()
                )
            );
            return result;
        }
    }
}