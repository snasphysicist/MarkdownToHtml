
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class HtmlElementSubstituter {

        private HtmlElement[] elements;

        public string Processed
        { get; private set; }

        private Dictionary<Guid, string> replacements;

        public HtmlElementSubstituter(
            HtmlElement[] elements
        ) {
            Processed = "";
            this.elements = elements;
            replacements = new Dictionary<Guid, string>();
        }

        public Dictionary<Guid, string> GetReplacements()
        {
            return new Dictionary<Guid, string>(replacements);
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
            Guid uuid = Guid.NewGuid();
            HtmlSnippet[] parts = element.Contents();
            Processed = Processed + parts[0].AsHtmlString() + parts[1].AsHtmlString() 
                + uuid.ToString() + parts[parts.Length - 2].AsHtmlString() + parts[parts.Length - 1].AsHtmlString();
            string replacedContent = "";
            for (int i = 2; i < parts.Length - 2; i++)
            {
                replacedContent = replacedContent + parts[i].AsHtmlString();
            }
            replacements.Add(
                uuid,
                replacedContent
            ); 
        }

        private void ProcessInlineElement(
            HtmlElement element
        ) {
            HtmlSnippet[] contents = element.Contents();
            HtmlSnippet opener = contents[0];
            Guid openerUuid = Guid.NewGuid();
            replacements.Add(
                openerUuid,
                opener.AsHtmlString()
            );
            HtmlSnippet closer = contents[contents.Length - 1];
            Guid closerUuid = Guid.NewGuid();
            replacements.Add(
                closerUuid,
                closer.AsHtmlString()
            );
            Processed = Processed + openerUuid.ToString();
            for (int i = 1; i < contents.Length - 1; i++)
            {
                Processed = Processed + contents[i].AsHtmlString();
            }
            Processed = Processed + closerUuid.ToString();
        }

        private void ProcessOther(
            HtmlElement element
        ) {
            Processed = Processed + element.AsHtmlString();
        }
    }
}