
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownParagraphTests
    {
        [TestMethod]
        [Timeout(500)]
        [Ignore]
        public void ShouldParseTwoAdjacentLinesAsOneParagraphSuccess() {
            string[] testData = new string[]
            {
                "test1",
                "test2"
            };
            string targetHtml = "<p>test1 test2</p>";
            MarkdownParser parser = new MarkdownParser(
                testData
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [TestMethod]
        [Timeout(500)]
        [Ignore]
        public void ShouldParseTwoSeparatedLinesAsTwoParagraphsSuccess() {
            string[] testData = new string[]
            {
                "test1",
                " ",
                "test2"
            };
            string targetHtml = "<p>test1</p><p>test2</p>";
            MarkdownParser parser = new MarkdownParser(
                testData
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [TestMethod]
        [Timeout(500)]
        [Ignore]
        public void ShouldParseTwoAdjacentLinesWithTwoSpacesAsOneParagraphWithLineBreakSuccess() {
            string[] testData = new string[]
            {
                "test1  ",
                "test2"
            };
            string targetHtml = "<p>test1<br>test2</p>";
            MarkdownParser parser = new MarkdownParser(
                testData
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