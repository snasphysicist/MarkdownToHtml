
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlTagDetectorEndToEndTests
    {
        [TestMethod]
        [Timeout(500)]
        public void EndToEndManyHtmlTagDetectionTest()
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
            HtmlToken unixLinebreak = new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                "\n"
            );
            HtmlToken windowsLinebreak = new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                "\r\n"
            );
            HtmlToken weirdLinebreak = new HtmlToken(
                HtmlTokenType.LineBreakingWhitespace,
                "\r"
            );
            HtmlToken singleSpace = new HtmlToken(
                HtmlTokenType.NonLineBreakingWhitespace,
                " "
            );

            HtmlSnippet firstText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "Hello."
                )
            );
            HtmlSnippet twoSpaces = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.NonLineBreakingWhitespace,
                    "  "
                )
            );
            HtmlSnippet secondText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "Test."
                )
            );
            HtmlSnippet openingParagraph = new HtmlSnippet(
                new HtmlTag(
                    new HtmlToken[]
                    {
                        opener,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "p"
                        ),
                        closer
                    },
                    HtmlDisplayType.Block,
                    HtmlTagType.Opening
                )
            );
            HtmlSnippet firstTextInParagraph = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "A"
                )
            );
            HtmlSnippet secondTextInParagraph = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "Test."
                )
            );
            HtmlSnippet closingParagraph = new HtmlSnippet(
                new HtmlTag(
                    new HtmlToken[]
                    {
                        opener,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "p"
                        ),
                        forwardSlash,
                        closer
                    },
                    HtmlDisplayType.Block,
                    HtmlTagType.Closing
                )
            );
            HtmlSnippet selfClosingWithOutAttributes = new HtmlSnippet(
                new HtmlTag(
                    new HtmlToken[]
                    {
                        opener,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "test2"
                        ),
                        twoSpaces.Token,
                        forwardSlash,
                        closer
                    },
                    HtmlDisplayType.Block,
                    HtmlTagType.SelfClosing
                )
            );
            HtmlSnippet anotherText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "Another"
                )
            );
            HtmlSnippet testText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "test."
                )
            );
            HtmlSnippet ofText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "test"
                )
            );
            HtmlSnippet specialsText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "specials"
                )
            );
            HtmlSnippet equals = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Equals,
                    "="
                )
            );
            HtmlSnippet fullStop = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "."
                )
            );
            HtmlSnippet notText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "Not"
                )
            );
            HtmlSnippet soText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "So"
                )
            );
            HtmlSnippet specialText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "special"
                )
            );
            HtmlSnippet actualSpecialChain = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "!@#$%^&*()."
                )
            );
            HtmlSnippet openingWithAttributes = new HtmlSnippet(
                new HtmlTag(
                    new HtmlToken[]
                    {
                        opener,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "open"
                        ),
                        singleSpace,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "a"
                        ),
                        equalsSign,
                        doubleQuote,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "b"
                        ),
                        doubleQuote,
                        singleSpace,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "c"
                        ),
                        equalsSign,
                        doubleQuote,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "47r3w7Hu8t943"
                        ),
                        doubleQuote,
                        closer
                    },
                    HtmlDisplayType.Inline,
                    HtmlTagType.Opening
                )
            );
            HtmlSnippet closingForOpenWithAttributes = new HtmlSnippet(
                new HtmlTag(
                    new HtmlToken[]
                    {
                        opener,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "open"
                        ),
                        forwardSlash,
                        closer
                    },
                    HtmlDisplayType.Block,
                    HtmlTagType.Closing
                )
            );
            HtmlSnippet finallyText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "Finally,"
                )
            );
            HtmlSnippet aText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "a"
                )
            );
            HtmlSnippet selfText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "self"
                )
            );
            HtmlSnippet closerText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "closer"
                )
            );
            HtmlSnippet selfClosingWithAttributes = new HtmlSnippet(
                new HtmlTag(
                    new HtmlToken[]
                    {
                        opener,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "sc"
                        ),
                        singleSpace,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "a"
                        ),
                        equalsSign,
                        doubleQuote,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "b"
                        ),
                        doubleQuote,
                        singleSpace,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "fdjnhis"
                        ),
                        equalsSign,
                        doubleQuote,
                        new HtmlToken(
                            HtmlTokenType.Text,
                            "7yu834thiundfv87"
                        ),
                        doubleQuote,
                        twoSpaces.Token,
                        forwardSlash,
                        closer
                    },
                    HtmlDisplayType.Inline,
                    HtmlTagType.SelfClosing
                )
            );
            HtmlSnippet withText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "with"
                )
            );
            HtmlSnippet attributesText = new HtmlSnippet(
                new HtmlToken(
                    HtmlTokenType.Text,
                    "attributes"
                )
            );
            HtmlSnippet[] expected = new HtmlSnippet[]
            {
                firstText,
                twoSpaces,
                testText,
                new HtmlSnippet(unixLinebreak),
                new HtmlSnippet(unixLinebreak),
                openingParagraph,
                firstTextInParagraph,
                new HtmlSnippet(singleSpace),
                secondTextInParagraph,
                closingParagraph,
                new HtmlSnippet(windowsLinebreak),
                new HtmlSnippet(windowsLinebreak),
                selfClosingWithOutAttributes,
                new HtmlSnippet(weirdLinebreak),
                new HtmlSnippet(weirdLinebreak),
                anotherText,
                new HtmlSnippet(singleSpace),
                testText,
                new HtmlSnippet(singleSpace),
                new HtmlSnippet(opener),
                new HtmlSnippet(singleSpace),
                ofText,
                new HtmlSnippet(singleSpace),
                new HtmlSnippet(forwardSlash),
                new HtmlSnippet(singleSpace),
                specialsText,
                new HtmlSnippet(singleSpace),
                new HtmlSnippet(equalsSign),
                fullStop,
                new HtmlSnippet(singleSpace),
                notText,
                new HtmlSnippet(singleSpace),
                soText,
                new HtmlSnippet(singleSpace),
                specialText,
                new HtmlSnippet(singleSpace),
                actualSpecialChain,
                openingWithAttributes,
                closingForOpenWithAttributes,
                new HtmlSnippet(unixLinebreak),
                new HtmlSnippet(unixLinebreak),
                finallyText,
                new HtmlSnippet(singleSpace),
                aText,
                new HtmlSnippet(singleSpace),
                selfText,
                new HtmlSnippet(singleSpace),
                closerText,
                new HtmlSnippet(singleSpace),
                selfClosingWithAttributes,
                new HtmlSnippet(singleSpace),
                withText,
                new HtmlSnippet(singleSpace),
                attributesText
            };

            string htmlString = "";
            foreach (HtmlSnippet snippet in expected)
            {
                if (snippet.IsTag())
                {
                    foreach (HtmlToken token in snippet.Tag.GetTokens())
                    {
                        htmlString = htmlString + token.Content;
                    }
                }
                if (snippet.IsToken())
                {
                    htmlString = htmlString + snippet.Token.Content;
                }
            }
            // Expect htmlString to be
            // Hello.  test.\n\n<p>A Test</p>\r\n\r\n<test2 />\r\rAnother test > of / specials =. Not so special !@#$%^&*().
            // <open a=\"b\" c=\"47r3w7Hu8t943\"></open>\n\nFinally, a self closer <sc a=\"b\" fdjnhis=\"7yu834thiundfv87\"   /> with attributes
            HtmlToken[] tokens = new HtmlTokeniser(htmlString).tokenise();
            HtmlSnippet[] actual = HtmlTagDetector.TagsFromTokens(tokens);
            // Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                HtmlSnippet expectedSnippet = expected[i];
                HtmlSnippet actualSnippet = actual[i];
                Assert.AreEqual(expectedSnippet.IsTag(), actualSnippet.IsTag());
                Assert.AreEqual(expectedSnippet.IsToken(), actualSnippet.IsToken());
                if (expectedSnippet.IsToken())
                {
                    HtmlToken expectedToken = expectedSnippet.Token;
                    HtmlToken actualToken = actualSnippet.Token;
                    Assert.AreEqual(
                        expectedToken.Type,
                        actualToken.Type
                    );
                    Assert.AreEqual(
                        expectedToken.Content,
                        actualToken.Content
                    );
                }
                if (expectedSnippet.IsTag())
                {
                    HtmlTag expectedTag = expectedSnippet.Tag;
                    HtmlTag actualTag = actualSnippet.Tag;
                    HtmlToken[] expectedTokens = expectedTag.GetTokens();
                    HtmlToken[] actualTokens = actualTag.GetTokens();
                    Assert.AreEqual(
                        expectedTokens.Length,
                        actualTokens.Length
                    );
                    for (int j = 0; j < expectedTokens.Length; j++)
                    {
                        HtmlToken expectedToken = expectedTokens[j];
                        HtmlToken actualToken = actualTokens[j];
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
            }
        }
    }
}