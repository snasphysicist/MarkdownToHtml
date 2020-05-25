
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MdToHtml
{
    [TestClass]
    public class ArgumentParserTests
    {
        [DataTestMethod]
        [DataRow("-f", "f")]
        public void ShouldParseSingleArgumentNoValueCorrectFlagSuccess(
            string arguments,
            string flag
        )
        {
            ArgumentParser parsed = new ArgumentParser(
                arguments
            );
            Assert.IsTrue(
                parsed.AllArgumentsValid()
            );
            Assert.IsTrue(
                parsed.HasValidFlag(
                    flag
                )
            );
        }

        [DataTestMethod]
        [DataRow("-f -g", "f g")]
        public void ShouldParseTwoArgumentsWithoutValuesCorrectFlagsSuccess(
            string arguments,
            string flags
        )
        {
            ArgumentParser parsed = new ArgumentParser(
                arguments
            );
            Assert.IsTrue(
                parsed.AllArgumentsValid()
            );
            string[] separateFlags = flags.Split(" ");
            foreach(string flag in separateFlags)
            {
                Assert.IsTrue(
                    parsed.HasValidFlag(
                        flag
                    )
                );
            }
        }
    }
}
