
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class HtmlTagDetector
    {
        private HtmlToken[] tokens;

        private ValidityTracker validity;

        private string probableName;

        private HtmlTagType probableType;

        public HtmlTagDetector(
            HtmlToken[] tokens
        ) {
            this.tokens = tokens;
            validity = new ValidityTracker();
        }

        public static HtmlSnippet[] TagsFromTokens(
            HtmlToken[] tokens
        ) {
            if (tokens.Length == 0) {
                return new HtmlSnippet[0];
            }
            int currentToken = 0;
            LinkedList<HtmlSnippet> snippets = new LinkedList<HtmlSnippet>();
            while (currentToken < tokens.Length)
            {
                HtmlTagDetector detector = new HtmlTagDetector(
                    new ArraySegment<HtmlToken>(
                        tokens,
                        currentToken,
                        tokens.Length - currentToken
                    ).ToArray()
                );
                HtmlSnippet snippet = detector.Detect();
                snippets.AddLast(snippet);
                currentToken = currentToken + snippet.TokenCount();
            }
            return snippets.ToArray();
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
                        tagTokens,
                        DetermineName(),
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
                return;
            };
            validity.Advance();
            if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Text)
            {
                validity.MarkInvalid();
                return;
            }
            probableName = tokens[validity.AtElement].Content;
            validity.Advance();
            MoveOverAttributes();
            MoveOverNonLineBreakingWhitespace();
            if (validity.AtElement >= tokens.Length || !(tokens[validity.AtElement].Type == HtmlTokenType.GreaterThan)) {
                validity.MarkInvalid();
                return;
            }
            validity.Advance();
            probableType = HtmlTagType.Opening;
        }

        private void CheckForClosingTag()
        {
            if (tokens.Length == 0 || tokens[0].Type != HtmlTokenType.LessThan) {
                validity.MarkInvalid();
                return;
            };
            validity.Advance();
            if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.ForwardSlash) {
                validity.MarkInvalid();
                return;
            };
            validity.Advance();
            if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Text)
            {
                validity.MarkInvalid();
                return;
            }
            probableName = tokens[validity.AtElement].Content;
            validity.Advance();
            MoveOverNonLineBreakingWhitespace();
            if (validity.AtElement >= tokens.Length || !(tokens[validity.AtElement].Type == HtmlTokenType.GreaterThan)) {
                validity.MarkInvalid();
                return;
            }
            validity.Advance();
            probableType = HtmlTagType.Closing;
        }

        private void CheckForSelfClosingTag()
        {
            if (tokens.Length == 0 || tokens[0].Type != HtmlTokenType.LessThan) {
                validity.MarkInvalid();
                return;
            };
            validity.Advance();
            if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Text)
            {
                validity.MarkInvalid();
                return;
            }
            probableName = tokens[validity.AtElement].Content;
            validity.Advance();
            MoveOverAttributes();
            MoveOverNonLineBreakingWhitespace();
            if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.ForwardSlash)
            {
                validity.MarkInvalid();
                return;
            }
            validity.Advance();
            if (validity.AtElement >= tokens.Length || !(tokens[validity.AtElement].Type == HtmlTokenType.GreaterThan)) {
                validity.MarkInvalid();
                return;
            }
            validity.Advance();
            probableType = HtmlTagType.SelfClosing;
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
                    return;
                }
                validity.Advance();
                MoveOverNonLineBreakingWhitespace();
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.DoubleQuote)
                {
                    validity.MarkInvalid();
                    return;
                }
                validity.Advance();
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.Text)
                {
                    validity.MarkInvalid();
                    return;
                }
                validity.Advance();
                if (validity.AtElement >= tokens.Length || tokens[validity.AtElement].Type != HtmlTokenType.DoubleQuote)
                {
                    validity.MarkInvalid();
                    return;
                }
                validity.Advance();
            }
        }

        private HtmlTagName DetermineName()
        {
            return new HtmlTagName(
                probableName
            );
        }

        private HtmlTagType DetermineTagType()
        {
            return probableType;
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