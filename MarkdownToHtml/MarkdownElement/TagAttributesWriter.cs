
namespace MarkdownToHtml
{
    public class TagAttributesWriter : HtmlWriterBase, IHtmlWriter
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
            html += $"</{details.Tag}>";
            return TerminateWithNewLineIfRequired(details, html);
        }
    }
}