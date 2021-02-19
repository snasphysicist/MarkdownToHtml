
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownPlainTextTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("Test1", "<p>Test1</p>\n")]
        public void SinglePlainTextLineParsedAsParagraph(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            // Should produce exactly one piece of content
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }
        
    }
}
