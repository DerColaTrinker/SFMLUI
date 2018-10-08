using System.Collections.Generic;

namespace Pandora.Interactions.UI.Design
{
    internal class AnimationContainer
    {
        public DesignAnimationEvents Event { get; set; }

        public IEnumerable<StyleContainer> Styles { get; set; }

        public string Groupname { get; internal set; }
    }
}