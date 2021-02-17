
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownHorizontalRuleTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("---", "<hr />\n")]
        [DataRow("***", "<hr />\n")]
        [DataRow("----", "<hr />\n")]
        [DataRow("*****", "<hr />\n")]
        public void ShouldParseCorrectlyFormattedHorizontalRuleSuccess(
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
        [DataRow("---*", "<p>---*</p>\n")]
        [DataRow("*** test", "<p><em>*</em> test</p>\n")]
        public void ShouldNotParseIncorrectlyFormattedHorizontalRuleFail(
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
