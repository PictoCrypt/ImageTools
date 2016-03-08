using System.Collections.Generic;
using System.Linq;

namespace FunctionLib.Helper
{
    public static class ListHelper
    {
        /// <summary>
        /// Gets the index based on the first element found in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Collection in which to search.</param>
        /// <param name="sequence">Sequence which should be searched.</param>
        /// <returns>0-Based-Index from the first element.</returns>
        public static int IndexOf<T>(IEnumerable<T> collection,
            IEnumerable<T> sequence)
        {
            //TODO: Vielleicht mit currentIndex
            var ccount = collection.Count();
            var scount = sequence.Count();

            if (scount > ccount) return -1;

            if (collection.Take(scount).SequenceEqual(sequence)) return 0;

            var index = Enumerable.Range(1, ccount - scount + 1)
                .FirstOrDefault(i => collection.Skip(i).Take(scount).SequenceEqual(sequence));
            if (index == 0) return -1;
            return index;
        }
    }
}