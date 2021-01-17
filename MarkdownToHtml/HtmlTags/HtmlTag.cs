
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

        private class ValidityTracker
        {
            public int AtElement
            { get; private set; }

            public bool Valid
            { get; private set; }

            public ValidityTracker()
            {
                AtElement = 0;
                Valid = true;
            }

            public void Advance()
            {
                AtElement++;
            }

            public void MarkInvalid()
            {
                Valid = false;
            }
        }

        private static bool IsValidOpeningTag(
            HtmlToken[] tokens
        ) {
            ValidityTracker validity = new ValidityTracker();
            if (tokens.Length == 0 || tokens[0].Type != HtmlTokenType.LessThan) {
                validity.MarkInvalid();
            };
            validity.Advance();
            if (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.Text)
            {
                validity.Advance();
            }
            MoveOverAttributes(
                tokens,
                validity
            );
            MoveOverNonLineBreakingWhitespace(
                tokens,
                validity
            );
            return (validity.AtElement + 1 == tokens.Length) && (tokens[validity.AtElement].Type == HtmlTokenType.GreaterThan);
        }

        private static void MoveOverNonLineBreakingWhitespace(
            HtmlToken[] tokens,
            ValidityTracker validity
        ) {
            while (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.NonLineBreakingWhitespace)
            {
                validity.Advance();
            }
        }

        private static void MoveOverAttributes(
            HtmlToken[] tokens,
            ValidityTracker validity
        ) {
            while (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.NonLineBreakingWhitespace)
            {
                MoveOverNonLineBreakingWhitespace(
                    tokens,
                    validity
                );
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Text)
                {
                    return;
                }
                validity.Advance();
                MoveOverNonLineBreakingWhitespace(
                    tokens,
                    validity
                );
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Equals)
                {
                    validity.MarkInvalid();
                }
                validity.Advance();
                MoveOverNonLineBreakingWhitespace(
                    tokens,
                    validity
                );
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.DoubleQuote)
                {
                    validity.MarkInvalid();
                }
                validity.Advance();
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Text)
                {
                    validity.MarkInvalid();
                }
                validity.Advance();
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.DoubleQuote)
                {
                    validity.MarkInvalid();
                }
                validity.Advance();
            }
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