
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class MarkdownBlockCodeTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("```\ntest1\n```", "<p><code>test1</code></p>\n")]
        [DataRow("test1\n\n```\ntest2\n```", "<p>test1</p>\n<p><code>test2</code></p>\n")]
        public void ParseProperlyBacktickDelimitedCodeBlockAsCode(
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
        [DataRow("```\ntest1\n\ntest2", "<p>``` test1</p>\n<p>test2</p>\n")]
        public void ParseImproperlyBacktickDelimitedCodeBlockAsParagraph(
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
        [DataRow("    test1", "<pre><code>test1</code></pre>\n")]
        public void ParseProperlyIndentedCodeBlockAsPreCode(
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
        [DataRow("     test1", "<pre><code> test1</code></pre>\n")]
        public void PreserveSpacesInIndentedCodeBlockAfterFirstFour(
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
        [DataRow("    test1\n    test2", "<pre><code>test1\ntest2</code></pre>\n")]
        public void GroupAdjacentIndentedLinesIntoSamePreCodeBlock(
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
        [DataRow("    test1\n\n    test2", "<pre><code>test1\n\ntest2</code></pre>\n")]
        public void PreserveEmptyLinesBetweenIndentedLinesInPreCodeBlock(
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
        [DataRow("   test1", "<p>test1</p>\n")]
        public void ParseThreeSpaceIndentedLineAsParagraph(
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