
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class HtmlTokeniser
    {
        private static Regex TEXT = new Regex(
            "^([\\w|!|@|#|\\$|%|^|&|\\*|\\(|\\)|_|\\+|=|\\[|\\]|'|;|:|\\.|\\,|\\?|\\\\]+).*"
        );

        private static Regex WHITESPACE = new Regex(
            "^([\\t+|\\n+|\\r+]).*"
        );

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
            if (TEXT.Match(content).Success) 
            {
                next = new HtmlToken(
                    HtmlTokenType.Text,
                    TEXT.Match(content).Groups[1].Captures[0].ToString()
                );
                content = content.Substring(TEXT.Match(content).Groups[1].Captures[0].Length);
            } else if (WhitespaceAtStart())
            {
                next = GetWhitespaceFromStart();
            } else 
            {
                next = LessThanFromStart();
            }
            return next;
        }

        private bool WhitespaceAtStart()
        {
            if (content.Length == 0) {
                return false;
            }
            char firstCharacter = content[0];
            return firstCharacter == ' '
                || firstCharacter == '\t'
                || firstCharacter == '\n'
                || firstCharacter == '\r';
        }

        private HtmlToken GetWhitespaceFromStart()
        {
            char firstCharacter = content[0];
            int currentCharacter = 0;
            while (currentCharacter < content.Length && content[currentCharacter] == firstCharacter)
            {
                currentCharacter++;
            }
            HtmlToken next = new HtmlToken(
                HtmlTokenType.Whitespace,
                content.Substring(0, currentCharacter)
            );
            content = content.Substring(currentCharacter);
            return next;
        }

        private bool LessThanAtStart()
        {
            return content.Length > 0 && content[0] == '<';
        }

        private HtmlToken LessThanFromStart()
        {
            HtmlToken next = new HtmlToken(
                HtmlTokenType.LessThan,
                "<"
            );
            content = content.Substring(1);
            return next;
        }
    }
}