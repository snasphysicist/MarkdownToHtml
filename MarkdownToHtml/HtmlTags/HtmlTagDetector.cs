
namespace MarkdownToHtml
{
    public class HtmlTagDetector
    {
        private HtmlToken[] tokens;

        private ValidityTracker validity;

        public HtmlTagDetector(
            HtmlToken[] tokens
        ) {
            this.tokens = tokens;
            validity = new ValidityTracker();
        }

        public HtmlSnippet Detect()
        {
            CheckForTokenAtStart();
            if (validity.Valid) {
                HtmlToken[] tagTokens = new HtmlToken[validity.AtElement];
                for (int i = 0; i < validity.AtElement; i++)
                {
                    tagTokens[i] = tokens[i];
                }
                return new HtmlSnippet(
                    new HtmlTag(
                        tokens,
                        DetermineDisplayType(),
                        DetermineTagType()
                    )
                );
            } else {
                return new HtmlSnippet(
                    tokens[0]
                );
            }
        }

        public void CheckForTokenAtStart() 
        { 
            CheckForOpeningTag();
            if (validity.Valid)
            {
                return;
            }
            validity = new ValidityTracker();
            CheckForClosingTag();
            if (validity.Valid)
            {
                return;
            }
            validity = new ValidityTracker();
            CheckForSelfClosingTag();
        }

        private void CheckForOpeningTag()
        {
            if (tokens.Length == 0 || tokens[0].Type != HtmlTokenType.LessThan) {
                validity.MarkInvalid();
            };
            validity.Advance();
            if (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.Text)
            {
                validity.Advance();
            }
            MoveOverAttributes();
            MoveOverNonLineBreakingWhitespace();
            if (!(validity.AtElement + 1 == tokens.Length) || !(tokens[validity.AtElement].Type == HtmlTokenType.GreaterThan)) {
                validity.MarkInvalid();
            }
        }

        private void CheckForClosingTag()
        {
            if (tokens.Length == 0 || tokens[0].Type != HtmlTokenType.LessThan) {
                validity.MarkInvalid();
            };
            validity.Advance();
            if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.ForwardSlash) {
                validity.MarkInvalid();
            };
            validity.Advance();
            if (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.Text)
            {
                validity.Advance();
            }
            MoveOverNonLineBreakingWhitespace();
            if (!(validity.AtElement + 1 == tokens.Length) || !(tokens[validity.AtElement].Type == HtmlTokenType.GreaterThan)) {
                validity.MarkInvalid();
            }
        }

        private void CheckForSelfClosingTag()
        {
            if (tokens.Length == 0 || tokens[0].Type != HtmlTokenType.LessThan) {
                validity.MarkInvalid();
            };
            validity.Advance();
            if (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.Text)
            {
                validity.Advance();
            }
            MoveOverAttributes();
            MoveOverNonLineBreakingWhitespace();
            if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.ForwardSlash)
            {
                validity.MarkInvalid();
            }
            validity.Advance();
            if (!(validity.AtElement + 1 == tokens.Length) || !(tokens[validity.AtElement].Type == HtmlTokenType.GreaterThan)) {
                validity.MarkInvalid();
            }
        }

        private void MoveOverNonLineBreakingWhitespace() 
        {
            while (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.NonLineBreakingWhitespace)
            {
                validity.Advance();
            }
        }

        private void MoveOverAttributes() 
        {
            while (validity.AtElement < tokens.Length && tokens[validity.AtElement].Type == HtmlTokenType.NonLineBreakingWhitespace)
            {
                MoveOverNonLineBreakingWhitespace();
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Text)
                {
                    return;
                }
                validity.Advance();
                MoveOverNonLineBreakingWhitespace();
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Equals)
                {
                    validity.MarkInvalid();
                }
                validity.Advance();
                MoveOverNonLineBreakingWhitespace();
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

        private HtmlDisplayType DetermineDisplayType()
        {
            return HtmlDisplayType.Block;
        }

        private HtmlTagType DetermineTagType()
        {
            return HtmlTagType.Opening;
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
    }
}