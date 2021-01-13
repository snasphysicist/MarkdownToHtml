
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MarkdownToHtml 
{
    [TestClass]
    public class HtmlTokeniserTests
    {
        [DataTestMethod]
        [Timeout(500)]
        [DataRow("jdf394AHk29MSD")]
        [DataRow("j!d@f#3_94A$H%k^2?9*M(SD)8:;3jh+u=j[FD]3'2jh7,3H.J\\F|D")]
        public void GroupOfContiguousNonWhitespaceCharactersExcludingKeyHtmlCharactersAreATextToken(
            string content
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Count
            );
            HtmlToken token = tokens.First.Value;
            Assert.AreEqual(
                HtmlTokenType.Text,
                token.Type
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(" ")]
        [DataRow("\n")]
        [DataRow("\r")]
        [DataRow("\t")]
        public void IndividualWhitespaceCharactersAreAWhitespaceToken(
            string character
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(character);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Count
            );
            HtmlToken token = tokens.First.Value;
            Assert.AreEqual(
                HtmlTokenType.Whitespace,
                token.Type
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("    ")]
        [DataRow("\n\n\n\n")]
        [DataRow("\r\r\r\r\r\r")]
        [DataRow("\t\t\t\t\t\t\t\t")]
        public void MultipleIdenticalWhitespaceCharactersAreASingleWhitespaceToken(
            string content
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Count
            );
            HtmlToken token = tokens.First.Value;
            Assert.AreEqual(
                HtmlTokenType.Whitespace,
                token.Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SeriesOfMixedWhitespaceCharactersAreGroupedIntoWhitespaceTokensForContiguousIdenticalCharacters() 
        {
            string content = "    \n\n\n\n\n\t\t\t\r\t";
            string[] expectedTokenContents = new string[]{
                "    ",
                "\n\n\n\n\n",
                "\t\t\t",
                "\r",
                "\t"
            };
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                expectedTokenContents.Length,
                tokens.Count
            );
            LinkedListNode<HtmlToken> checking = tokens.First;
            for (int i = 0; i < expectedTokenContents.Length; i++)
            {
                Assert.AreEqual(
                    HtmlTokenType.Whitespace,
                    checking.Value.Type
                );
                Assert.AreEqual(
                    expectedTokenContents[i],
                    checking.Value.Content
                );
                checking = checking.Next;
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void LessThanCharacterIsRecognisedAsLessThanHtmlToken() 
        {
            string content = "<";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Count
            );
            HtmlToken checking = tokens.First.Value;
            Assert.AreEqual(
                HtmlTokenType.LessThan,
                checking.Type
            );
            Assert.AreEqual(
                content,
                checking.Content
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void MultipleContiguousLessThanCharactersAreSeparatedIntoOneTokenPerCharacter() 
        {
            string content = "<<<<<<";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                6,
                tokens.Count
            );
            LinkedListNode<HtmlToken> checking = tokens.First;
            for (int i = 0; i < content.Length; i++)
            {
                Assert.AreEqual(
                    HtmlTokenType.LessThan,
                    checking.Value.Type
                );
                Assert.AreEqual(
                    "<",
                    checking.Value.Content
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void GreaterThanCharacterIsRecognisedAsGreaterThanHtmlToken() 
        {
            string content = ">";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            LinkedList<HtmlToken> tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Count
            );
            HtmlToken checking = tokens.First.Value;
            Assert.AreEqual(
                HtmlTokenType.GreaterThan,
                checking.Type
            );
            Assert.AreEqual(
                content,
                checking.Content
            );
        }
    }
}
