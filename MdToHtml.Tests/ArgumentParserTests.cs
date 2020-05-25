
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

        [DataTestMethod]
        [DataRow("-f test", "f", "test")]
        public void ShouldParseOneArgumentWithValuesCorrectFlagValueSuccess(
            string arguments,
            string flags,
            string values
        )
        {
            ArgumentParser parsed = new ArgumentParser(
                arguments
            );
            Assert.IsTrue(
                parsed.AllArgumentsValid()
            );
            string[] separateFlags = flags.Split(" ");
            string[] separateValues = values.Split(" ");
            for (int i = 0; i < separateFlags.Length; i++)
            {
                Assert.IsTrue(
                    parsed.HasValidFlag(
                        separateFlags[i]
                    )
                );
                Assert.AreEqual(
                    separateValues[i],
                    parsed.ValueForFlag(
                        separateFlags[i]
                    )
                );
            }
        }
    }
}
