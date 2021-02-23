
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
            Name = name;
            Type = tagType;
        }

        public HtmlToken[] GetTokens()
        {
            return tokens;
        }

        public string AsHtmlString()
        {
            string html = "";
            foreach (HtmlToken token in tokens)
            {
                html = html + token.AsHtmlString();
            }
            return html;
        }
    }
}