
namespace MarkdownToHtml
{
    public class HtmlElement
    {
        private HtmlSnippet[] sequence;

        public bool IsTagGroup
        { get; private set; }

        public HtmlElement(
            HtmlSnippet snippet,
            bool isTagGroup
        ) {
            this.sequence = new HtmlSnippet[]
            {
                snippet
            };
            IsTagGroup = isTagGroup;
        }

        public HtmlElement(
            HtmlSnippet[] sequence,
            bool isTagGroup
        ) {
            this.sequence = sequence;
            IsTagGroup = isTagGroup;
        }

        public int Count()
        {
            return sequence.Length;
        }
    }
}