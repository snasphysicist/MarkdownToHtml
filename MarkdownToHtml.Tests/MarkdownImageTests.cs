
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownImageTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "![alttext](url)", 
            "<p><img src=\"url\" alt=\"alttext\" title=\"\"></img></p>"
        )]
        public void ShouldParseCorrectlyFormattedImageAdjacentNoTitleSuccess(
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
        [DataRow(
            "![alttext](url \"title\")", 
            "<p><img src=\"url\" alt=\"alttext\" title=\"title\"></img></p>"
        )]
        public void ShouldParseCorrectlyFormattedImageAdjacentWithTitleSuccess(
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
        [DataRow(
            "![alttext][ref]\n\n\n[ref]: url", 
            "<p><img src=\"url\" alt=\"alttext\" title=\"\"></img></p>"
        )]
        public void ShouldParseCorrectlyFormattedImageReferenceNoTitleSuccess(
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
        [DataRow(
            "![alttext][ref]\n\n\n[ref]: url \"title\"", 
            "<p><img src=\"url\" alt=\"alttext\" title=\"title\"></img></p>"
        )]
        public void ShouldParseCorrectlyFormattedImageReferenceWithTitleSuccess(
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
        [DataRow(
            "test1 ![alttext](url) test2", 
            "<p>test1 <img src=\"url\" alt=\"alttext\" title=\"\"></img> test2</p>"
        )]
        public void ShouldParseCorrectlyFormattedImageEmbeddedSuccess(
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
