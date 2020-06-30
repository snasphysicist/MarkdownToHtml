
using System.Collections.Generic;

namespace MarkdownToHtml
{
    static class LinkedListToArray
    {
        public static T[] ToArray<T>(
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