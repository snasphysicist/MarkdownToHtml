
namespace MarkdownToHtml
{
    public class MarkdownText : IHtmlable
    {

        private string[][] EscapeReplacements = new string[][]{
            new string[] {"\\*", "*"},
            new string[] {"\\_", "_"},
            new string[] {"\\~", "~"},
            new string[] {"\\`", "`"}
        };

        string content;

        public MarkdownElementType Type
        { get; private set; }

        public MarkdownText(
            string content
        ) {
            Type = MarkdownElementType.Text;
            this.content = ReplaceEscapeCharacters(
                content
            );
        }

        public string ToHtml() {
            return content;
        }

        private string ReplaceEscapeCharacters(
            string text
        ) {
            foreach (string[] replacement in EscapeReplacements)
            {
                text = text.Replace(
                    replacement[0],
                    replacement[1]
                );
            }
            return text;
        }
    }
}