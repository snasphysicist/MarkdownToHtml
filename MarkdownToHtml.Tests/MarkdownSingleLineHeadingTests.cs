
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownSingleLineHeadingTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("#test", "<h1>test</h1>")]
        [DataRow("##test", "<h2>test</h2>")]
        [DataRow("###test", "<h3>test</h3>")]
        [DataRow("####test", "<h4>test</h4>")]
        [DataRow("#####test", "<h5>test</h5>")]
        [DataRow("######test", "<h6>test</h6>")]
        public void ShouldParseCorrectlyFormattedNoSpaceSingleLineHeadingSuccess(
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
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("#   test", "<h1>test</h1>")]
        public void ShouldParseCorrectlyFormattedWithSpaceSingleLineHeadingSuccess(
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
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("#test#", "<h1>test</h1>")]
        [DataRow("#test###########", "<h1>test</h1>")]
        public void ShouldParseCorrectlyFormattedWithTrailingHashesSingleLineHeadingSuccess(
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
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow("#######test", "<h6>#test</h6>")]
        [DataRow("##########test", "<h6>####test</h6>")]
        public void ShouldParseTooManyLeadingHashesSingleLineHeadingSuccess(
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
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [Ignore]
        [DataRow(" # test", "<p># test</p>")]
        public void ShouldParseIncorrectlyFormattedSingleLineHeadingAsTextSuccess(
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
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

    }
}
