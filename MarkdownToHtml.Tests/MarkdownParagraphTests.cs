
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
    }
}