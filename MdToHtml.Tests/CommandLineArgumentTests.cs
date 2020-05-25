
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MdToHtml
{
    [TestClass]
    public class CommandLineArgumentTests
    {
        [DataTestMethod]
        [DataRow("")]
        public void ShouldNotParseEmptyStringAsArgumentFail(
            string argument
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsFalse(
                parsed.Valid
            );
        }

        [DataTestMethod]
        [DataRow("-f", "f")]
        public void ShouldParseDashCharacterAsFlagNoValueSuccess(
            string argument,
            string flag
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsTrue(
                parsed.Valid
            );
            Assert.AreEqual(
                flag,
                parsed.Flag
            );
        }

        [DataTestMethod]
        [DataRow("/f", "f")]
        public void ShouldParseSlashCharacterAsFlagNoValueSuccess(
            string argument,
            string flag
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsTrue(
                parsed.Valid
            );
            Assert.AreEqual(
                flag,
                parsed.Flag
            );
        }

        [DataTestMethod]
        [DataRow("-F", "f")]
        public void ShouldParseDashUpperCharacterAsLowerFlagNoValueSuccess(
            string argument,
            string flag
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsTrue(
                parsed.Valid
            );
            Assert.AreEqual(
                flag,
                parsed.Flag
            );
        }

        [DataTestMethod]
        [DataRow(" -f", "f")]
        public void ShouldParseDashCharacterWithLeadingWhitespaceAsFlagNoValueSuccess(
            string argument,
            string flag
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsTrue(
                parsed.Valid
            );
            Assert.AreEqual(
                flag,
                parsed.Flag
            );
        }

        [DataTestMethod]
        [DataRow("-FR")]
        public void ShouldNotParseDashTwoCharactersAsArgumentFail(
            string argument
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsFalse(
                parsed.Valid
            );
        }

        [DataTestMethod]
        [DataRow("-f test", "f", "test")]
        [DataRow("-f           test", "f", "test")]
        public void ShouldParseDashCharacterValueAsArgumentSuccess(
            string argument,
            string flag,
            string value
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsTrue(
                parsed.Valid
            );
            Assert.AreEqual(
                flag,
                parsed.Flag
            );
            Assert.AreEqual(
                value,
                parsed.Value
            );
        }

        [DataTestMethod]
        [DataRow("-f test-test", "f", "test-test")]
        public void ShouldPreserveDashInValueSuccess(
            string argument,
            string flag,
            string value
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsTrue(
                parsed.Valid
            );
            Assert.AreEqual(
                flag,
                parsed.Flag
            );
            Assert.AreEqual(
                value,
                parsed.Value
            );
        }

        [DataTestMethod]
        [DataRow("/f test/test", "f", "test/test")]
        public void ShouldPreserveSlashInValueSuccess(
            string argument,
            string flag,
            string value
        )
        {
            CommandLineArgument parsed = new CommandLineArgument(
                argument
            );
            Assert.IsTrue(
                parsed.Valid
            );
            Assert.AreEqual(
                flag,
                parsed.Flag
            );
            Assert.AreEqual(
                value,
                parsed.Value
            );
        }
    }
}
