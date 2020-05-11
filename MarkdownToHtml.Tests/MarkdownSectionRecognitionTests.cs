
/*
 * This set of tests is a bit different to the others since it is not intended to
 * test the parsing of a specific markdown element, but its purpose is
 * instead to test whether the parser can split different 'groups' of lines
 * into sections correctly. 
 * This happens even before a group of lines is parsed as specific element type
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class MarkdownSectionRecognitionTests
    {

        /*
         * Lines of the same element type (here, quote)
         * which are separated by many lines of whitespace
         * should be parsed into a single markdown element
         */
        [DataTestMethod]
        [DataRow(">test1\n\n\n>test2", "<blockquote><p>test1 test2</p></blockquote>")]
        public void ShouldGroupLinesSameTypeSeparatedByManyWhitespaceLines(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split('\n')
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

        /*
         * Lines of the same element type (here, quote)
         * which are separated by at least one
         * non whitespace line of different type should not
         * be parsed into a single markdown element
         */
        [DataTestMethod]
        [DataRow(
           ">test1\n\ntest2\n\n>test3", 
            "<blockquote><p>test1</p></blockquote><p>test2</p>"
            + "<blockquote><p>test3</p></blockquote>"
        )]
        public void ShouldNotGroupLinesSameTypeSeparatedByLineOfDifferentType(
            string markdown,
            string targetHtml
        ) {
            MarkdownParser parser = new MarkdownParser(
                markdown.Split('\n')
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