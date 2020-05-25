
using System;
using System.IO;

namespace MdToHtml
{
    class Program
    {
        private const string HelpText =
            "Markdown To HTML Conversion Utility\n\n"
            + "Usage: MdToHtml -i input_file -o output_file\n\n"
            + "Required Flags:\n"
            + "  -i input_file  -> path to Markdown file to be converted\n"
            + "  -o output_file -> path to which converted HTML file will be saved\n"
            + "\nOptional Flags:\n"
            + "  -h             -> print help text\n";

        private const string InvalidInputsText = 
            "Markdown To HTML Conversion Utility\n\n"
            + "Invalid/missing input file and/or output file\n\n"
            + "You must provide both an input file path (i flag)\n"
            + "and an output file path (o flag)\n\n"
            + "Please run MdToHtml -h for help text/more information\n";

        private const string MissingInputFileText = 
            "Markdown To HTML Conversion Utility\n\n"
            + "Input file is empty or does not exist\n\n"
            + "Please run MdToHtml -h for help text/more information\n";

        static void Main(
            string[] args
        )
        {
            ArgumentParser arguments = new ArgumentParser(
                String.Join(
                    " ",
                    args
                )
            );
            // Print help if no arguments/invalid arguments/h flag
            if (
                !arguments.AllArgumentsValid()
                || arguments.HasValidFlag("h")
                || arguments.NoValidArguments()
            )
            {
                PrintAndExit(
                    HelpText
                );
            }
            // Need input file output file arguments
            if (
                !(
                    arguments.HasValidFlag("i")
                    && arguments.HasValidFlag("o")
                )
            ) {
                PrintAndExit(
                    InvalidInputsText
                );
            }
            // Attempt to read in file, error if cannot
            string inputFilePath = arguments.ValueForFlag(
                "i"
            );
            string input = ReadFileIfExists(
                inputFilePath
            );
            if (input == "")
            {
                PrintAndExit(
                    MissingInputFileText
                );
            }
        }

        static void PrintAndExit(
            string message
        ) {
            Console.WriteLine(
                message
            );
            System.Environment.Exit(0);
        }

        static string ReadFileIfExists(
            string filePath
        ) {
            if (
                File.Exists(
                    filePath
                )
            ) {
                string[] lines = File.ReadAllLines(
                    filePath
                );
                return String.Join(
                    "\n",
                    filePath
                );
            }
            return "";
        }
    }
}
