
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownSingleLineHeadingTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("#test", "<h1>test</h1>\n")]
        [DataRow("##test", "<h2>test</h2>\n")]
        [DataRow("###test", "<h3>test</h3>\n")]
        [DataRow("####test", "<h4>test</h4>\n")]
        [DataRow("#####test", "<h5>test</h5>\n")]
        [DataRow("######test", "<h6>test</h6>\n")]
        public void ShouldParseCorrectlyFormattedNoSpaceSingleLineHeadingSuccess(
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
        [DataRow("#   test", "<h1>test</h1>\n")]
        public void ShouldParseCorrectlyFormattedWithSpaceSingleLineHeadingSuccess(
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
        [DataRow("#test#", "<h1>test</h1>\n")]
        [DataRow("#test###########", "<h1>test</h1>\n")]
        public void ShouldParseCorrectlyFormattedWithTrailingHashesSingleLineHeadingSuccess(
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
        [DataRow("#######test", "<h6>#test</h6>\n")]
        [DataRow("##########test", "<h6>####test</h6>\n")]
        public void ShouldParseTooManyLeadingHashesSingleLineHeadingSuccess(
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
        [DataRow(" # test", "<p># test</p>")]
        public void ShouldParseIncorrectlyFormattedSingleLineHeadingAsTextSuccess(
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
