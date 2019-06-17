using Pandora.SFML.Graphics;
using Pandora.SFML.Native;
using System;
using System.Runtime.ConstrainedExecution;

namespace Pandora.Interactions.UI.Renderer
{
    public class Context : CriticalFinalizerObject
    {
        private static Context ourGlobalContext = null;

        private readonly IntPtr _pointer = IntPtr.Zero;

        public Context()
        {
            _pointer = NativeSFML.sfContext_create();
        }

        ~Context()
        {
            NativeSFML.sfContext_destroy(_pointer);
        }

        public bool SetActive(bool active)
        {
            return NativeSFML.sfContext_setActive(_pointer, active);
        }

        public ContextSettings Settings
        {
            get { return NativeSFML.sfContext_getSettings(_pointer); }
        }

        public static Context Global
        {
            get
            {
                if (ourGlobalContext == null)
                    ourGlobalContext = new Context();

                return ourGlobalContext;
            }
        }

        public override string ToString()
        {
            return "[Context]";
        }
    }
}
