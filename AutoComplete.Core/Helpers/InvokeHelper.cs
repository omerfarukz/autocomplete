using System;

namespace AutoComplete.Core.Helpers
{
    public static class InvokeHelper
    {
        public static void InvokeWithLock(bool @lock, object lockObject, Action action)
        {
            if (@lock)
            {
                lock (lockObject)
                {
                    action();
                }

                return;
            }

            action();
        }
    }
}
