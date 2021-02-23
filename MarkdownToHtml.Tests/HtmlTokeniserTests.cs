
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
        [DataRow("j!d@f#3_94A$H%k^2?9*M(SD)8:;3jh+uj[FD]3'2jh7,3H.J\\F|D")]
        public void GroupOfContiguousNonWhitespaceCharactersExcludingKeyHtmlCharactersAreATextToken(
            string content
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.Text,
                token.Type
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow(" ")]
        [DataRow("\t")]
        public void IndividualNonLineBreakingWhitespaceCharactersAreAWhitespaceToken(
            string character
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(character);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.NonLineBreakingWhitespace,
                token.Type
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("    ")]
        [DataRow("\t\t\t\t\t\t\t\t")]
        public void MultipleIdenticalNonLineBreakingWhitespaceCharactersAreASingleWhitespaceToken(
            string content
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.NonLineBreakingWhitespace,
                token.Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void SeriesOfMixedNonLineBreakingWhitespaceCharactersAreGroupedIntoWhitespaceTokensForContiguousIdenticalCharacters() 
        {
            string content = "    \t\t\t\t  \t";
            string[] expectedTokenContents = new string[]{
                "    ",
                "\t\t\t\t",
                "  ",
                "\t"
            };
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                expectedTokenContents.Length,
                tokens.Length
            );
            for (int i = 0; i < expectedTokenContents.Length; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    checking.Type
                );
                Assert.AreEqual(
                    expectedTokenContents[i],
                    checking.Content
                );
            }
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("\n")]
        [DataRow("\r")]
        public void IndividualLineBreakingWhitespaceCharactersAreALineBreakingWhitespaceToken(
            string character
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(character);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.LineBreakingWhitespace,
                token.Type
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void CRLFWhitespaceCharacterIsASingleLineBreakingWhitespaceToken() 
        {
            string content = "\r\n";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                1,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.LineBreakingWhitespace,
                token.Type
            );
            Assert.AreEqual(
                content,
                token.Content
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("\n\n\n\n\n", 5)]
        [DataRow("\r\r\r\r\r", 5)]
        [DataRow("\r\n\r\n\r\n\r\n\r\n", 5)]
        public void MultipleLineBreakingWhitespaceCharacters(
            string content,
            int numberOfTokens
        ) {
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                numberOfTokens,
                tokens.Length
            );
            int tokenSize = content.Length / numberOfTokens;
            for (int i = 0; i < numberOfTokens; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.LineBreakingWhitespace,
                    checking.Type
                );
                Assert.AreEqual(
                    content.Substring(0, tokenSize),
                    checking.Content
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void MixedSetOfWhitespaceCharactersSeparatesSingleLineBreaksIntoindividualTokens() 
        {
            string content = "\n\n\r\n\r\r\r\n\r\n\n\n\r\r";
            string [] expectedTokenContents = new string[]
            {
                "\n",
                "\n",
                "\r\n",
                "\r",
                "\r",
                "\r\n",
                "\r\n",
                "\n",
                "\n",
                "\r",
                "\r"
            };
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                expectedTokenContents.Length,
                tokens.Length
            );
            for (int i = 0; i < expectedTokenContents.Length; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.LineBreakingWhitespace,
                    checking.Type
                );
                Assert.AreEqual(
                    expectedTokenContents[i],
                    checking.Content
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void LessThanCharacterIsRecognisedAsLessThanHtmlToken() 
        {
            string content = "<";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.LessThan,
                token.Type
            );
            Assert.AreEqual(
                content,
                token.Content
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void MultipleContiguousLessThanCharactersAreSeparatedIntoOneTokenPerCharacter() 
        {
            string content = "<<<<<<";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            for (int i = 0; i < content.Length; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.LessThan,
                    checking.Type
                );
                Assert.AreEqual(
                    "<",
                    checking.Content
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void GreaterThanCharacterIsRecognisedAsGreaterThanHtmlToken() 
        {
            string content = ">";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.GreaterThan,
                token.Type
            );
            Assert.AreEqual(
                content,
                token.Content
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void MultipleContiguousGreaterThanCharactersAreSeparatedIntoOneTokenPerCharacter() 
        {
            string content = ">>>>>>";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            for (int i = 0; i < content.Length; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.GreaterThan,
                    checking.Type
                );
                Assert.AreEqual(
                    ">",
                    checking.Content
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void ForwardSlashCharacterIsRecognisedAsForwardSlashHtmlToken() 
        {
            string content = "/";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.ForwardSlash,
                token.Type
            );
            Assert.AreEqual(
                content,
                token.Content
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void MultipleContiguousForwardSlashCharactersAreSeparatedIntoOneTokenPerCharacter() 
        {
            string content = "//////";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            for (int i = 0; i < content.Length; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.ForwardSlash,
                    checking.Type
                );
                Assert.AreEqual(
                    "/",
                    checking.Content
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void DoubleQuoteCharacterIsRecognisedAsDoubleQuoteHtmlToken() 
        {
            string content = "\"";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.DoubleQuote,
                token.Type
            );
            Assert.AreEqual(
                content,
                token.Content
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void MultipleContiguousDoubleQuoteCharactersAreSeparatedIntoOneTokenPerCharacter() 
        {
            string content = "\"\"\"\"\"\"";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            for (int i = 0; i < content.Length; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.DoubleQuote,
                    checking.Type
                );
                Assert.AreEqual(
                    "\"",
                    checking.Content
                );
            }
        }

        [TestMethod]
        [Timeout(500)]
        public void EqualsCharacterIsRecognisedAsEqualsHtmlToken() 
        {
            string content = "=";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            HtmlToken token = tokens[0];
            Assert.AreEqual(
                HtmlTokenType.Equals,
                token.Type
            );
            Assert.AreEqual(
                content,
                token.Content
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void MultipleContiguousEqualsCharactersAreSeparatedIntoOneTokenPerCharacter() 
        {
            string content = "======";
            HtmlTokeniser tokeniser = new HtmlTokeniser(content);
            HtmlToken[] tokens = tokeniser.tokenise();
            Assert.AreEqual(
                content.Length,
                tokens.Length
            );
            for (int i = 0; i < content.Length; i++)
            {
                HtmlToken checking = tokens[i];
                Assert.AreEqual(
                    HtmlTokenType.Equals,
                    checking.Type
                );
                Assert.AreEqual(
                    "=",
                    checking.Content
                );
            }
        }
    }
}