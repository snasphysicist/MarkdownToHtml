
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlSubstitutionTests
    {

        private HtmlElement[] htmlStringToElements(
            string html
        ) {
            HtmlToken[] tokens = new HtmlTokeniser(html).tokenise();
            HtmlSnippet[] snippets = HtmlTagDetector.TagsFromTokens(tokens);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(snippets);
            return elements;
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
            HtmlElement[] elements = htmlStringToElements(html);
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
    }
}