using Pandora.SFML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Bindings
{
    public abstract class BindingObject : ObjectPointer
    {
        public BindingObject(IntPtr pointer) : base(pointer)
        {
            Bindings = new BindingCollection(this);
        }

        public BindingObject() : base(IntPtr.Zero)
        {
            Bindings = new BindingCollection(this);
        }

        public BindingCollection Bindings { get; }
    }
}
