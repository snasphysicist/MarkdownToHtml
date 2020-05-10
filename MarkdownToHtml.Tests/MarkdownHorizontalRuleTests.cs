
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownHorizontalRuleTests
    {

        [DataTestMethod]
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
            string html = parser.Content[0].ToHtml();
            Assert.AreEqual(
                targetHtml,
                html
            );
        }

        [DataTestMethod]
        [DataRow("---*")]
        [DataRow("*** test")]
        public void ShouldNotParseIncorrectlyFormattedHorizontalRuleFail(
            string markdown,
            string targetHtml
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
