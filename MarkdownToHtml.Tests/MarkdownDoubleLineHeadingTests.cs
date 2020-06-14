
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownDoubleLineHeadingTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("test1\n=", "<h1>test1</h1>")]
        [DataRow("test1\n=====", "<h1>test1</h1>")]
        [DataRow("test1\n====================", "<h1>test1</h1>")]
        public void ShouldParseHeadingLineFollowedByOnlyEqualsSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [Ignore]
        [DataRow("test1\n=1", "<p>test1 =1</p>")]
        public void ShouldNotParseEqualsHeadingLineFollowedByIncorrectCharacterSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [Ignore]
        [DataRow("test1\n-", "<h2>test1</h2>")]
        [DataRow("test1\n-----", "<h2>test1</h2>")]
        [DataRow("test1\n--------------------", "<h2>test1</h2>")]
        public void ShouldParseHeadingLineFollowedByOnlyDashSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [Ignore]
        [DataRow("test1\n-1", "<p>test1 -1</p>")]
        public void ShouldNotParseDashHeadingLineFollowedByIncorrectCharacterSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
