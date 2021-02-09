
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlSpecialCharacterEscaperLongInputTests
    {
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
        public void HtmlSpecialCharacterEscaperLongInput()
        {
            string[] notEscaped = new string[7];
            string[] escaped = new string[7];
            string[] expected = new string[7];
            notEscaped[0] = "# Heading line\n\nA ";
            escaped[0] = "&";
            expected[0] = "&amp;";
            notEscaped[1] = " paragraph\n\n```\n";
            escaped[1] = "<";
            expected[1] = "&lt;";
            notEscaped[2] = "!-- HTML COMMENT --";
            escaped[2] = ">";
            expected[2] = "&gt;";
            notEscaped[3] = "\n```\n\nAccording to someone\n\n<emph> ";
            escaped[3] = "\"";
            expected[3] = "&quot;";
            notEscaped[4] = "You _can_ quote me";
            escaped[4] = "\"";
            expected[4] = "&quot;";
            notEscaped[5] = "</emph>\n\n======\n\n* Well that";
            escaped[5] = "'";
            expected[5] = "&apos;";
            notEscaped[6] = "s fantastic";
            escaped[6] = "";
            expected[6] = "";
            string markdown = "";
            for (int i = 0; i < 7; i++)
            {
                markdown = markdown + notEscaped[i] + escaped[i];
            }
            HtmlTokeniser tokeniser = new HtmlTokeniser(markdown);
            HtmlToken[] tokens = tokeniser.tokenise();
            HtmlSnippet[] snippets = HtmlTagDetector.TagsFromTokens(tokens);
            HtmlElement[] elements = HtmlElementDetector.ElementsFromTags(
                snippets, 
                LineBreaksAroundBlocks.Required
            );
            HtmlElementSubstituter substituter = new HtmlElementSubstituter(elements);
            substituter.Process();
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(substituter.Processed);
            string postEscape = escaper.Escaped;
            string[] parts = SplitByManyStrings(
                postEscape, 
                expected
            );
            Assert.AreEqual(
                notEscaped.Length,
                parts.Length
            );
            string reconstructedOutput = "";
            for (int i = 0; i < parts.Length; i++)
            {
                reconstructedOutput = reconstructedOutput + parts[i] + expected[i];
            }
            Assert.AreEqual(
                reconstructedOutput,
                escaper.Escaped
            );
        }
    }
}