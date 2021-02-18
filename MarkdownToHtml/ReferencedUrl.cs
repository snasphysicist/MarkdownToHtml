
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ReferencedUrl
    {

        private static Regex regexLinkReference = new Regex(
            @"^\[(.*[^\\])\]:\s*([^\(\)""']+)"
        );

        private static Regex regexLinkReferenceWithDoubleQuotedTitle = new Regex(
            @"^\[(.*[^\\])\]:\s*(.+)\s+""(.+)""\s*"
        );

        private static Regex regexLinkReferenceWithSingleQuotedTitle = new Regex(
            @"^\[(.*[^\\])\]:\s*(.+)\s+'(.+)'\s*"
        );

        private static Regex regexLinkReferenceWithParentheticTitle = new Regex(
            @"^\[(.*[^\\])\]:\s*(.+)\s+\((.+)\)\s*"
        );

        private static Regex regexImageReference = new Regex(
            @"^\[(.*[^\\])\]:\s*(.+?)\s+""(.+?)""\s*$"
        );

        public string Reference
        { get; private set; }

        public string Url
        { get; private set; }

        public string Title
        { get; private set; }

        public ReferencedUrl(
            string reference,
            string url,
            string title
        ) {
            Reference = reference;
            Url = url;
            Title = title;
        }

        public static bool CanParseFrom(
            ArraySegment<string> lines
        ) {
            return (
                regexLinkReference.Match(lines[0]).Success
                || regexImageReference.Match(lines[0]).Success
                || regexLinkReferenceWithDoubleQuotedTitle.Match(lines[0]).Success
                || regexLinkReferenceWithSingleQuotedTitle.Match(lines[0]).Success
                || regexLinkReferenceWithParentheticTitle.Match(lines[0]).Success
            );
        }

        public static ReferencedUrl ParseFrom(
            ArraySegment<string> lines
        ) {
            Match urlMatch;
            // Image ![alt](link \"title\")
            urlMatch = regexImageReference.Match(
                lines[0]
            );
            if (urlMatch.Success)
            {
                lines[0] = regexImageReference.Replace(
                    lines[0],
                    ""
                );
                return new ReferencedUrl(
                    urlMatch.Groups[1].Value,
                    urlMatch.Groups[2].Value,
                    urlMatch.Groups[3].Value
                );
            }
            urlMatch = regexLinkReferenceWithDoubleQuotedTitle.Match(
                lines[0]
            );
            if (urlMatch.Success)
            {
                lines[0] = regexLinkReferenceWithDoubleQuotedTitle.Replace(
                    lines[0],
                    ""
                );
                return new ReferencedUrl(
                    urlMatch.Groups[1].Value,
                    urlMatch.Groups[2].Value,
                    urlMatch.Groups[3].Value
                );
            }
            urlMatch = regexLinkReferenceWithSingleQuotedTitle.Match(
                lines[0]
            );
            if (urlMatch.Success)
            {
                lines[0] = regexLinkReferenceWithSingleQuotedTitle.Replace(
                    lines[0],
                    ""
                );
                return new ReferencedUrl(
                    urlMatch.Groups[1].Value,
                    urlMatch.Groups[2].Value,
                    urlMatch.Groups[3].Value
                );
            }
            urlMatch = regexLinkReferenceWithParentheticTitle.Match(
                lines[0]
            );
            if (urlMatch.Success) 
            {
                lines[0] = regexLinkReferenceWithParentheticTitle.Replace(
                    lines[0],
                    ""
                );
                return new ReferencedUrl(
                    urlMatch.Groups[1].Value,
                    urlMatch.Groups[2].Value,
                    urlMatch.Groups[3].Value
                );
            }
            urlMatch = regexLinkReference.Match(
                lines[0]
            );
            lines[0] = regexLinkReference.Replace(
                lines[0],
                ""
            );
            return new ReferencedUrl(
                urlMatch.Groups[1].Value,
                urlMatch.Groups[2].Value,
                ""
            );
        }
    }
}
