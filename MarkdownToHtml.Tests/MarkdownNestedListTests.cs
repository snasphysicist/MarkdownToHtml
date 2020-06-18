
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownNestedListTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("1. test1\n    1. test2", "<ol><li>test1<ol><li>test2</li></ol></li></ol>")]
        [DataRow("1. test1\n     1. test2", "<ol><li>test1<ol><li>test2</li></ol></li></ol>")]
        [DataRow("1. test1\n      1. test2", "<ol><li>test1<ol><li>test2</li></ol></li></ol>")]
        [DataRow("1. test1\n       1. test2", "<ol><li>test1<ol><li>test2</li></ol></li></ol>")]
        public void OrderedListItemOneIdentationLevelInFromListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                new string[] 
                {
                    markdown
                }
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

    }
}
