using System;
using System.Collections;

namespace AutoComplete.Core.Helpers
{
    public static class BitArrayHelper
    {
        /// <summary>
        /// Custom CopyTo implementation for BitArray
        /// </summary>
        /// <param name="bitArray"></param>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public static void CopyToInt32Array(this BitArray bitArray, int[] array, int index)
        {
            if (bitArray == null)
                throw new ArgumentNullException("bitArray");

            if (array == null)
                throw new ArgumentNullException("array");

            int location = 0;
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (i % 32 == 0 && i != 0)
                {
                    index++;
                    location = 0;
                }

                if (bitArray.Get(i))
                    array[index] |= (int)(1 << location);

                location++;
            }
        }
    }
}