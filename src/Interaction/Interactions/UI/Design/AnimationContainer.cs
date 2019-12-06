using System.Collections.Generic;

namespace Pandora.Interactions.UI.Design
{
    internal class AnimationContainer
    {
        public TriggerEvents Event { get; set; }

        public IEnumerable<PropertySetterContainer> PropertySetters { get; set; }

        public string Groupname { get; internal set; }

        public AnimationType AnimationType { get; internal set; }
    }
}