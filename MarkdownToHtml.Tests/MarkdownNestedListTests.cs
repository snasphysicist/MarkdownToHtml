
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
        public void OrderedListItemOneIdentationLevelInFromOrderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
        [DataRow("1. test1\n    * test2", "<ol><li>test1<ul><li>test2</li></ul></li></ol>")]
        [DataRow("1. test1\n     + test2", "<ol><li>test1<ul><li>test2</li></ul></li></ol>")]
        [DataRow("1. test1\n      - test2", "<ol><li>test1<ul><li>test2</li></ul></li></ol>")]
        [DataRow("1. test1\n       * test2", "<ol><li>test1<ul><li>test2</li></ul></li></ol>")]
        public void UnorderedListItemOneIdentationLevelInFromOrderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
        [DataRow("* test1\n    1. test2", "<ul><li>test1<ol><li>test2</li></ol></li></ul>")]
        [DataRow("+ test1\n     1. test2", "<ul><li>test1<ol><li>test2</li></ol></li></ul>")]
        [DataRow("- test1\n      1. test2", "<ul><li>test1<ol><li>test2</li></ol></li></ul>")]
        [DataRow("* test1\n       1. test2", "<ul><li>test1<ol><li>test2</li></ol></li></ul>")]
        public void OrderedListItemOneIdentationLevelInFromUnorderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
        [DataRow("* test1\n    + test2", "<ul><li>test1<ul><li>test2</li></ul></li></ul>")]
        [DataRow("+ test1\n     - test2", "<ul><li>test1<ul><li>test2</li></ul></li></ul>")]
        [DataRow("- test1\n      * test2", "<ul><li>test1<ul><li>test2</li></ul></li></ul>")]
        [DataRow("* test1\n       + test2", "<ul><li>test1<ul><li>test2</li></ul></li></ul>")]
        public void UnorderedListItemOneIdentationLevelInFromUnorderedListItemCreatesNestedOrderedList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
            "<ol><li>test1</li><li>test2<ol><li>test3</li><li>test4</li></ol></li></ol>"
        )]
        public void MultilineListNestedInMultilineListItemsParsedIntoInnerList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
            "<ol><li>test1</li><li>test2<ol><li>test3</li><li>test4</li></ol></li>" 
            + "<li>test5</li></ol>"
        )]
        public void UnindentedListItemAfterNestedListAddedToOuterList(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
            "<ol><li>test1 test2<ol><li>test3</li></ol></li></ol>"
        )]
        public void AllTextInListItemBeforeNestedListIncludedInSameStructure(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
            "1. test1\n\n    test2\ntest3", 
            "<ol><li><p>test1</p><p>test2 test3</p></li></ol>"
        )]
        [DataRow(
            "1. test1\n\n    test2\n    test3", 
            "<ol><li><p>test1</p><p>test2 test3</p></li></ol>"
        )]
        public void IndentedFollowingParagraphParsedAsPartOfListItem(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split(
                    "\n"
                )
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
