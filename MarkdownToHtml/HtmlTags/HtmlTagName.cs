
namespace MarkdownToHtml
{
    public class HtmlTagName
    {
        public string Name
        { get; private set; }

        public HtmlDisplayType Type
        { get; private set; }

        public HtmlTagName(
            string name
        ) {
            this.Name = name;
            this.Type = DetermineDisplayType(name);
        }

        private HtmlDisplayType DetermineDisplayType(
            string name
        ) {
            return HtmlDisplayType.Inline;
        }
    }
}