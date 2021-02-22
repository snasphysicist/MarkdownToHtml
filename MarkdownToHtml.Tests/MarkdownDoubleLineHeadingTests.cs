
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownDoubleLineHeadingTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test1\n=", "<h1>test1</h1>\n")]
        [DataRow("test1\n=====", "<h1>test1</h1>\n")]
        [DataRow("test1\n====================", "<h1>test1</h1>\n")]
        public void ShouldParseHeadingLineFollowedByOnlyEqualsSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test1\n=1", "<p>test1 =1</p>\n")]
        public void ShouldNotParseEqualsHeadingLineFollowedByIncorrectCharacterSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test1\n-", "<h2>test1</h2>\n")]
        [DataRow("test1\n-----", "<h2>test1</h2>\n")]
        [DataRow("test1\n--------------------", "<h2>test1</h2>\n")]
        public void ShouldParseHeadingLineFollowedByOnlyDashSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("test1\n-1", "<p>test1 -1</p>\n")]
        public void ShouldNotParseDashHeadingLineFollowedByIncorrectCharacterSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

    }
}
