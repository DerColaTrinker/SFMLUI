using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML
{
    public abstract class ObjectPointer : IDisposable
    {
        public ObjectPointer(IntPtr pointer)
        {
            Pointer = pointer;
        }

        ~ObjectPointer()
        {
            Dispose(false);
        }

        public IntPtr Pointer { get; protected set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Pointer != IntPtr.Zero)
            {
                Destroy(disposing);
                Pointer = IntPtr.Zero;
            }
        }

        protected abstract void Destroy(bool disposing);
    }
}
