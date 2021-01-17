
using System;

namespace MarkdownToHtml
{
    public class HtmlTag
    {
        private HtmlToken[] tokens;

        public HtmlDisplayType DisplayType
        { get; private set; }

        public HtmlTagType TagType
        { get; private set; }

        private HtmlTag(
            HtmlToken[] tokens,
            HtmlDisplayType displayType,
            HtmlTagType tagType
        ) {
            this.tokens = tokens;
            DisplayType = displayType;
            TagType = tagType;
        }

        public HtmlToken[] getTokens()
        {
            return tokens;
        }

        public static HtmlTag FromTokens(
            HtmlToken[] tokens
        ) {
            if (!IsValidTag(tokens))
            {
                throw new ArgumentException(string.Join(", ", tokens.GetEnumerator()));
            }
            return new HtmlTag(
                tokens,
                DetermineDisplayType(tokens),
                DetermineTagType(tokens)
            );
        }

        public static bool IsValidTag(
            HtmlToken[] tokens
        ) { 
            return IsValidOpeningTag(tokens);
        }

        private static bool IsValidOpeningTag(
            HtmlToken[] tokens
        ) {
            if (tokens.Length == 0 || tokens[0].Type != HtmlTokenType.LessThan) {
                return false;
            };
            int i = 1;
            while (i < tokens.Length && tokens[i].Type == HtmlTokenType.NonLineBreakingWhitespace)
            {
                i++;
            }
            if (i < tokens.Length && tokens[i].Type == HtmlTokenType.Text)
            {
                i++;
            }
            while (i < tokens.Length && tokens[i].Type == HtmlTokenType.NonLineBreakingWhitespace)
            {
                i++;
            }
            return (i + 1 == tokens.Length) && (tokens[i].Type == HtmlTokenType.GreaterThan);
        }

        private static HtmlDisplayType DetermineDisplayType(
            HtmlToken[] tokens
        ) {
            return HtmlDisplayType.Block;
        }

        private static HtmlTagType DetermineTagType(
            HtmlToken[] tokens
        ) {
            return HtmlTagType.Opening;
        }
    }
}