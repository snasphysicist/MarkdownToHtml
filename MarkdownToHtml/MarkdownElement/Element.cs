
namespace MarkdownToHtml
{
    public class Element : IHtmlable
    {
        private ElementDetails details;
        private IHtmlWriter writer;

        public Element(
            ElementDetails details,
            IHtmlWriter writer
        ) {
            this.details = details;
            this.writer = writer;
        }

        public string ToHtml()
        {
            return writer.WriteToString(
                details
            );
        }
    }
}