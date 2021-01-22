
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
        [DataRow("<sc a=\"b\"/>")]
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
    }
}