
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownNestedListTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("1. test1\n    1. test2", "<ol><li>test1<ol><li>test2</li>\n</ol></li>\n</ol>")]
        [DataRow("1. test1\n     1. test2", "<ol><li>test1<ol><li>test2</li>\n</ol></li>\n</ol>")]
        [DataRow("1. test1\n      1. test2", "<ol><li>test1<ol><li>test2</li>\n</ol></li>\n</ol>")]
        [DataRow("1. test1\n       1. test2", "<ol><li>test1<ol><li>test2</li>\n</ol></li>\n</ol>")]
        public void OrderedListItemOneIdentationLevelInFromOrderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("1. test1\n    * test2", "<ol><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ol>")]
        [DataRow("1. test1\n     + test2", "<ol><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ol>")]
        [DataRow("1. test1\n      - test2", "<ol><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ol>")]
        [DataRow("1. test1\n       * test2", "<ol><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ol>")]
        public void UnorderedListItemOneIdentationLevelInFromOrderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("* test1\n    1. test2", "<ul><li>test1<ol><li>test2</li>\n</ol></li>\n</ul>\n")]
        [DataRow("+ test1\n     1. test2", "<ul><li>test1<ol><li>test2</li>\n</ol></li>\n</ul>\n")]
        [DataRow("- test1\n      1. test2", "<ul><li>test1<ol><li>test2</li>\n</ol></li>\n</ul>\n")]
        [DataRow("* test1\n       1. test2", "<ul><li>test1<ol><li>test2</li>\n</ol></li>\n</ul>\n")]
        public void OrderedListItemOneIdentationLevelInFromUnorderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("* test1\n    + test2", "<ul><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ul>\n")]
        [DataRow("+ test1\n     - test2", "<ul><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ul>\n")]
        [DataRow("- test1\n      * test2", "<ul><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ul>\n")]
        [DataRow("* test1\n       + test2", "<ul><li>test1<ul><li>test2</li>\n</ul>\n</li>\n</ul>\n")]
        public void UnorderedListItemOneIdentationLevelInFromUnorderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "1. test1\n2. test2\n    1. test3\n    2. test4", 
            "<ol><li>test1</li>\n<li>test2<ol><li>test3</li>\n<li>test4</li>\n</ol></li>\n</ol>"
        )]
        public void MultilineListNestedInMultilineListItemsParsedIntoInnerList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "1. test1\n2. test2\n    1. test3\n    2. test4\n3. test5", 
            "<ol><li>test1</li>\n<li>test2<ol><li>test3</li>\n<li>test4</li>\n</ol></li>\n" 
            + "<li>test5</li>\n</ol>"
        )]
        public void UnindentedListItemAfterNestedListAddedToOuterList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
            );
            Assert.IsTrue(
                parser.Success
            );
            Assert.AreEqual(
                targetHtml,
                parser.ToHtml()
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(
            "1. test1\ntest2\n    2. test3", 
            "<ol><li>test1 test2<ol><li>test3</li>\n</ol></li>\n</ol>"
        )]
        public void AllTextInListItemBeforeNestedListIncludedInSameStructure(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown
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
