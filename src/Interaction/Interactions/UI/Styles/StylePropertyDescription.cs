using System;
using System.Reflection;
using Pandora.Interactions.Bindings;

namespace Pandora.Interactions.UI.Styles
{
    internal class StylePropertyDescription
    {
        public StylePropertyDescription()
        { }

        public PropertyInfo Property { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public BindingProperty Binding { get; internal set; }
    }
}