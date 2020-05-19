
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ReferencedUrl
    {

        private static Regex regexLinkReference = new Regex(
            @"^\[(.*)[^\\]\]:(.+)"
        );

        private static Regex regexImageReference = new Regex(
            @"^\[(.*[^\\])\]:\s*(.+?)\s+""(.+?)""\s*$"
        );

        public ReferencedUrlType Type
        { get; private set; }

        public string Reference
        { get; private set; }

        public string Url
        { get; private set; }

        public string Title
        { get; private set; }

        public ReferencedUrl(
            string reference,
            string url,
            string title,
            ReferencedUrlType type
        ) {
            Reference = reference;
            Url = url;
            Title = title;
            Type = type;
        }

        public static bool CanParseFrom(
            ArraySegment<string> lines
        ) {
            return (
                regexLinkReference.Match(lines[0]).Success
                || regexImageReference.Match(lines[0]).Success
            );
        }

        public static ReferencedUrl ParseFrom(
            ArraySegment<string> lines
        ) {
            string reference = "";
            string url = "";
            string title = "";
            ReferencedUrlType type = ReferencedUrlType.Link;
            Match linkMatch = regexLinkReference.Match(
                lines[0]
            );
            Match imageMatch = regexImageReference.Match(
                lines[0]
            );
            if (linkMatch.Success)
            {
                reference = linkMatch.Groups[1].Value;
                url = linkMatch.Groups[2].Value;
                type = ReferencedUrlType.Link;
                lines[0] = regexLinkReference.Replace(
                    lines[0],
                    ""
                );
            } else if (imageMatch.Success)
            {
                reference = imageMatch.Groups[1].Value;
                url = imageMatch.Groups[2].Value;
                title = imageMatch.Groups[3].Value;
                type = ReferencedUrlType.Image;
                lines[0] = regexImageReference.Replace(
                    lines[0],
                    ""
                );
            }
            return new ReferencedUrl(
                reference,
                url,
                title,
                type
            );
        }

    }
}
