
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class Link : IMarkdownParser
    {
        private static Regex regexLinkImmediate = new Regex(
            @"^\(([^""]*[^\\])\)"
        );

        private static Regex regexLinkImmediateWithTitle = new Regex(
            @"^\((.*[^\\])\s+""(.+)""\s*\)"
        );

        private static Regex regexLinkReference = new Regex(
            @"^\s*\[(.*[^\\])\]"
        );

        public bool CanParseFrom(
            ParseInput input
        ) {
            string line = input[0].Text;
            ReferencedUrl[] urls = input.Urls;
            if (line.Length == 0 || line[0] != '[')
            {
                return false;
            }
            int closingBracket = FindClosingBracket(
                line
            );
            if (
                closingBracket == line.Length 
                && !line.EndsWith(']')
            ) {
                return false;
            }
            string restOfString = line.Substring(closingBracket);
            if (
                regexLinkImmediate.Match(restOfString).Success
                || regexLinkImmediateWithTitle.Match(restOfString).Success
            ) {
                return true;
            }
            Match linkMatch = regexLinkReference.Match(restOfString);
            string reference;
            if (linkMatch.Success)
            {
                reference = linkMatch.Groups[1].Value;
                foreach (ReferencedUrl url in urls)
                {
                    if (url.Reference == reference)
                    {
                        return true;
                    }
                }
            }
            reference = line.Substring(1, closingBracket - 2);
            foreach (ReferencedUrl url in urls)
            {
                if (url.Reference == reference)
                {
                    return true;
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
            int closingBracket = FindClosingBracket(line);
            string inSquareBrackets = line.Substring(1, closingBracket - 2);
            string restOfLine = line.Substring(closingBracket);
            LinkedList<Attribute> attributes = new LinkedList<Attribute>();
            Match linkMatch;
            // Format: [text](url)
            linkMatch = regexLinkImmediate.Match(restOfLine);
            if (linkMatch.Success)
            {
                attributes.AddLast(
                    new Attribute(
                        "href",
                        linkMatch.Groups[1].Value
                    )
                );
                result.Success = true;
                input[0].Text = regexLinkImmediate.Replace(
                    restOfLine,
                    ""
                );
                result.AddContent(
                    new ElementFactory().New(
                        ElementType.Link,
                        MarkdownParser.ParseInnerText(
                            new ParseInput(
                                input,
                                inSquareBrackets
                            )
                        ),
                        attributes.ToArray()
                    )
                );
            }
            // Format: [text](url "title")
            linkMatch = regexLinkImmediateWithTitle.Match(restOfLine);
            if (linkMatch.Success)
            {
                attributes.AddLast(
                    new Attribute(
                        "href",
                        linkMatch.Groups[1].Value
                    )
                );
                attributes.AddLast(
                    new Attribute(
                        "title",
                        linkMatch.Groups[2].Value
                    )
                );
                result.Success = true;
                input[0].Text = regexLinkImmediateWithTitle.Replace(
                    restOfLine,
                    ""
                );
                result.AddContent(
                    new ElementFactory().New(
                        ElementType.Link,
                        MarkdownParser.ParseInnerText(
                            new ParseInput(
                                input,
                                inSquareBrackets
                            )
                        ),
                        attributes.ToArray()
                    )
                );
            }
            // Format: [text][id]    [id]: url
            linkMatch = regexLinkReference.Match(restOfLine);
            if (linkMatch.Success)
            {
                string reference = linkMatch.Groups[1].Value;
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
                        attributes.AddLast(
                            new Attribute(
                                "title",
                                url.Title
                            )
                        );
                        result.Success = true;
                        input[0].Text = regexLinkReference.Replace(
                            restOfLine,
                            ""
                        );
                        result.AddContent(
                            new ElementFactory().New(
                                ElementType.Link,
                                MarkdownParser.ParseInnerText(
                                    new ParseInput(
                                        input,
                                        inSquareBrackets
                                    )
                                ),
                                attributes.ToArray()
                            )
                        );
                    }
                }
            }
            // Format: [text]   [text]: url
            foreach (ReferencedUrl url in urls)
            {
                if (url.Reference == inSquareBrackets)
                {
                    attributes.AddLast(
                        new Attribute(
                            "href",
                            url.Url
                        )
                    );
                    result.Success = true;
                    input[0].Text = restOfLine;
                    result.AddContent(
                        new ElementFactory().New(
                            ElementType.Link,
                            MarkdownParser.ParseInnerText(
                                new ParseInput(
                                    input,
                                    inSquareBrackets
                                )
                            ),
                            attributes.ToArray()
                        )
                    );
                }
            }
            return result;
        }

        private static int FindClosingBracket(
            string line
        ) {
            Stack<char> accumulator = new Stack<char>();
            accumulator.Push('[');
            int current = 1;
            while (
                current < line.Length 
                && accumulator.Count > 0
            ) {
                if (line[current] == '\\')
                {
                    if (accumulator.Peek() == '\\')
                    {
                        accumulator.Pop();
                    } else 
                    {
                        accumulator.Push('\\');
                    }
                } else if (line[current] == ']')
                {
                    char top = accumulator.Peek();
                    if (
                        top == '['
                        || top == '\\'
                    ) {
                        accumulator.Pop();
                    } else {
                        accumulator.Push(']');
                    }
                } else if (line[current] == '[')
                {
                    if (accumulator.Peek() == '\\')
                    {
                        accumulator.Pop();
                    } else {
                        accumulator.Push('[');
                    }
                }
                current++;
            }
            return current;
        }
    }
}