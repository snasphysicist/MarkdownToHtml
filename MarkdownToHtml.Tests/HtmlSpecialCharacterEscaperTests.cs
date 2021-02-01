
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

        [TestMethod]
        [Timeout(500)]
        public void EscapedSpecialCharactersAreLeftUnchanged() 
        {
            string manyEscaped = "&sp;&blank;&excl;&quot;&num;&dollar;&percnt;&amp;&apos;&lpar;"
                + "&rpar;&ast;&plus;&comma;&hyphen;&dash;&period;&sol;&colon;&semi;&equals;&quest;"
                + "&commat;&lsqb;&bsol;&rsqb;&caret;&lowbar;&lcub;&verbar;&rcub;&tilde;&sim;&nbsp;" 
                + "&iexcl;&cent;&pound;&curren;&yen;&brkbar;&sect;&uml;&die;&copy;&ordf;&laquo;&not;"
                +  "&shy;&reg;&hibar;&deg;&plusmn;&sup2;&sup3;&acute;&micro;&para;&middot;&cedil;&sup1;"
                + "&ordm;&raquo;&frac14;&half;&frac34;&iquest;&lt;&gt;";
            HtmlSpecialCharacterEscaper escaper = new HtmlSpecialCharacterEscaper(manyEscaped);
            Assert.AreEqual(
                manyEscaped,
                escaper.Escaped
            );
        }
    }
}