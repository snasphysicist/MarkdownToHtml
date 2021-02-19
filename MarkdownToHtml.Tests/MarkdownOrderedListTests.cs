
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownOrderedListTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("1. test1", "<ol><li>test1</li></ol>")]
        [DataRow(" 1. test1", "<ol><li>test1</li></ol>")]
        [DataRow("  1. test1", "<ol><li>test1</li></ol>")]
        [DataRow("   1. test1", "<ol><li>test1</li></ol>")]
        public void ShouldParseOrderedListZeroToThreeSpacesSuccess(
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
        [DataRow("19274. test1", "<ol><li>test1</li></ol>")]
        public void ShouldParseOrderedListNotStartAtOneSuccess(
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
        [DataRow("1. test1\n2. test2", "<ol><li>test1</li><li>test2</li></ol>")]
        public void ShouldParseOrderedListLinesNotSeparatedByWhitespaceAscendingSuccess(
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
        [DataRow("39438. test1\n749. test2", "<ol><li>test1</li><li>test2</li></ol>")]
        public void ShouldParseOrderedListLinesNotSeparatedByWhitespaceDescendingSuccess(
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
            "1. test1\n\n2. test2", 
            "<ol><li><p>test1</p>\n</li><li><p>test2</p>\n</li></ol>"
        )]
        public void ShouldParseOrderedListLinesSeparatedByWhitespaceSuccess(
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
            "1. test1\n\ntest2\n\n2. test3", 
            "<ol><li>test1</li></ol><p>test2</p>\n<ol><li>test3</li></ol>"
        )]
        public void ShouldParseOrderedListLinesSeparatedByNormalParagraphSuccess(
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
        [DataRow("test1\n2. test2", "<p>test1 2. test2</p>\n")]
        public void ShouldParseOrderedListLineAfterParagraphAsParagraphSuccess(
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
            "1. test1\n\n    test2\ntest3", 
            "<ol><li><p>test1</p>\n<p>test2 test3</p>\n</li></ol>"
        )]
        [DataRow(
            "1. test1\n\n    test2\n    test3", 
            "<ol><li><p>test1</p>\n<p>test2 test3</p>\n</li></ol>"
        )]
        public void IndentedFollowingParagraphParsedAsPartOfListItem(
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
            "1. test1\n\n    > test2", 
            "<ol><li><p>test1</p>\n<blockquote><p>test2</p>\n</blockquote></li></ol>"
        )]
        public void IndentedQuoteBlockParsedAsPartOfListItem(
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
