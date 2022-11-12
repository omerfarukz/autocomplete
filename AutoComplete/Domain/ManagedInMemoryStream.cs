using System.IO;

namespace AutoComplete.Domain
{
    public class ManagedInMemoryStream : MemoryStream
    {
        public ManagedInMemoryStream(byte[] buffer)
            : base(buffer)
        { }

        public override bool CanWrite => false;
    }
}