using System.Reflection;

namespace Pandora.Interactions.UI.Design
{
    internal class PropertySetterContainer
    {
        public PropertySetterContainer()
        { }

        public string BindingName { get; set; }

        public int Duration { get; set; }
                
        public string Value { get; set; }

        public int Start { get; set; }
        public string PublicBindingName { get; internal set; }
    }
}