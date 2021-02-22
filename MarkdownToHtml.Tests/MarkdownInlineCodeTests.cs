
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownInlineCodeTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("`test1`", "<p><code>test1</code></p>\n")]
        [DataRow("test1`test2`test3", "<p>test1<code>test2</code>test3</p>\n")]
        public void ShouldParseCorrectlyFormattedInlineCodeSuccess(
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
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("`te\\`st1`", "<p><code>te`st1</code></p>\n")]
        public void ShouldParseCorrectlyEscapedStrikethroughCharactersSuccess(
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
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("`test1", "<p>`test1</p>\n")]
        public void ShouldParseIncorrectlyDelimitedInlineCodeAsParagraphSuccess(
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
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("``Markdown code like `this`, right?``", "<p><code>Markdown code like `this`, right?</code></p>\n")]
        [DataRow("```Using `double quotes` like ``this``, though```", "<p><code>Using `double quotes` like ``this``, though</code></p>\n")]
        public void TextSurroundedByNumberOfQuotesUsedInCodeBlockPlusOneIsParsedAsCodeBlock(
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
