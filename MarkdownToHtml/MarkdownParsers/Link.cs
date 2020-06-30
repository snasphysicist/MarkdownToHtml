
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Link : IMarkdownParser
    {
        private static Regex regexLinkImmediate = new Regex(
            @"^\[(.*[^\\])\]\((.*[^\\])\)"
        );

        private static Regex regexLinkReference = new Regex(
            @"^\[(.*[^\\])\]\[(.*[^\\])\]"
        );

        private Regex regexLinkSelfReference = new Regex(
            @"^\[(.*[^\\])\]"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
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
            // Format: [text](url)
            linkMatch = regexLinkImmediate.Match(line);
            if (linkMatch.Success)
            {
                attributes.AddLast(
                    new Attribute(
                        "href",
                        linkMatch.Groups[2].Value
                    )
                );
                result.Success = true;
                input[0].Text = regexLinkImmediate.Replace(
                    line,
                    ""
                );
                result.AddContent(
                    new ElementFactory().New(
                        ElementType.Link,
                        MarkdownParser.ParseInnerText(
                            new ParseInput(
                                input,
                                linkMatch.Groups[1].Value
                            )
                        ),
                        attributes.ToArray()
                    )
                );
            }
            // Format: [text][id]    [id]: url
            linkMatch = regexLinkReference.Match(line);
            if (linkMatch.Success)
            {
                string reference = linkMatch.Groups[2].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == reference)
                    {
                        attributes.AddLast(
                            new Attribute(
                                "href",
                                url.Url
                            )
                        );
                        result.Success = true;
                        input[0].Text = regexLinkReference.Replace(
                            line,
                            ""
                        );
                        result.AddContent(
                            new ElementFactory().New(
                                ElementType.Link,
                                MarkdownParser.ParseInnerText(
                                    new ParseInput(
                                        input,
                                        linkMatch.Groups[1].Value
                                    )
                                ),
                                attributes.ToArray()
                            )
                        );
                    }
                }
            }
            // Format: [text]   [text]: url
            linkMatch = regexLinkSelfReference.Match(line);
            if (linkMatch.Success)
            {
                string innerText = linkMatch.Groups[1].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == innerText)
                    {
                        attributes.AddLast(
                            new Attribute(
                                "href",
                                url.Url
                            )
                        );
                        result.Success = true;
                        input[0].Text = regexLinkSelfReference.Replace(
                            line,
                            ""
                        );
                        result.AddContent(
                            new ElementFactory().New(
                                ElementType.Link,
                                MarkdownParser.ParseInnerText(
                                    new ParseInput(
                                        input,
                                        innerText
                                    )
                                ),
                                attributes.ToArray()
                            )
                        );
                    }
                }
            }
            return result;
        }
    }
}