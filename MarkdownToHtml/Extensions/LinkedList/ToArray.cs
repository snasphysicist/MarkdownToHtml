
using System.Collections.Generic;

namespace MarkdownToHtml
{
    static class ToArray
    {
        public static T[] LinkedListToArray<T>(
            this LinkedList<T> linkedList
        ) {
            T[] array = new T[linkedList.Count];
            linkedList.CopyTo(
                array,
                0
            );
            return array;
        }
    }
}