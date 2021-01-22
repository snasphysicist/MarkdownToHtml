
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlElementDetectorTests
    {
        private static HtmlSnippet[] snippetsFromHtmlString(
            string htmlString
        ) {
            HtmlToken[] tokens = new HtmlTokeniser(htmlString).tokenise();
            return HtmlTagDetector.TagsFromTokens(tokens);
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(" ")]
        [DataRow("Some")]
        [DataRow("\n")]
        [DataRow("<")]
        [DataRow(">")]
        [DataRow("/")]
        [DataRow("=")]
        [DataRow("\"")]
        [DataRow("<p>")]
        [DataRow("</p>")]
        public void SingleSnippetNotSelfClosingTagIsNotATagGroup(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(snippets);
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsFalse(
                elements[0].IsTagGroup
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("<sc a=\"b\"/>")]
        public void SingleSnippetSelfClosingTagIsTagGroup(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(snippets);
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void ProperlyClosedInlineTagWithNoInnerTextIsATagGroup() 
        {
            string htmlString = "<span></span>";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(snippets);
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
        }
    }
}