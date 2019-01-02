using System.Reflection;

namespace Pandora.Interactions.UI.Design
{
    internal class PropertySetterContainer
    {
        public PropertySetterContainer()
        { }

        public int Duration { get; set; }

        public PropertyInfo Property { get; set; }

        public string Value { get; set; }

        public int Start { get; set; }
    }
}