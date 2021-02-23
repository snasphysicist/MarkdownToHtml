
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
        [DataRow("<span>Inner <p>important text</span>")]
        [DataRow("<span>Inner important</p> text</span>")]
        [DataRow("<span>Inner <p>important</div> text</span>")]
        public void ProperlyClosedInlineTagContainingImproperlyClosedBlockTagIsASingleTagGroupWithOuterElementsType(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
        [DataRow("<span>Inner <p>important</p> text</span>")]
        [DataRow("<span>Inner \n\n<p>important</p>\n\n text</span>")]
        public void ProperlyClosedInlineTagContainingProperlyClosedBlockTagIsNotATagGroupOfInlineType(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
            foreach (HtmlElement element in elements)
            {
                if (element.IsTagGroup)
                {
                    Assert.AreNotEqual(
                        "span",
                        element.GroupDisplayType().Name
                    );
                    Assert.AreNotEqual(
                        HtmlDisplayType.Inline,
                        element.GroupDisplayType().Type
                    );
                }
            }
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("<b>Inner<b>")]
        [DataRow("</b>Inner</b>")]
        public void TwoTagsOfSameTypeOpeningOrClosingDoNotFormATagGroup(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
            foreach (HtmlElement element in elements)
            {
                Assert.IsFalse(
                    elements[0].IsTagGroup
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        [DataRow()]
        public void ProperlyClosedInlineTagContainingProperlyClosedInlineTagOfSameTypeIsSingleTagGroup() 
        {
            string htmlString = "<b>Inner <b>important</b> text</b>";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
            Assert.AreEqual(
                "b",
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
        public void ProperlyClosedBlockTagWithoutAtLeastTwoPrecedingAndSucceedingLineBreaksIsTagGroupWhenLineBreaksNotRequired(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
            int numberOfTagGroups = 0;
            foreach (HtmlElement element in elements)
            {
                if (element.IsTagGroup)
                {
                    numberOfTagGroups++;
                    Assert.AreEqual(
                        "p",
                        element.GroupDisplayType().Name
                    );
                }
            }
            Assert.AreEqual(
                1,
                numberOfTagGroups
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
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
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
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
        public void ProperlyClosedBlockTagWithoutSurroundingLineBreaksWithInnerTextIsATagGroupWhenLineBreaksNotRequired() 
        {
            string htmlString = "<p>Inside the paragraph</p>";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.NotRequired
            );
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
        public void ProperlyClosedBlockTagWithInnerTextAndSurroundingLineBreaksIsATagGroupWhenLineBreaksRequired() 
        {
            string htmlString = "\n\n<p>Inside the paragraph</p>\n\n";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
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
        public void ProperlyClosedBlockTagWhichContainsProperlyClosedInlineTagIsASingleBlockTagGroup() 
        {
            string htmlString = "\n\n<p>Inside <b>the</b> paragraph</p>\n\n";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
            Assert.AreEqual(
                HtmlDisplayType.Block,
                elements[0].GroupDisplayType().Type
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("\n\n<p>Inside <b>the paragraph</p>\n\n")]
        [DataRow("\n\n<p>Inside the</b> paragraph</p>\n\n")]
        [DataRow("\n\n<p>Inside <b>the</i> paragraph</p>\n\n")]
        public void ProperlyClosedBlockTagWhichContainsImproperlyClosedInlineTagIsASingleBlockTagGroup(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
            Assert.AreEqual(
                HtmlDisplayType.Block,
                elements[0].GroupDisplayType().Type
            );
        }


        [TestMethod]
        [Timeout(500)]
        public void ProperlyClosedBlockTagWhichContainsProperlyClosedBlockTagOfDifferentNameIsASingleBlockTagGroup() 
        {
            string htmlString = "\n\n<p>Inside <div>the</div> paragraph</p>\n\n";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
            Assert.AreEqual(
                HtmlDisplayType.Block,
                elements[0].GroupDisplayType().Type
            );
            Assert.AreEqual(
                "p",
                elements[0].GroupDisplayType().Name
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("\n\n<p>Inside <div>the paragraph</p>\n\n")]
        [DataRow("\n\n<p>Inside the</div> paragraph</p>\n\n")]
        [DataRow("\n\n<p>Inside <div>the</article> paragraph</p>\n\n")]
        public void ProperlyClosedBlockTagWhichContainsImproperlyClosedBlockTagOfDifferentNameIsASingleBlockTagGroup(
            string htmlString
        ) {
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
            Assert.AreEqual(
                1,
                elements.Length
            );
            Assert.IsTrue(
                elements[0].IsTagGroup
            );
            Assert.AreEqual(
                HtmlDisplayType.Block,
                elements[0].GroupDisplayType().Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void BlockTagClosedWithExtraClosingTagInsideWithoutLineBreaksIsNotATagGroup() 
        {
            string htmlString = "\n\n<p>Yes</p>No</p>\n\n";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
            foreach (HtmlElement element in elements)
            {
                Assert.IsFalse(
                    element.IsTagGroup
                );
            }
        }


        [TestMethod]
        [Timeout(500)]
        public void BlockTagClosedWithExtraOpeningTagInsideWithoutLineBreaksIsNotATagGroup() 
        {
            string htmlString = "\n\n<p>Yes<p>No</p>\n\n";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
            foreach (HtmlElement element in elements)
            {
                Assert.IsFalse(
                    element.IsTagGroup
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void ProperlyClosedBlockTagContaingProperlyClosedTagOfSameNameWithoutLineBreaksIsSingleTagGroup() 
        {
            string htmlString = "\n\n<div>Yes<div>No</div>Maybe</div>\n\n";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
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
        public void ProperlyClosedBlockTagContaingProperlyClosedTagOfSameNameWithLineBreaksIsSingleTagGroup() 
        {
            string htmlString = "\n\n<div>Yes\n\n<div>No</div>\n\nMaybe</div>\n\n";
            HtmlSnippet[] snippets = snippetsFromHtmlString(htmlString);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets,
                LineBreaksAroundBlocks.Required
            );
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