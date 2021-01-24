
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

        [TestMethod]
        [Timeout(500)]
        public void ProperlyClosedInlineTagWithInnerTextIsATagGroup() 
        {
            string htmlString = "<span>Inner text</span>";
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
        public void ProperlyClosedInlineTagContainingAnotherProperlyClosedInlineTagIsASingleTagGroupWithOuterElementsType() 
        {
            string htmlString = "<span>Inner <b>important</b> text</span>";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(snippets);
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
            Assert.AreEqual(
                "span",
                elements[0].GroupDisplayType().Name
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("<span>Inner <b>important text</span>")]
        [DataRow("<span>Inner important</b> text</span>")]
        [DataRow("<span>Inner <b>important</i> text</span>")]
        public void ProperlyClosedInlineTagContainingImproperlyClosedInlineTagIsASingleTagGroupWithOuterElementsType(
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
            Assert.AreEqual(
                "span",
                elements[0].GroupDisplayType().Name
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("<p></p>")]
        [DataRow("<p></p>\n")]
        [DataRow("\n<p></p>")]
        [DataRow("\n<p></p>\n")]
        [DataRow("\n\n<p></p>\n")]
        [DataRow("\n<p></p>\n\n")]
        public void ProperlyClosedBlockTagWithoutAtLeastTwoPrecedingAndSucceedingLineBreaksIsNotATagGroup(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(snippets);
            foreach (HtmlElement element in elements)
            {
                Assert.IsFalse(
                    element.IsTagGroup
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void ProperlyClosedBlockTagWithTwoPrecedingTwoSucceedingLineBreaksAndNoInnerTextIsATagGroup() 
        {
            string htmlString = "\n\n<p></p>\n\n";
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
        public void ProperlyClosedBlockTagWithInnerTextIsATagGroup() 
        {
            string htmlString = "\n\n<p>Inside the paragraph</p>\n\n";
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