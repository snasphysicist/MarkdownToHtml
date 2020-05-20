
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml 
{
    [TestClass]
    public class MarkdownListTests
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("19274. test1", "<ol><li>test1</li></ol>")]
        public void ShouldParseOrderedListNotStartAtOneSuccess(
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

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("1. test1\n2. test2", "<ol><li>test1</li><li>test2</li></ol>")]
        public void ShouldParseOrderedListLinesNotSeparatedByWhitespaceAscendingSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
                markdown.Split("\n")
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
            "<ol><li><p>test1</p></li><li><p>test2</p></li></ol>"
        )]
        public void ShouldParseOrderedListLinesSeparatedByWhitespaceSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
            "<ol><li>test1</li></ol><p>test2</p><ol><li>test3</li></ol>"
        )]
        public void ShouldParseOrderedListLinesSeparatedByNormalParagraphSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
        [DataRow("test1\n2. test2", "<p>test1 2. test2</p>")]
        public void ShouldParseOrderedListLineAfterParagraphAsParagraphSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
            "1. test1\n\n2. test2\n3. test3\n4. test4\n\n5. test5", 
            "<ol><li><p>test1</p></li><li><p>test2</p></li><li>test3</li>"
            + "<li><p>test4</p></li><li><p>test5</p></li></ol>"
        )]
        public void ShouldParseOrderedListLinesAdjacentToWhitespaceLineWithParagraphsSuccess(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split("\n")
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
