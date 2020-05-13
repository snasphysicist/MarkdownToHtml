
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
        [DataRow("___", "<hr>")]
        [DataRow("----", "<hr>")]
        [DataRow("*****", "<hr>")]
        [DataRow("__________", "<hr>")]
        public void ShouldParseCorrectlyFormattedHorizontalRuleSuccess(
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
        [DataRow("---*")]
        [DataRow("*** test")]
        public void ShouldNotParseIncorrectlyFormattedHorizontalRuleFail(
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
