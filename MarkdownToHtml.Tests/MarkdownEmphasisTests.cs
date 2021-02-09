
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownEmphasisTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("*test1*", "<p><em>test1</em></p>")]
        [DataRow("test1*test2*test3", "<p>test1<em>test2</em>test3</p>")]
        public void ShouldParseCorrectlyFormattedStarEmphasisLineSuccess(
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
        [DataRow("_test1_", "<p><em>test1</em></p>")]
        [DataRow("test1_test2_test3", "<p>test1<em>test2</em>test3</p>")]
        public void ShouldParseCorrectlyFormattedUnderscoreEmphasisLineSuccess(
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
        [DataRow("*te\\*st1*", "<p><em>te*st1</em></p>")]
        [DataRow("_te\\_st1_", "<p><em>te_st1</em></p>")]
        public void ShouldParseCorrectlyEscapedEmphasisCharactersSuccess(
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
        [DataRow("*test1", "<p>*test1</p>")]
        [DataRow("_test1", "<p>_test1</p>")]
        public void ShouldNotParseIncorrectlyDelimitedEmphasisFail(
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
    }
}
