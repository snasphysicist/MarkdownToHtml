
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
    }
}