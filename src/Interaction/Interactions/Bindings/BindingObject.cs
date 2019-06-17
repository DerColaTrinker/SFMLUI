using Pandora.SFML;
using System;

namespace Pandora.Interactions.Bindings
{
    public abstract class BindingObject : ObjectPointer
    {
        public BindingObject(IntPtr pointer) : base(pointer)
        {
            Bindings = new BindingCollection();
        }

        public BindingObject() : base(IntPtr.Zero)
        {
            Bindings = new BindingCollection();
        }

        public BindingCollection Bindings { get; }
    }
}
