using System.Collections.Generic;

namespace Pandora.Interactions.UI.Styles
{
    public class Style
    {
        public Style()
        {
        }

        public string Name { get; set; }

        public IEnumerable<StylePropertyValueSetter> Setter { get; set; }
    }
}