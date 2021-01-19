
namespace MarkdownToHtml
{
    public class HtmlSnippet
    {
        public HtmlToken Token 
        {
            get; private set;
        }

        public HtmlTag Tag
        {
            get; private set;
        }

        public HtmlSnippet(
            HtmlToken token
        ) {
            Token = token;
            Tag = null;
        }

        public HtmlSnippet(
            HtmlTag tag
        ) {
            Tag = tag;
            Token = null;
        }

        public bool IsTag()
        {
            return Tag != null;
        }

        public bool IsToken()
        {
            return Token != null;
        }

        public int TokenCount()
        {
            if (IsTag())
            {
                return Tag.GetTokens().Length;
            }
            if (IsToken())
            {
                return 1;
            }
            return 0;
        }
    }
}