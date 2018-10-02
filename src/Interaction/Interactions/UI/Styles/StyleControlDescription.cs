using System;
using System.Collections.Generic;

namespace Pandora.Interactions.UI.Styles
{
    internal class StyleControlDescription
    {
        public Type Type { get; internal set; }
        public IEnumerable<StylePropertyDescription> BindingProperties { get; internal set; }
    }
}