
namespace MarkdownToHtml
{
    public class TagContentAttributesWriter : IHtmlWriter
    {
        public string WriteToString(
            ElementDetails details
        ) {
            string attributeSection = "";
            foreach (Attribute attribute in details.Attributes())
            {
                attributeSection += $" {attribute.Name}=\"{attribute.Value}\"";
            }
            string html = $"<{details.Tag}{attributeSection}>";
            foreach (IHtmlable item in details.Content())
            {
                html += item.ToHtml();
            }
            html += $"</{details.Tag}>";
            return html;
        }
    }
}