
namespace MarkdownToHtml
{
    public class HtmlTagName
    {
        private static string[] BLOCK_ELEMENTS = new string[]
        {
            "address",
            "article",
            "aside",
            "blockquote",
            "dd",
            "details",
            "dialog",
            "div",
            "dl",
            "dt",
            "fieldset",
            "figcaption",
            "figure",
            "footer",
            "form",
            "h1",
            "h2",
            "h3",
            "h4",
            "h5",
            "h6",
            "header",
            "hgroup",
            "hr",
            "li",
            "main",
            "nav",
            "ol",
            "p",
            "pre",
            "section",
            "table",
            "tfoot",
            "ul"
        };

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
            foreach (string block in BLOCK_ELEMENTS)
            {
                if (block == name) {
                    return HtmlDisplayType.Block; 
                }
            }
            return HtmlDisplayType.Inline;
        }
    }
}