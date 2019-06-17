using Pandora.Interactions.Controller;
using System;

namespace Pandora.Interactions.Dispatcher
{
    public class KeyEventArgs : EventArgs
    {
        internal KeyEventArgs(KeyEvent e)
        {
            Code = e.Code;
            Alt = e.Alt != 0;
            Control = e.Control != 0;
            Shift = e.Shift != 0;
            System = e.System != 0;
        }

        public override string ToString()
        {
            return "[KeyEventArgs]" +
                   " Code(" + Code + ")" +
                   " Alt(" + Alt + ")" +
                   " Control(" + Control + ")" +
                   " Shift(" + Shift + ")" +
                   " System(" + System + ")";
        }

        public KeyboardKey Code { get; private set; }

        public bool Alt { get; private set; }

        public bool Control { get; private set; }

        public bool Shift { get; private set; }

        public bool System { get; private set; }
    }
}
