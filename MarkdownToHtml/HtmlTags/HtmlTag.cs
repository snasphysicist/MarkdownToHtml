
namespace MarkdownToHtml
{
    public class HtmlTag
    {
        private HtmlToken[] tokens;

        public HtmlTagName Name
        { get; private set; }

        public HtmlTagType Type
        { get; private set; }

        public HtmlTag(
            HtmlToken[] tokens,
            HtmlTagName name,
            HtmlTagType tagType
        ) {
            this.tokens = tokens;
            Name = Name;
            Type = tagType;
        }

        public HtmlToken[] GetTokens()
        {
            return tokens;
        }
    }
}