
using System;
using System.Collections.Generic;

namespace MarkdownToHtml
{
    public class Utils
    {
        public static T[] LinkedListToArray<T>(
            LinkedList<T> linkedList
        ) {
            T[] array = new T[linkedList.Count];
            linkedList.CopyTo(array, 0);
            return array;
        }

        /*
         * Used to find the line on which a
         * contiguous markdown element section
         * ends, based on the string used at the
         * start of the line to indicate the section
         */
        public static int FindEndOfSection(
            ParseInput input,
            string sectionIndicator
        )
        {
            int index = 1;
            bool previousLineWasWhitespace = false;
            /* 
             * Condition
             * Don't allow index to exceed the number of elements to avoid Exceptions
             * We want to break the loop when there is a whitespace line
             * (previousLineWasWhitespace)
             * followed by a non-whitespace line (!ContainsOnlyWhitespace(lines[index])) 
             * which is not a line of given section type !lines[index].StartsWith
             */
            while (
                index < input.Count
                && !(
                    previousLineWasWhitespace
                    && !input[index].ContainsOnlyWhitespace()
                    && !input[index].StartsWith(
                        sectionIndicator
                    )
                )
            ) {
                if (input[index].ContainsOnlyWhitespace())
                {
                    previousLineWasWhitespace = true;
                } else {
                    previousLineWasWhitespace = false;
                }
                index++;
            }
            return index;
        }

        public static bool ContainsOnlyWhitespace(
            string line
        ) {
            return line.Replace(
                " ",
                ""
            ).Length == 0;
        }

        public static string StripLeadingCharacter(
            string line,
            char character
        ) {
            return StripLeadingCharacterUpTo(
                line,
                character,
                line.Length
            );
        }

        public static string StripLeadingCharacterUpTo(
            string line,
            char character,
            int maximumNumberToRemove
        ) {
            int removed = 0;
            while (
                (line.Length > 0)
                && (line[0] == character)
                && (removed < maximumNumberToRemove)
            ) {
                line = line.Substring(1);
                removed++;
            }
            return line;
        }

        public static string StripTrailingCharacter(
            string line,
            char character
        ) {
            while (
                (line.Length > 0)
                && (line[^1] == character)
            ) {
                line = line.Substring(
                    0,
                    line.Length - 1 
                );
            }
            return line;
        }
    }
}