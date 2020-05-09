
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownParagraphTests
    {
        [DataTestMethod]
        [DataRow("Test1")]
        public void ShouldParseCorrectlyFormattedPlainTextLineSuccess(
            string markdown
        ) {
            MarkdownParser parser = new MarkdownParser(
                new string[] {
                    markdown
                }
            );
            Assert.IsTrue(
                parser.Success
            );
            // Should produce exactly one piece of content
            Assert.IsTrue(
                parser.Content.Length == 1
            );
            string html = parser.Content[0].ToHtml();
            // HTML should contain the provided text
            Assert.IsTrue(
                html.Contains(markdown)
            );
            // HTML should contain open/close paragraph tags
            Assert.IsTrue(
                html.Contains("<p>")
            );
            Assert.IsTrue(
                html.Contains("</p>")
            );
        }

        [DataTestMethod]
        [DataRow("*test1*", "<p><emph>test1</emph></p>")]
        [DataRow("test1*test2*test3", "<p>test1<emph>test2</emph>test3</p>")]
        public void ShouldParseCorrectlyFormattedEmphasisLineSuccess(
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
                html,
                targetHtml
            );
        }
    }
}