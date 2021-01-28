
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlSubstitutionTests
    {

        private HtmlElement[] HtmlStringToElements(
            string html
        ) {
            HtmlToken[] tokens = new HtmlTokeniser(html).tokenise();
            HtmlSnippet[] snippets = HtmlTagDetector.TagsFromTokens(tokens);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(snippets);
            return elements;
        }

        private Guid[] Keys(
            Dictionary<Guid, string> replacements
        ) {
            Guid[] guids = new Guid[replacements.Count];
            replacements.Keys.CopyTo(guids, 0);
            return guids;
        }

        private string[] Values(
            Dictionary<Guid, string> replacements
        ) {
            string[] strings = new string[replacements.Count];
            replacements.Values.CopyTo(strings, 0);
            return strings;
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("Text")]
        [DataRow(" ")]
        [DataRow("\n")]
        [DataRow("<")]
        [DataRow(">")]
        [DataRow("/")]
        [DataRow("=")]
        [DataRow("\"")]
        [DataRow("<p>")]
        [DataRow("</p>")]
        public void TextWhichDoesNotFormAValidHtmlElementIsNotReplacedByAnything(
            string html
        ) {
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                html,
                substituter.Processed
            );
            Assert.AreEqual(
                0,
                substituter.GetReplacements().Count
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SingleValidBlockLevelElementWithoutNestedElementsIsReplacedByLineBreaksAndUUID()
        {
            string html = "\n\n<p>Test</p>\n\n";
            HtmlElement[] elements = HtmlStringToElements(html);
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                1,
                substituter.GetReplacements().Count
            );
            Guid uuid = Keys(substituter.GetReplacements())[0];
            string substituted = Values(substituter.GetReplacements())[0];
            Assert.AreEqual(
                html.Replace("\n", ""),
                substituted
            );
            Assert.AreEqual(
                substituter.Processed,
                "\n\n" + uuid.ToString() + "\n\n"
            );
        }
    }
}