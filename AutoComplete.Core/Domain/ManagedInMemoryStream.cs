using System.IO;

namespace AutoComplete.Core.Domain
{
    public class ManagedInMemoryStream : MemoryStream
    {
        public ManagedInMemoryStream(byte[] buffer)
            : base(buffer)
        { }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
    }
}