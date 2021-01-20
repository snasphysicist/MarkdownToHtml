
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlTokeniserEndToEndTest
    {
        [TestMethod]
        [Timeout(500)]
        public void ParseCompleteStringOfHtmlTokens()
        {
            HtmlToken opener = new HtmlToken(
                HtmlTokenType.LessThan,
                "<"
            );
            HtmlToken closer = new HtmlToken(
                HtmlTokenType.GreaterThan,
                ">"
            );
            HtmlToken forwardSlash = new HtmlToken(
                HtmlTokenType.ForwardSlash,
                "/"
            );
            HtmlToken equalsSign = new HtmlToken(
                HtmlTokenType.Equals,
                "="
            );
            HtmlToken doubleQuote = new HtmlToken(
                HtmlTokenType.DoubleQuote,
                "\""
            );
            HtmlToken unixLineBreak = new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                "\n"
            );
            HtmlToken weirdLineBreak = new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                "\r"
            );
            HtmlToken windowsLineBreak = new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                "\r\n"
            );
            HtmlToken singleSpace = new HtmlToken(
                HtmlTokenType.NonLineBreakingWhitespace,
                " "
            );
            HtmlToken doubleSpace = new HtmlToken(
                HtmlTokenType.NonLineBreakingWhitespace,
                "  "
            );
            HtmlToken[] expected = new HtmlToken[]
            {
                Text("Hello."),
                doubleSpace,
                Text("test."),
                unixLineBreak,
                unixLineBreak,
                opener,
                Text("p"),
                closer,
                Text("A"),
                singleSpace,
                Text("Test"),
                opener,
                forwardSlash,
                Text("p"),
                closer,
                windowsLineBreak,
                windowsLineBreak,
                opener,
                Text("test2"),
                singleSpace,
                forwardSlash,
                closer,
                weirdLineBreak,
                weirdLineBreak,
                Text("Another"),
                singleSpace,
                Text("test"),
                singleSpace,
                closer, 
                singleSpace,
                Text("of"),
                singleSpace,
                forwardSlash,
                singleSpace,
                Text("specials"),
                singleSpace, 
                equalsSign,
                Text("."),
                singleSpace,
                Text("Not"),
                singleSpace,
                Text("so"),
                singleSpace,
                Text("special"),
                singleSpace,
                Text("!@#$%^&*()."),
                opener,
                Text("open"),
                singleSpace,
                Text("a"),
                equalsSign,
                doubleQuote,
                Text("b"),
                doubleQuote,
                singleSpace,
                Text("c"),
                equalsSign,
                doubleQuote,
                Text("47r3w7Hu8t943"),
                doubleQuote,
                closer,
                opener,
                forwardSlash,
                Text("open"),
                closer,
                unixLineBreak,
                unixLineBreak,
                Text("Finally,"),
                singleSpace,
                Text("a"),
                singleSpace,
                Text("self"),
                singleSpace,
                Text("closer"),
                singleSpace,
                opener,
                Text("sc"),
                singleSpace,
                Text("a"),
                equalsSign,
                doubleQuote,
                Text("b"),
                doubleQuote,
                doubleSpace,
                Text("fdjnhis"),
                equalsSign,
                doubleQuote,
                Text("7yu834thiundfv87"),
                doubleSpace,
                forwardSlash,
                closer,
                singleSpace,
                Text("with"),
                singleSpace,
                Text("attributes")
            };
            // Expect htmlString to be
            // Hello.  test.\n\n<p>A Test</p>\r\n\r\n<test2 />\r\rAnother test > of / specials =. Not so special !@#$%^&*().
            // <open a=\"b\" c=\"47r3w7Hu8t943\"></open>\n\nFinally, a self closer <sc a=\"b\" fdjnhis=\"7yu834thiundfv87\"   /> with attributes
            string htmlString = "";
            foreach (HtmlToken token in expected)
            {
                htmlString = htmlString + token.Content;
            }
            HtmlTokeniser tokeniser = new HtmlTokeniser(htmlString);
            LinkedList<HtmlToken> tokenised = tokeniser.tokenise();
            HtmlToken[] actual = new HtmlToken[tokenised.Count];
            Assert.AreEqual(
                expected.Length,
                actual.Length
            );
            LinkedListNode<HtmlToken> current = tokenised.First;
            for (int i = 0; i < actual.Length; i++)
            {
                actual[i] = current.Value;
                current = current.Next;
            }
            for (int i = 0; i < expected.Length; i++)
            {
                HtmlToken expectedToken = expected[i];
                HtmlToken actualToken = actual[i];
                Assert.AreEqual(
                    expectedToken.Type,
                    actualToken.Type
                );
                Assert.AreEqual(
                    expectedToken.Content,
                    actualToken.Content
                );
            }
        }

        private HtmlToken NonLineBreakingWhitespace(
            string content
        ) {
            return new HtmlToken(
                HtmlTokenType.NonLineBreakingWhitespace,
                content
            );
        }

        private HtmlToken LineBreakingWhitespace(
            string content
        ) {
            return new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                content
            );
        }

        private HtmlToken Text(
            string content
        ) {
            return new HtmlToken(
                HtmlTokenType.Text,
                content
            );
        }
    }
}