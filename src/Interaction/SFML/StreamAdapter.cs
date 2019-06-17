using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Pandora.SFML
{
    internal class StreamAdaptor : IDisposable
    {
        public StreamAdaptor(Stream stream)
        {
            myStream = stream;

            myInputStream = new InputStream();
            myInputStream.Read = new ReadCallbackType(Read);
            myInputStream.Seek = new SeekCallbackType(Seek);
            myInputStream.Tell = new TellCallbackType(Tell);
            myInputStream.GetSize = new GetSizeCallbackType(GetSize);

            myInputStreamPtr = Marshal.AllocHGlobal(Marshal.SizeOf(myInputStream));
            Marshal.StructureToPtr(myInputStream, myInputStreamPtr, false);
        }

        ~StreamAdaptor()
        {
            Dispose(false);
        }

        public IntPtr InputStreamPtr
        {
            get { return myInputStreamPtr; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Marshal.FreeHGlobal(myInputStreamPtr);
        }

        private long Read(IntPtr data, long size, IntPtr userData)
        {
            byte[] buffer = new byte[size];
            int count = myStream.Read(buffer, 0, (int)size);
            Marshal.Copy(buffer, 0, data, count);
            return count;
        }

        private long Seek(long position, IntPtr userData)
        {
            return myStream.Seek(position, SeekOrigin.Begin);
        }

        private long Tell(IntPtr userData)
        {
            return myStream.Position;
        }

        private long GetSize(IntPtr userData)
        {
            return myStream.Length;
        }

        private Stream myStream;
        private InputStream myInputStream;
        private IntPtr myInputStreamPtr;
    }
}
