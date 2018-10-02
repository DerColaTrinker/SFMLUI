using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML.System
{
    public class Clock : ObjectPointer
    {
        public Clock() : base(NativeSFML.sfClock_create())
        {
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfClock_destroy(Pointer);
        }

        public Time ElapsedTime { get { return NativeSFML.sfClock_getElapsedTime(Pointer); } }

        public Time Restart()
        {
            return NativeSFML.sfClock_restart(Pointer);
        }
    }

}
