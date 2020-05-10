
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownStrikethroughTests
    {

        [DataTestMethod]
        [DataRow("~~test1~~", "<p><s>test1</s></p>")]
        [DataRow("test1~~test2~~test3", "<p>test1<s>test2</s>test3</p>")]
        public void ShouldParseCorrectlyFormattedStrikethroughSuccess(
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
        [DataRow("~~te\\~st1~~", "<p><s>te~st1</s></p>")]
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
        [DataRow("~~test1")]
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