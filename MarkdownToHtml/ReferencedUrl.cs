
using System;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class ReferencedUrl
    {

        private static Regex regexUrlReference = new Regex(
            @"^\[(.*[^\\])\]:\s*(.+)"
        );

        public string Reference
        { get; private set; }

        public string Url
        { get; private set; }

        public ReferencedUrl(
            string reference,
            string url
        ) {
            Reference = reference;
            Url = url;
        }

        public static bool CanParseFrom(
            ArraySegment<string> lines
        ) {
            return regexUrlReference.Match(lines[0]).Success;
        }

        public static ReferencedUrl ParseFrom(
            ArraySegment<string> lines
        ) {
            Match contentMatch = regexUrlReference.Match(
                lines[0]
            );
            string reference = contentMatch.Groups[1].Value;
            string url = contentMatch.Groups[2].Value;
            lines[0] = regexUrlReference.Replace(
                lines[0],
                ""
            );
            return new ReferencedUrl(
                reference,
                url
            );
        }

    }
}
