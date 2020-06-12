
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Image : IMarkdownParser
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

        public bool CanParseFrom(
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

        public ParseResult ParseFrom(
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
            LinkedList<Attribute> attributes = new LinkedList<Attribute>();
            Match linkMatch;
            // Format: ![text](url)
            linkMatch = regexImageImmediateNoTitle.Match(line);
            if (linkMatch.Success)
            {
                attributes.AddLast(
                    new Attribute(
                        "text",
                        linkMatch.Groups[1].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        "url",
                        linkMatch.Groups[2].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        "title",
                        ""
                    )
                );
                input.FirstLine = regexImageImmediateNoTitle.Replace(
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
                        "text",
                        linkMatch.Groups[1].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        "url",
                        linkMatch.Groups[2].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        "title",
                        linkMatch.Groups[3].Value
                    )
                );
                result.Success = true;
                input.FirstLine = regexImageImmediateWithTitle.Replace(
                    line,
                    ""
                );
            }
            // Format: [text][id]    [id]: url     (title optional)
            linkMatch = regexImageReference.Match(line);
            if (linkMatch.Success)
            {
                attributes.AddLast(
                    new Attribute(
                        "text",
                        linkMatch.Groups[1].Value
                    )
                );
                string text = linkMatch.Groups[1].Value;
                string reference = linkMatch.Groups[2].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == reference)
                    {
                        attributes.AddLast(
                            new Attribute(
                                "url",
                                url.Url
                            )
                        );
                        attributes.AddLast(
                            new Attribute(
                                "title",
                                url.Title
                            )
                        );
                        result.Success = true;
                        input.FirstLine = regexImageReference.Replace(
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
                    Utils.LinkedListToArray(attributes)
                )
            );
            return result;
        }
    }
}