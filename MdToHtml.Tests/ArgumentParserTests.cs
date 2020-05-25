
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

        [DataTestMethod]
        [DataRow("-f test1 -g test2", "f g", "test1 test2")]
        public void ShouldParseTwoArgumentsWithValuesCorrectFlagsValuesSuccess(
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

        [DataTestMethod]
        [DataRow("-f -g -h", "f g h")]
        public void ShouldParseInnerArgumentWithoutValueCorrectFlagSuccess(
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
            for (int i = 0; i < separateFlags.Length; i++)
            {
                Assert.IsTrue(
                    parsed.HasValidFlag(
                        separateFlags[i]
                    )
                );
            }
        }

        [DataTestMethod]
        [DataRow("-f -g test1 -h", "f g h", "test1")]
        public void ShouldParseInnerFlagWithValuesCorrectFlagValueSuccess(
            string arguments,
            string flags,
            string value
        )
        {
            ArgumentParser parsed = new ArgumentParser(
                arguments
            );
            Assert.IsTrue(
                parsed.AllArgumentsValid()
            );
            string[] separateFlags = flags.Split(" ");
            for (int i = 0; i < separateFlags.Length; i++)
            {
                Assert.IsTrue(
                    parsed.HasValidFlag(
                        separateFlags[i]
                    )
                );
                string parsedValue = parsed.ValueForFlag(
                    separateFlags[i]
                );
                if (parsedValue != "")
                {
                    Assert.AreEqual(
                        value,
                        parsedValue
                    );
                }
            }
        }
    }
}
