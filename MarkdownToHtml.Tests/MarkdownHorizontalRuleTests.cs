
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownHorizontalRuleTests
    {

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("---", "<hr>")]
        [DataRow("***", "<hr>")]
        [DataRow("----", "<hr>")]
        [DataRow("*****", "<hr>")]
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
        [DataRow("---*", "<p>---*</p>")]
        [DataRow("*** test", "<p><em>*</em> test</p>")]
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
