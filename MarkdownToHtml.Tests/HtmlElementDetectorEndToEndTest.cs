
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlElementDetectorEndToEndTest
    {
        private int current;            

        HtmlElement[] elements;

        private void AssertTagGroup(
            string content
        ) {
            Assert.IsTrue(elements[current].IsTagGroup);
            Assert.AreEqual(
                content,
                elements[current].AsHtmlString()
            );
            current++;
        }

        private void AssertNotTagGroup(
            string content
        ) {
            Assert.IsFalse(elements[current].IsTagGroup);
            Assert.AreEqual(
                content,
                elements[current].AsHtmlString()
            );
            current++;
        }

        [TestMethod]
        [Timeout(500)]
        public void LongInputTestWithLineBreaksRequired()
        {
            // This is some *standard* Markdown text
            // 
            // <p = "Something">Some inline <b>HTML</b> </p>
            // 
            // # Markdown Heading <i>heaaadiiing</i>
            // <div> Oh no, where are the lines new lines???</div>
            // 
            // <i> Well this <b class="youreaboldone">is fine</b> and <div>this is ignored</div></i>
            //
            // <span>\n\n<div>This should well fail, however</div>\n\n</span>
            //
            // Why not just do this
            //
            // <hr />
            //
            // in markdown? <sarcasm>Your choice...</sarcasm>
            //
            // <span>Just one <b>last</b> check</b>
            // 
            // > Sure, quote me
            string markdown = "This is some *standard* Markdown text\n\n";
            markdown = markdown + "<p attr=\"Something\">Some inline <b>HTML</b> </p>\n\n";
            markdown = markdown + "# Markdown Heading <i>heaaadiiing</i>\n";
            markdown = markdown + "<div> Oh no, where are the lines new lines???</div>\r\n";
            markdown = markdown + "\r<i> Well this <b class=\"youreaboldone\">is fine</b> but <div>this is ignored</div></i>\n\r";
            markdown = markdown + "<span>\n\n<div>This should well fail, however</div>\n\n</span>\n\n";
            markdown = markdown + "Why not just do this\n\n";
            markdown = markdown + "<hr />\n\n";
            markdown = markdown + "in markdown? <sarcasm>Your choice...</sarcasm>\r\n\r\n";
            markdown = markdown + "<span>Just one <b>last</b> check</span>\n\n";
            markdown = markdown + "> Sure, quote me";
            HtmlTokeniser tokeniser = new HtmlTokeniser(markdown);
            HtmlToken[] tokens = tokeniser.tokenise();
            HtmlSnippet[] tags = HtmlTagDetector.TagsFromTokens(tokens);
            elements = HtmlElementDetector.ElementsFromTags(
                tags,
                LineBreaksAroundBlocks.Required
            );
            current = 0;
            // This is some *standard* Markdown text
            AssertNotTagGroup("This");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("is");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("some");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("*standard*");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Markdown");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("text");
            // <p attr=\"Something\">Some inline <b>HTML</b> </p>
            AssertTagGroup("\n\n<p attr=\"Something\">Some inline <b>HTML</b> </p>\n\n");
            // # Markdown Heading <i>heaaadiiing</i>
            AssertNotTagGroup("#");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Markdown");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Heading");
            AssertNotTagGroup(" ");
            AssertTagGroup("<i>heaaadiiing</i>");
            AssertNotTagGroup("\n");
            // <div> Oh no, where are the lines new lines???</div>
            AssertNotTagGroup("<div>");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Oh");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("no,");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("where");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("are");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("the");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("lines");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("new");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("lines???");
            AssertNotTagGroup("</div>");
            AssertNotTagGroup("\r\n");
            // <i> Well this <b class=\"youreaboldone\">is fine</b> but <div>this is ignored</div></i>
            AssertNotTagGroup("\r");
            AssertTagGroup("<i> Well this <b class=\"youreaboldone\">is fine</b> but <div>this is ignored</div></i>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\r");
            // <span>\n\n<div>This should well fail, however</div>\n\n</span>
            AssertNotTagGroup("<span>");
            AssertTagGroup("\n\n<div>This should well fail, however</div>\n\n");
            AssertNotTagGroup("</span>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // Why not just do this
            AssertNotTagGroup("Why");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("not");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("just");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("do");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("this");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // <hr />
            AssertTagGroup("<hr />");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // in markdown? <sarcasm>Your choice...</sarcasm>
            AssertNotTagGroup("in");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("markdown?");
            AssertNotTagGroup(" ");
            AssertTagGroup("<sarcasm>Your choice...</sarcasm>");
            AssertNotTagGroup("\r\n");
            AssertNotTagGroup("\r\n");
            // <span>Just one <b>last</b> check</span>
            AssertTagGroup("<span>Just one <b>last</b> check</span>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // > Sure, quote me
            AssertNotTagGroup(">");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Sure,");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("quote");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("me");
        }

        [TestMethod]
        [Timeout(500)]
        public void LongInputTestWithLineBreaksNotRequired()
        {
            // This is some *standard* Markdown text
            // 
            // <p = "Something">Some inline <b>HTML</b> </p>
            // 
            // # Markdown Heading <i>heaaadiiing</i>
            // <div> Oh no, where are the lines new lines???</div>
            // 
            // <i> Well this <b class="youreaboldone">is fine</b> and <div>this is ignored</div></i>
            //
            // <span>\n\n<div>This should well fail, however</div>\n\n</span>
            //
            // Why not just do this
            //
            // <hr />
            //
            // in markdown? <sarcasm>Your choice...</sarcasm>
            //
            // <span>Just one <b>last</b> check</b>
            // 
            // > Sure, quote me
            string markdown = "This is some *standard* Markdown text\n\n";
            markdown = markdown + "<p attr=\"Something\">Some inline <b>HTML</b> </p>\n\n";
            markdown = markdown + "# Markdown Heading <i>heaaadiiing</i>\n";
            markdown = markdown + "<div> Oh no, where are the lines new lines???</div>\r\n";
            markdown = markdown + "\r<i> Well this <b class=\"youreaboldone\">is fine</b> but <div>this is ignored</div></i>\n\r";
            markdown = markdown + "<span>\n\n<div>This should well fail, however</div>\n\n</span>\n\n";
            markdown = markdown + "Why not just do this\n\n";
            markdown = markdown + "<hr />\n\n";
            markdown = markdown + "in markdown? <sarcasm>Your choice...</sarcasm>\r\n\r\n";
            markdown = markdown + "<span>Just one <b>last</b> check</span>\n\n";
            markdown = markdown + "> Sure, quote me";
            HtmlTokeniser tokeniser = new HtmlTokeniser(markdown);
            HtmlToken[] tokens = tokeniser.tokenise();
            HtmlSnippet[] tags = HtmlTagDetector.TagsFromTokens(tokens);
            elements = HtmlElementDetector.ElementsFromTags(
                tags,
                LineBreaksAroundBlocks.NotRequired
            );
            current = 0;
            // This is some *standard* Markdown text
            AssertNotTagGroup("This");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("is");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("some");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("*standard*");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Markdown");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("text");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // <p attr=\"Something\">Some inline <b>HTML</b> </p>
            AssertTagGroup("<p attr=\"Something\">Some inline <b>HTML</b> </p>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // # Markdown Heading <i>heaaadiiing</i>
            AssertNotTagGroup("#");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Markdown");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Heading");
            AssertNotTagGroup(" ");
            AssertTagGroup("<i>heaaadiiing</i>");
            AssertNotTagGroup("\n");
            // <div> Oh no, where are the lines new lines???</div>
            AssertTagGroup("<div> Oh no, where are the lines new lines???</div>");
            AssertNotTagGroup("\r\n");
            // <i> Well this <b class=\"youreaboldone\">is fine</b> but <div>this is ignored</div></i>
            AssertNotTagGroup("\r");
            AssertNotTagGroup("<i>");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Well");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("this");
            AssertNotTagGroup(" ");
            AssertTagGroup("<b class=\"youreaboldone\">is fine</b>");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("but");
            AssertNotTagGroup(" ");
            AssertTagGroup("<div>this is ignored</div>");
            AssertNotTagGroup("</i>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\r");
            // <span>\n\n<div>This should well fail, however</div>\n\n</span>
            AssertNotTagGroup("<span>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            AssertTagGroup("<div>This should well fail, however</div>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("</span>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // Why not just do this
            AssertNotTagGroup("Why");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("not");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("just");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("do");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("this");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // <hr />
            AssertTagGroup("<hr />");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // in markdown? <sarcasm>Your choice...</sarcasm>
            AssertNotTagGroup("in");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("markdown?");
            AssertNotTagGroup(" ");
            AssertTagGroup("<sarcasm>Your choice...</sarcasm>");
            AssertNotTagGroup("\r\n");
            AssertNotTagGroup("\r\n");
            // <span>Just one <b>last</b> check</span>
            AssertTagGroup("<span>Just one <b>last</b> check</span>");
            AssertNotTagGroup("\n");
            AssertNotTagGroup("\n");
            // > Sure, quote me
            AssertNotTagGroup(">");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("Sure,");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("quote");
            AssertNotTagGroup(" ");
            AssertNotTagGroup("me");
        }
    }
}