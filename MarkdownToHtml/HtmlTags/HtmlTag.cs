
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

        public HtmlTag fromTokens(
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

        private bool IsValidTag(
            HtmlToken[] tokens
        ) {
            return false;
        }

        private HtmlDisplayType DetermineDisplayType(
            HtmlToken[] tokens
        ) {
            return HtmlDisplayType.Block;
        }

        private HtmlTagType DetermineTagType(
            HtmlToken[] tokens
        ) {
            return HtmlTagType.Opening;
        }
    }
}