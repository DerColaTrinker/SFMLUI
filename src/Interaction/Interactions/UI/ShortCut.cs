using Pandora.Interactions.Controller;

namespace Pandora.Interactions.UI.Controls
{
    public struct ShortCut
    {
        public ShortCut(KeyboardKey key)
        {
            Key = key;
            Control = false;
            Shift = false;
            Alt = false;
            System = false;
        }

        public ShortCut(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            Key = key;
            Control = control;
            Shift = shift;
            Alt = alt;
            System = system;
        }

        public bool Control { get; set; }
        public bool Shift { get; set; }
        public bool Alt { get; set; }
        public bool System { get; set; }

        public KeyboardKey Key { get; set; }

        public bool Stroke(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            return (control == Control && alt == Alt && shift == Shift && system == System && key == Key);
        }
    }
}