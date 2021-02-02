
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkdownToHtml
{
    [TestClass]
    public class HtmlSpecialCharacterEscaperTests
    {
        [TestMethod]
        [Timeout(500)]
        public void NonSpecialCharactersAreNotEscaped()
        {
            string nonSpecials = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^*([]{})\\|;:/?,.=+";
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(nonSpecials);
            Assert.AreEqual(
                nonSpecials,
                escaper.Escaped
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("&sp;")]
        [DataRow("&blank;")]
        [DataRow("&excl;")]
        [DataRow("&quot;")]
        [DataRow("&num;")]
        [DataRow("&dollar;")]
        [DataRow("&percnt;")]
        [DataRow("&amp;")]
        [DataRow("&apos;")]
        [DataRow("&lpar;")]
        [DataRow("&rpar;")]
        [DataRow("&ast;")]
        [DataRow("&plus;")]
        [DataRow("&comma;")]
        [DataRow("&hyphen;")]
        [DataRow("&dash;")]
        [DataRow("&period;")]
        [DataRow("&sol;")]
        [DataRow("&colon;")]
        [DataRow("&semi;")]
        [DataRow("&equals;")]
        [DataRow("&quest;")]
        [DataRow("&commat;")]
        [DataRow("&lsqb;")]
        [DataRow("&bsol;")]
        [DataRow("&rsqb;")]
        [DataRow("&caret;")]
        [DataRow("&lowbar;")]
        [DataRow("&lcub;")]
        [DataRow("&verbar;")]
        [DataRow("&rcub;")]
        [DataRow("&tilde;")]
        [DataRow("&sim;")]
        [DataRow("&nbsp;")]
        [DataRow("&iexcl;")]
        [DataRow("&cent;")]
        [DataRow("&pound;")]
        [DataRow("&curren;")]
        [DataRow("&yen;")]
        [DataRow("&brkbar;")]
        [DataRow("&sect;")]
        [DataRow("&uml;")]
        [DataRow("&die;")]
        [DataRow("&copy;")]
        [DataRow("&ordf;")]
        [DataRow("&laquo;")]
        [DataRow("&not;")]
        [DataRow("&shy;")]
        [DataRow("&reg;")]
        [DataRow("&hibar;")]
        [DataRow("&deg;")]
        [DataRow("&plusmn;")]
        [DataRow("&sup2;")]
        [DataRow("&sup3;")]
        [DataRow("&acute;")]
        [DataRow("&micro;")]
        [DataRow("&para;")]
        [DataRow("&middot;")]
        [DataRow("&cedil;")]
        [DataRow("&sup1;")]
        [DataRow("&ordm;")]
        [DataRow("&raquo;")]
        [DataRow("&frac14;")]
        [DataRow("&half;")]
        [DataRow("&frac34;")]
        [DataRow("&iquest;")]
        public void EscapedSpecialCharactersAreLeftUnchanged(
            string escapedAlready
        ) {
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(escapedAlready);
            Assert.AreEqual(
                escapedAlready,
                escaper.Escaped
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("&", "amp")]
        [DataRow("\"", "quot")]
        [DataRow("'", "apos")]
        [DataRow("<", "lt")]
        [DataRow(">", "gt")]
        public void AutomaticallyEscapedSpecialsAreReplacedByHtmlEscapeSequence(
            string unescaped,
            string expectedEscapeName
        ) {
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(unescaped);
            Assert.AreEqual(
                "&" + expectedEscapeName + ";",
                escaper.Escaped
            );
        }

        [DataTestMethod]
        [Timeout(500)]
        [DataRow("Test&Test", "amp")]
        [DataRow("Test\"Test", "quot")]
        [DataRow("Test'Test", "apos")]
        [DataRow("Test<Test", "lt")]
        [DataRow("Test>Test", "gt")]
        public void OnlyAutomaticallyEscapedSpecialsAreReplacedByHtmlEscapeSequenceWhenInBetweenOtherText(
            string unescaped,
            string expectedEscapeName
        ) {
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(unescaped);
            string withoutSurroundingText = escaper.Escaped.Replace("Test", "");
            Assert.AreEqual(
                escaper.Escaped.Length - 8,
                withoutSurroundingText.Length
            );
            Assert.AreEqual(
                "&" + expectedEscapeName + ";",
                withoutSurroundingText
            );
        }

        [TestMethod]
        [Timeout(500)]
        public void AutomaticallyEscapedSpecialsAdjacentToOneAnotherAreBothEscaped() 
        {
            string adjacentSpecials = "<>>&'\"";
            string replaced = "&lt;&gt;&gt;&amp;&apos;&quot;";
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(adjacentSpecials);
            Assert.AreEqual(
                replaced,
                escaper.Escaped
            );
        }
    }
}