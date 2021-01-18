
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

        public HtmlTag(
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
    }
}