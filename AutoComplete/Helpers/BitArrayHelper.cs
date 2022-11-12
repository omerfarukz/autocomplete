using System;
using System.Collections;

namespace AutoComplete.Helpers
{
    public static class BitArrayHelper
    {
        /// <summary>
        ///     Custom CopyTo implementation for BitArray
        /// </summary>
        /// <param name="bitArray"></param>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public static void CopyToInt32Array(this BitArray bitArray, int[] array, int index)
        {
            if (bitArray == null)
                throw new ArgumentNullException(nameof(bitArray));

            if (array == null)
                throw new ArgumentNullException(nameof(array));
    
            var location = 0;
            for (var i = 0; i < bitArray.Length; i++)
            {
                if (i % 32 == 0 && i != 0)
                {
                    index++;
                    location = 0;
                }

                if (bitArray.Get(i))
                    array[index] |= 1 << location;

                location++;
            }
        }
    }
}