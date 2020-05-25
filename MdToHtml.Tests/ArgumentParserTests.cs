
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        [DataRow("-f -g", "fg")]
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
            for (int i = 0; i < flags.Length; i++)
            {
                Console.WriteLine(
                    flags.Substring(
                            i,
                            1
                        )
                );
                Assert.IsTrue(
                    parsed.HasValidFlag(
                        flags.Substring(
                            i,
                            1
                        )
                    )
                );
            }
        }
    }
}
