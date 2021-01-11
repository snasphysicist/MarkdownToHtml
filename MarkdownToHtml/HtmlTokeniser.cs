
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class HtmlTokeniser
    {
        private static Regex TEXT = new Regex("^(\\w*).*");

        private string content;

        public HtmlTokeniser(
            string content
        ) {
            this.content = content;
        }

        public LinkedList<HtmlToken> tokenise()
        {
            LinkedList<HtmlToken> tokens = new LinkedList<HtmlToken>();
            while (content.Length > 0)
            {
                tokens.AddLast(nextToken());
            }
            return tokens;
        }

        private HtmlToken nextToken()
        {
            HtmlToken next;
            next = new HtmlToken(
                HtmlTokenType.Text,
                TEXT.Match(content).Groups[1].Captures[0].ToString()
            );
            content = content.Substring(TEXT.Match(content).Groups[1].Captures[0].Length);
            return next;
        }
    }
}