
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownParagraphTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test1\ntest2", "<p>test1 test2</p>")]
        public void TwoAdjacentLinesAreParsedWithOneSpaceBetweenAsOneParagraph(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test1\n\ntest2", "<p>test1</p><p>test2</p>")]
        public void TwoLinesSeparatedByAWhitespaceLineAreParsedAsTwoParagraphs(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test1  \ntest2", "<p>test1<br>test2</p>")]
        public void TwoAdjacentLinesSeparatedByLinebreakWhenFirstEndsInTwoSpaces(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }
        
    }
}