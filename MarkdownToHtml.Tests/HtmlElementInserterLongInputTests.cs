
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlElementInserterLongInputTests
    {
        [TestMethod]
        [Timeout(500)]
        public void HtmlElementInserterLongInput() {
            string markdown = "This is some *standard* Markdown text\n\n";
            markdown = markdown + "<p attr=\"Something\">Some inline <b>HTML</b> </p>\n\n";
            markdown = markdown + "# Markdown Heading <i>heaaadiiing</i>\n";
            markdown = markdown + "<div> Oh no, where are the lines new lines???</div>\n\n";
            markdown = markdown + "<i> Well this <b class=\"youreaboldone\">is fine</b> and <div>this is ignored</div></i>\n\n";
            markdown = markdown + "<span>\n\n<div>This should well fail, however</div>\n\n</span>\n\n";
            markdown = markdown + "Why not just do this\n\n";
            markdown = markdown + "<hr />\n\n";
            markdown = markdown + "in markdown? <sarcasm>Your choice...</sarcasm>\n\n";
            markdown = markdown + "<span>Just one <em>last</em> check</span>\n\n";
            markdown = markdown + "> Sure, quote me\n";
            HtmlTokeniser tokeniser = new HtmlTokeniser(markdown);
            HtmlToken[] tokens = tokeniser.tokenise();
            HtmlSnippet[] tags = HtmlTagDetector.TagsFromTokens(tokens);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                tags,
                LineBreaksAroundBlocks.NotRequired
            );
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            HtmlElementInserter inserter = new HtmlElementInserter(
                substituter.GetReplacements(),
                substituter.Processed
            );
            Assert.AreEqual(
                markdown,
                inserter.Processed
            );
        }
    }
}