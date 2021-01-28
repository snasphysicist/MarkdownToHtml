
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class HtmlElementSubstituter {

        private HtmlElement[] elements;

        public string Processed
        { get; private set; }

        private Dictionary<string, string> replacements;

        public HtmlElementSubstituter(
            HtmlElement[] elements
        ) {
            this.elements = elements;
            replacements = new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetReplacements()
        {
            return new Dictionary<string, string>(replacements);
        }

        public void Process()
        {
            foreach (HtmlElement element in elements)
            {
                if (IsBlockElement(element))
                {
                    ProcessBlockElement(element);
                } else if (IsInlineElement(element))
                {
                    ProcessInlineElement(element);
                } else {
                    ProcessOther(element);
                }
            }
        }

        private bool IsBlockElement(
            HtmlElement element
        ) {
            return element.IsTagGroup
                && element.GroupDisplayType().Type == HtmlDisplayType.Block;
        }

        private bool IsInlineElement(
            HtmlElement element
        ) {
            return element.IsTagGroup
                && element.GroupDisplayType().Type == HtmlDisplayType.Inline;
        }

        private void ProcessBlockElement(
            HtmlElement element
        ) {

        }

        private void ProcessInlineElement(
            HtmlElement element
        ) {

        }

        private void ProcessOther(
            HtmlElement element
        ) {
            Processed = Processed + element.AsHtmlString();
        }
    }
}