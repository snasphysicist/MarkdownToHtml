
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownStrongTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("**test1**", "<p><strong>test1</strong></p>")]
        [DataRow("test1**test2**test3", "<p>test1<strong>test2</strong>test3</p>")]
        public void ShouldParseCorrectlyFormattedStarStrongSuccess(
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
            string html = parser.ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("__test1__", "<p><strong>test1</strong></p>")]
        [DataRow("test1__test2__test3", "<p>test1<strong>test2</strong>test3</p>")]
        public void ShouldParseCorrectlyFormattedUnderscoreStrongSuccess(
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
            string html = parser.ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("**te\\*st1**", "<p><strong>te*st1</strong></p>")]
        [DataRow("__te\\_st1__", "<p><strong>te_st1</strong></p>")]
        public void ShouldParseCorrectlyEscapedStrongCharactersSuccess(
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
            string html = parser.ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("**test1", "<p>**test1</p>")]
        [DataRow("__test1", "<p>__test1</p>")]
        public void ShouldParseIncorrectlyDelimitedStrongAsParagraphSuccess(
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
            string html = parser.ToHtml();
            // Check that the correct HTML is produced
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

    }
}
