
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarkdownToHtml
{
    public class HtmlTokeniser
    {
        private static Regex TEXT = new Regex(
            "^([\\w|!|@|#|\\$|%|^|&|\\*|\\(|\\)|_|\\+|=|\\[|\\]|'|;|:|\\.|\\,|\\?|\\\\]+).*"
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
            } else if (NonLineBreakingWhitespaceAtStart())
            {
                next = NonLineBreakingWhitespaceFromStart();
            } else if (LineBreakingWhitespaceAtStart())
            {
                next = LineBreakingWhitespaceFromStart();
            } else if (CharacterAtStartOfContent('<')) 
            {
                next = CharacterFromStartOfContent(HtmlTokenType.LessThan);
            } else if (CharacterAtStartOfContent('>'))
            {
                next = CharacterFromStartOfContent(HtmlTokenType.GreaterThan);
            } else if (CharacterAtStartOfContent('/')) {
                next = CharacterFromStartOfContent(HtmlTokenType.ForwardSlash);
            } else {
                next = CharacterFromStartOfContent(HtmlTokenType.DoubleQuote);
            }
            return next;
        }

        private bool NonLineBreakingWhitespaceAtStart()
        {
            if (content.Length == 0) {
                return false;
            }
            char firstCharacter = content[0];
            return firstCharacter == ' '
                || firstCharacter == '\t';
        }

        private HtmlToken NonLineBreakingWhitespaceFromStart()
        {
            char firstCharacter = content[0];
            int currentCharacter = 0;
            while (currentCharacter < content.Length && content[currentCharacter] == firstCharacter)
            {
                currentCharacter++;
            }
            HtmlToken next = new HtmlToken(
                HtmlTokenType.NonLineBreakingWhitespace,
                content.Substring(0, currentCharacter)
            );
            content = content.Substring(currentCharacter);
            return next;
        }

        private bool LineBreakingWhitespaceAtStart()
        {
            if (content.Length == 0) {
                return false;
            }
            char firstCharacter = content[0];
            return firstCharacter == '\r'
                || firstCharacter == '\n';
        }

        private HtmlToken LineBreakingWhitespaceFromStart()
        {
            string tokenContent = "";
            if (content.Length > 1 && content.Substring(0, 2) == "\r\n") 
            {
                tokenContent = "\r\n";
                content = content.Substring(2);
            } else 
            {
                tokenContent = content.Substring(0, 1);
                content = content.Substring(1);
            }
            HtmlToken next = new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                tokenContent
            );
            return next;
        }

        private bool CharacterAtStartOfContent(
            char checkFor
        ) {
            return content.Length > 0 && content[0] == checkFor;
        }

        private HtmlToken CharacterFromStartOfContent(HtmlTokenType characterType)
        {
            HtmlToken next = new HtmlToken(
                characterType,
                content.Substring(0, 1)
            );
            content = content.Substring(1);
            return next;
        }
    }
}