
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MdToHtml
{
    public class ArgumentParser
    {
        private static Regex regexArgumentFlag = new Regex(
            @"\s[-|/][A-Z|a-z][\s+|^]"
        );

        private List<CommandLineArgument> providedArguments;

        public ArgumentParser(
            string arguments
        ) {
            // Find all flags in the string, and get index where each occurs
            MatchCollection flagsMatch = regexArgumentFlag.Matches(
                arguments
            );
            int[] matchLocations = new int[flagsMatch.Count];
            for (int i = 0; i < flagsMatch.Count; i++)
            {
                matchLocations[i] = flagsMatch.Item[i].Index;
            }
            // Parse each section associated with a flag
            providedArguments = new LinkedList<CommandLineArgument>();
            for (int i = 1; i < flagsMatch.Count; i++)
            {
                providedArguments.Add(
                    new CommandLineArgument(
                        argument.Substring(
                            matchLocations[i - 1],
                            matchLocations[i]
                        )
                    )
                );
            }
        }

        public bool HasValidFlag(
            string flag
        ) {
            foreach (CommandLineArgument argument in providedArguments)
            {
                if(
                    (flag == argument.Flag)
                    && (argument.Valid)
                )
                {
                    return true;
                }
            }
            return false;
        }

        public string ValueForFlag(
            string value
        ) {
            foreach (CommandLineArgument argument in providedArguments)
            {
                if (flag == argument.Flag)
                {
                    return argument.Value;
                }
            }
            return null;
        }

        public bool AllArgumentsValid()
        {
            foreach (CommandLineArgument argument in providedArguments)
            {
                if (!argument.Valid)
                {
                    return false;
                }
            }
            return true;
        }
    }
}