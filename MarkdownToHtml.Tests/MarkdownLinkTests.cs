
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownLinkTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("[text](url)", "<p><a href=\"url\">text</a></p>")]
        public void ShouldParseCorrectlyFormattedLinkAdjacentSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [DataRow("[text][ref]\n\n\n[ref]: url", "<p><a href=\"url\">text</a></p>")]
        public void ShouldParseCorrectlyFormattedLinkReferenceSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [DataRow("[text]\n\n\n[text]: url", "<p><a href=\"url\">text</a></p>")]
        public void ShouldParseCorrectlyFormattedLinkSelfReferenceSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [DataRow("[text]", "<p>[text]</p>")]
        public void ShouldParseIncorrectlyFormattedLinkAsParagraphSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [DataRow(
            "test1 [test2](test3) test4", 
            "<p>test1 <a href=\"test3\">test2</a> test4</p>"
        )]
        public void ShouldParseCorrectlyFormattedLinkEmbeddedSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
