
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlElementSubstituterLongInputTest
    {
        private Guid FindKeyForValueOrDefault(
            Dictionary <Guid, string> replacements,
            string search
        ) {
            foreach (KeyValuePair<Guid, string> entry in replacements)
            {
                if (entry.Value == search)
                {
                    return entry.Key;
                }
            }
            throw new AssertFailedException("The provided exact string \"" + search + "\" was not in the list of replaced values");
        }

        private string[] SplitByManyStrings(
            string toSplit,
            string[] splitKeys
        ) {
            LinkedList<string> oldParts = new LinkedList<string>();
            oldParts.AddLast(toSplit);
            LinkedList<string> newParts = new LinkedList<string>();
            foreach(string splitKey in splitKeys)
            {
                newParts.Clear();
                foreach (string old in oldParts)
                {
                    string[] split = old.Split(splitKey);
                    for (int i = 0; i < split.Length; i++)
                    {
                        newParts.AddLast(split[i]);
                    }
                }
                oldParts.Clear();
                foreach (string part in newParts)
                {
                    oldParts.AddLast(part);
                }
            }
            string[] output = new string[newParts.Count];
            LinkedListNode<string> current = newParts.First;
            for (int i = 0; i < oldParts.Count; i++)
            {
                output[i] = current.Value;
                current = current.Next;
            }
            return output;
        }

        [TestMethod]
        [Timeout(500)]
        public void HtmlElementSubstituterLongInput() {
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
            // <span>Just one <em>last</em> check</span>
            // 
            // > Sure, quote me
            string[] notReplaced = new string[16];
            string[] replaced = new string[16];
            notReplaced[0] = "This is some *standard* Markdown text\n\n";
            replaced[0] = "<p attr=\"Something\">Some inline <b>HTML</b> </p>";
            notReplaced[1] = "\n\n# Markdown Heading ";
            replaced[1] = "<i>";
            notReplaced[2] = "heaaadiiing";
            replaced[2] = "</i>";
            notReplaced[3] = "\n";
            replaced[3] = "<div> Oh no, where are the lines new lines???</div>";
            notReplaced[4] = "\r\n\r<i> Well this ";
            replaced[4] = "<b class=\"youreaboldone\">";
            notReplaced[5] = "is fine";
            replaced[5] = "</b>";
            notReplaced[6] = " but ";
            replaced[6] = "<div>this is ignored</div>";
            notReplaced[7] = "</i>\n\r<span>\n\n";
            replaced[7] = "<div>This should well fail, however</div>";
            notReplaced[8] = "\n\n</span>\n\nWhy not just do this\n\n";
            replaced[8] = "<hr />";
            notReplaced[9] = "\n\nin markdown? ";
            replaced[9] = "<sarcasm>";
            notReplaced[10] = "Your choice...";
            replaced[10] = "</sarcasm>";
            notReplaced[11] = "\r\n\r\n";
            replaced[11] = "<span>";
            notReplaced[12] = "Just one ";
            replaced[12] = "<em>";
            notReplaced[13] = "last";
            replaced[13] = "</em>";
            notReplaced[14] = " check";
            replaced[14] = "</span>";
            notReplaced[15] = "\n\n> Sure, quote me";
            replaced[15] = "";
            string markdown = "";
            for (int i = 0; i < 16; i++)
            {
                markdown = markdown + notReplaced[i] + replaced[i];
            }
            HtmlTokeniser tokeniser = new HtmlTokeniser(markdown);
            HtmlToken[] tokens = tokeniser.tokenise();
            HtmlSnippet[] tags = HtmlTagDetector.TagsFromTokens(tokens);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                tags,
                LineBreaksAroundBlocks.NotRequired
            );
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            Assert.AreEqual(
                15,
                substituter.GetReplacements().Count
            );
            Guid[] guids = new Guid[15];
            string[] guidStrings = new string[15];
            for (int i = 0; i < 15; i++)
            {
                guids[i] = FindKeyForValueOrDefault(
                    substituter.GetReplacements(),
                    replaced[i]
                );
                guidStrings[i] = guids[i].ToString();
            }
            string[] partsThatShouldBeUnreplaced = SplitByManyStrings(
                substituter.Processed,
                guidStrings
            );
            Assert.AreEqual(
                notReplaced.Length,
                partsThatShouldBeUnreplaced.Length
            );
            for (int i = 0; i < notReplaced.Length; i++)
            {
                Assert.AreEqual(
                    notReplaced[i],
                    partsThatShouldBeUnreplaced[i]
                );
            }
            for (int i = 0; i < guids.Length; i++)
            {
                Assert.AreEqual(
                    replaced[i],
                    substituter.GetReplacements()[guids[i]]
                );
            }
        }
    }
}