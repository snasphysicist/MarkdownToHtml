
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownInlineCodeTests
    {

        [DataTestMethod]
        [DataRow("`test1`", "<p><code>test1</code></p>")]
        [DataRow("test1`test2`test3", "<p>test1<code>test2</code>test3</p>")]
        public void ShouldParseCorrectlyFormattedInlineCodeSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                new string[] {
                    markdown
                }
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.Content[0].ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [DataRow("`te\\`st1`", "<p><code>te`st1</code></p>")]
        public void ShouldParseCorrectlyEscapedStrikethroughCharactersSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                new string[] {
                    markdown
                }
            );
            Assert.IsTrue(
                parser.Success
            );
            string html = parser.Content[0].ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [DataRow("`test1")]
        public void ShouldNotParseIncorrectlyDelimitedStrikethroughFail(
            string markdown
        ) {
            MarkdownParser parser = new MarkdownParser(
                new string[] {
                    markdown
                }
            );
            Assert.IsFalse(
                parser.Success
            );
        }

    }
}
